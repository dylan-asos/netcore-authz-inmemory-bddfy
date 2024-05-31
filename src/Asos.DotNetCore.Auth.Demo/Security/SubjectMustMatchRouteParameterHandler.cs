using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asos.DotNetCore.Auth.Demo.Security;

public class SubjectMustMatchRouteParameterHandler : AuthorizationHandler<SubjectMustMatchRouteParameterRequirement>
{
    private readonly ILogger<SubjectMustMatchRouteParameterHandler> _logger;

    public SubjectMustMatchRouteParameterHandler(ILogger<SubjectMustMatchRouteParameterHandler> logger)
    {
        _logger = logger;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SubjectMustMatchRouteParameterRequirement requirement)
    {
        var identifierClaim = context.User.Claims.FirstOrDefault(c => c.Type == requirement.IdentifierClaim);
        if (identifierClaim == null)
        {
            return Task.CompletedTask;
        }
        
        var claimValues = GetValueFromClaim(context.User, requirement.IdentifierClaim);
        var routeValue = GetValueFromRoute(context.Resource, requirement.RouteName);
        
        if (claimValues.Contains(routeValue))
        {
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogInformation("Authorization Failed - Claim '{ClaimType}' did not match '{RouteValue}' route value", requirement.IdentifierClaim, requirement.RouteName);
        }

        return Task.CompletedTask;
    }
    
    private static IEnumerable<object> GetValueFromClaim(ClaimsPrincipal principal, string claimType)
    {
        return principal?
                   .Claims
                   .Where(x => x.Type == claimType)
                   .Select(x => x.Value)
               ?? Enumerable.Empty<object>();
    }
    
    private string? GetValueFromRoute(object? resource, string routeKey)
    {
        var routeValues = resource switch
        {
            ActionContext actionContext => actionContext.RouteData.Values,
            HttpContext httpContext => httpContext.Request.RouteValues,
            _ => throw new InvalidOperationException($"Unexpected resource type {resource?.GetType().FullName ?? "<null>"}")
        };

        if (routeValues.TryGetValue(routeKey, out var value))
        {
            return value as string;
        }
        return null;
    }
}
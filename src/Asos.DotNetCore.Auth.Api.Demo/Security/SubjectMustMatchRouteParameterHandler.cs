
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asos.DotNetCore.Auth.Api.Demo.Security
{
    public class SubjectMustMatchRouteParameterHandler : AuthorizationHandler<SubjectMustMatchRouteParameterRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SubjectMustMatchRouteParameterRequirement requirement)
        {
            var identifierClaim = context.User.Claims.FirstOrDefault(c => c.Type == requirement.IdentifierClaim);
            if (identifierClaim == null)
            {
                return Task.CompletedTask;
            }            

            var actionContext = context.Resource as ActionContext;

            var routeValueAndClaimMatch = DoRouteValueAndClaimTypeValueMatch(context.User, actionContext,
                requirement.RouteName, identifierClaim.Type);

            if (routeValueAndClaimMatch)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        public bool DoRouteValueAndClaimTypeValueMatch(ClaimsPrincipal principal, ActionContext actionContext, string routeParameterName, string claimsType)
        {
            var requestedRouteValue = actionContext.RouteData.Values.ContainsKey(routeParameterName)
                ? actionContext.RouteData.Values[routeParameterName] as string
                : null;

            return !string.IsNullOrWhiteSpace(requestedRouteValue) &&
                   HasClaimAndValue(principal, claimsType, requestedRouteValue);
        }

        public bool HasClaimAndValue(ClaimsPrincipal principal, string claimsType, string expectedClaimValue)
        {
            var claimValue = principal.Claims.FirstOrDefault(x => x.Type == claimsType);

            return claimValue != null && claimValue.Value.Equals(expectedClaimValue, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
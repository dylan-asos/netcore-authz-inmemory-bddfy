using Microsoft.AspNetCore.Authorization;

namespace Asos.DotNetCore.Auth.Demo.Security;

public class SubjectMustMatchRouteParameterRequirement : IAuthorizationRequirement
{
    public SubjectMustMatchRouteParameterRequirement(string identifierClaim, string routeName)
    {
        IdentifierClaim = identifierClaim;
        RouteName = routeName;
    }

    public string IdentifierClaim { get; }

    public string RouteName { get; }
}
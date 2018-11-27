﻿using Microsoft.AspNetCore.Authorization;

namespace Asos.DotNetCore.Auth.Api.Demo.Security
{
    public class SubjectMustMatchRouteParameterRequirement : IAuthorizationRequirement
    {
        public SubjectMustMatchRouteParameterRequirement(string identifierClaim, string routeName)
        {
            IdentifierClaim = identifierClaim;
            RouteName = routeName;
        }

        public string IdentifierClaim { get; set; }

        public string RouteName { get; set; }
    }
}

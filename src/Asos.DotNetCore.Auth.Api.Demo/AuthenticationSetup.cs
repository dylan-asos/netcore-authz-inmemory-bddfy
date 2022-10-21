using System.IdentityModel.Tokens.Jwt;
using Asos.DotNetCore.Auth.Api.Demo.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Asos.DotNetCore.Auth.Api.Demo;

public static class AuthenticationSetup
{
    public static void Initialize(IServiceCollection services)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters.NameClaimType = "sub";
                options.MetadataAddress = "https://login.microsoftonline.com/common/.well-known/openid-configuration";
                options.Audience = "https://myapi.audience.com";
            });

        services.AddSingleton<IAuthorizationHandler, SubjectMustMatchRouteParameterHandler>();

        services.AddAuthorization(
            options =>
            {
                options.AddPolicy("RouteMustMatchSubject", builder => builder.Requirements.Add(
                    new SubjectMustMatchRouteParameterRequirement("sub", "customerId")));
            });
    }
}
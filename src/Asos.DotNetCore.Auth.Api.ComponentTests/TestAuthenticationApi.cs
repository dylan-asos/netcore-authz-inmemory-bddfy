using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Asos.DotNetCore.Auth.Api.Demo;
using Asos.DotNetCore.Auth.Api.Demo.Orders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Asos.DotNetCore.Auth.Api.ComponentTests
{
public class TestAuthenticationApi
{
    public TestAuthenticationApi()
    {
        var builder = new WebHostBuilder()
            .UseStartup<Startup>()
            .ConfigureTestServices(services =>
            {
                services.AddTransient<IOrderRetriever, MockOrderRetriever>();
            })
            .ConfigureServices(services =>
            {
                services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        SignatureValidator = (token, parameters) => new JwtSecurityToken(token)
                    };
                    options.Audience = TestAuthorisationConstants.Audience;
                    options.Authority = TestAuthorisationConstants.Issuer;
                    options.BackchannelHttpHandler = new MockBackchannel();
                    options.MetadataAddress = "https://inmemory.microsoft.com/common/.well-known/openid-configuration";
                });
            });

        var server = new TestServer(builder);

        Client = server.CreateClient();
        Client.BaseAddress = new Uri("http://localhost:5012");
    }

    public HttpClient Client { get; set; }
}
}
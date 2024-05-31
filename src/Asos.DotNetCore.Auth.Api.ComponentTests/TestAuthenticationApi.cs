using Asos.DotNetCore.Auth.Demo.Orders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Asos.DotNetCore.Auth.Api.ComponentTests
{
    public class TestAuthenticationApi : WebApplicationFactory<Program>
    {
        private const string HostUrl = "https://localhost:5811";
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = TestAuthorisationConstants.Audience,
                        SignatureValidator = GetSignatureValidator
                    };
                    options.Authority = TestAuthorisationConstants.Issuer;
                    options.MetadataAddress =
                        "https://inmemory.microsoft.com/common/.well-known/openid-configuration";                    
                    options.BackchannelHttpHandler = new MockBackchannel();
                });
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IOrderRetriever, MockOrderRetriever>();
            });
            
            builder.UseUrls(HostUrl);
        }
        
        private static JsonWebToken GetSignatureValidator(
            string token, 
            TokenValidationParameters validationParameters)
        {
            return new JsonWebToken(token);
        }
    }
}
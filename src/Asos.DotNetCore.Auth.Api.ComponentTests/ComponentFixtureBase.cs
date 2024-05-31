using NUnit.Framework;

namespace Asos.DotNetCore.Auth.Api.ComponentTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class ComponentFixtureBase
    {
        private readonly TestAuthenticationApi _apiApplication = new();
        protected readonly ComponentTestContext ComponentContext;

        protected ComponentFixtureBase()
        {
            ComponentContext = new ComponentTestContext
            {
                Client = _apiApplication.CreateClient(), 

                TokenBuilder =
                    new BearerTokenBuilder()
                        .ForAudience(TestAuthorisationConstants.Audience)
                        .IssuedBy(TestAuthorisationConstants.Issuer)
                        .WithSigningCertificate(EmbeddedResourceReader.GetCertificate())                    
            };
        }
    }
}
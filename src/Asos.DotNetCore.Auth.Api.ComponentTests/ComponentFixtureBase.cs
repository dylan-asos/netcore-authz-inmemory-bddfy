using NUnit.Framework;

namespace Asos.DotNetCore.Auth.Api.ComponentTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class ComponentFixtureBase 
    {
        public ComponentTestContext ComponentContext;

        public ComponentFixtureBase()
        {
            ComponentContext = new ComponentTestContext
                {
                    Client = new TestAuthenticationApi().Client, 

                    TokenBuilder =
                        new BearerTokenBuilder()
                            .ForAudience(TestAuthorisationConstants.Audience)
                            .IssuedBy(TestAuthorisationConstants.Issuer)
                            .WithSigningCertificate(EmbeddedResourceReader.GetCertificate())                    
                };
        }
    }
}
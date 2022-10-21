using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Asos.DotNetCore.Auth.Api.ComponentTests.Features
{
    [Story(AsA = "test demonstration",
        IWant = "to call the basic authorization endpoint with a token",
        SoThat = "i can demonstrate the authorization attribute")]
    public class SimpleAuthorisationTest : ComponentFixtureBase
    {
        private HttpResponseMessage _response;

        private string _token;

        [Test]
        public void The_Secured_Endpoint_Can_Be_Accessed_By_Bearer_Token()
        {
            this.Given(_ => ATokenIsSelected())
                .When(_ => TheEndpointIsCalled())
                .Then(_ => TheResponseIs(HttpStatusCode.OK))
                .BDDfy();
        }

        private void ATokenIsSelected()
        {                    
            _token = ComponentContext.TokenBuilder.BuildToken();
        }

        private async Task TheEndpointIsCalled()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "demo/basic-authz");
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);

            _response = await ComponentContext.Client.SendAsync(request);
        }

        private void TheResponseIs(HttpStatusCode ok)
        {
            Assert.AreEqual(ok, _response.StatusCode);
        }
    }
}
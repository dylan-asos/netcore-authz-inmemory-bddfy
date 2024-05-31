using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Asos.DotNetCore.Auth.Demo.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Asos.DotNetCore.Auth.Api.ComponentTests.Features
{
    [Story(AsA = "test demonstration",
        IWant = "to call the basic authorization endpoint with a token",
        SoThat = "i can verify routes are locked down by claims")]
    public class AuthorisedRouteTests : ComponentFixtureBase
    {
        private HttpResponseMessage _response;

        private string _token;

        private OrderDetailResponse _orderDetailResponse;

        [Test]
        public void An_Endpoint_With_Customer_Policy_Allows_Matching_Claim_And_Route()
        {
            const string customerId = "123456";

            this.Given(_ => ACustomerTokenIsSelectedWithId(customerId))
                .When(_ => TheEndpointIsCalledForCustomer(customerId))
                .Then(_ => TheResponseIs(HttpStatusCode.OK))
                .And(_ => AValidOrderResponseIsReturnedForTheCustomer(customerId))
                .And(_ => TheResponseContainsClaim("groups", "group1"))
                .And(_ => TheResponseContainsClaim("groups", "group2"))
                .BDDfy();
        }

        [Test]
        public void A_Forbidden_Result_Is_Returned_When_Mismatch_Of_Claim_And_Route()
        {
            this.Given(_ => ACustomerTokenIsSelectedWithId("123456"))
                .When(_ => TheEndpointIsCalledForCustomer("654321"))
                .Then(_ => TheResponseIs(HttpStatusCode.Forbidden))
                .BDDfy();
        }

        private void ACustomerTokenIsSelectedWithId(string customerId)
        {                    
            _token = ComponentContext
                .TokenBuilder
                .ForSubject(customerId)
                .WithClaim("groups", "group1")
                .WithClaim("groups", "group2")
                .BuildToken();
        }

        private async Task AValidOrderResponseIsReturnedForTheCustomer(string customerId)
        {
            var jsonContent = await _response.Content.ReadAsStringAsync();

            _orderDetailResponse = JsonConvert.DeserializeObject<OrderDetailResponse>(jsonContent);

            Assert.That(_orderDetailResponse.Order.CustomerId == Convert.ToInt32(customerId));
        }

        private async Task TheEndpointIsCalledForCustomer(string customerId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"demo/route-based/{customerId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            
            _response = await ComponentContext.Client.SendAsync(request);
        }

        private void TheResponseIs(HttpStatusCode expected)
        {
            Assert.That(expected == _response.StatusCode);
        }

        private void TheResponseContainsClaim(string claimType, string claimValue)
        {
            var claimDetails = _orderDetailResponse.Claims.FirstOrDefault(claim =>
                claim.ClaimType == claimType && claim.ClaimValue == claimValue);

            Assert.That(claimDetails != null);
        }
    }
}
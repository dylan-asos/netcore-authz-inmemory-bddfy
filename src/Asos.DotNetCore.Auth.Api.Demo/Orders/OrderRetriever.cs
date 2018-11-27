using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Asos.DotNetCore.Auth.Api.Demo.Orders
{
    public interface IOrderRetriever
    {
        Task<Order> GetOrderForCustomer(int customerId);
    }

    public class OrderRetriever : IOrderRetriever
    {
        private readonly HttpClient _client;

        public OrderRetriever(HttpClient client)
        {
            _client = client;
        }

        public async Task<Order> GetOrderForCustomer(int customerId)
        {
            var requestUri = string.Format("/customers/{0}/orders/orderId", customerId);
           
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Order>(content);
        }
    }
}
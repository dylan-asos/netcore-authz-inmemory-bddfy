using System.Text.Json;

namespace Asos.DotNetCore.Auth.Demo.Orders;

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
        var requestUri = $"/customers/{customerId}/orders/orderId";

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var response = await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<Order>(content);
    }
}
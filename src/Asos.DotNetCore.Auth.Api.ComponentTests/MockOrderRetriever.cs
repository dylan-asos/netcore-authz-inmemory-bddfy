using System;
using System.Threading.Tasks;
using Asos.DotNetCore.Auth.Api.Demo.Orders;

namespace Asos.DotNetCore.Auth.Api.ComponentTests
{
    public class MockOrderRetriever : IOrderRetriever
    {
        public Task<Order> GetOrderForCustomer(int customerId)
        {
            var order = new Order()
            {
                OrderId = 12345,
                OrderDate = DateTime.Today,
                CustomerId = customerId
            };

            return Task.FromResult(order);
        }
    }
}
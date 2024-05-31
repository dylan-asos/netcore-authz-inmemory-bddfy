using System;

namespace Asos.DotNetCore.Auth.Demo.Orders;

public class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public int CustomerId { get; set; }
}
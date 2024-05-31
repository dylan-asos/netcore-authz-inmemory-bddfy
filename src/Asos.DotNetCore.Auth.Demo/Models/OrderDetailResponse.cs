using Asos.DotNetCore.Auth.Demo.Orders;

namespace Asos.DotNetCore.Auth.Demo.Models;

public class OrderDetailResponse
{
    public Order Order { get; init; }

    public List<ClaimDetailResponse> Claims { get; init; }
}
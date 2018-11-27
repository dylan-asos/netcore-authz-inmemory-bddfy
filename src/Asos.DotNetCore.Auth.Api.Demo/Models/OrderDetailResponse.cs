using System.Collections.Generic;
using Asos.DotNetCore.Auth.Api.Demo.Orders;

namespace Asos.DotNetCore.Auth.Api.Demo.Models
{
    public class OrderDetailResponse
    {
        public Order Order { get; set; }

        public List<ClaimDetailResponse> Claims {get; set; }
    }
}
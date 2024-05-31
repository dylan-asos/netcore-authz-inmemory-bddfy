using Asos.DotNetCore.Auth.Demo.Models;
using Asos.DotNetCore.Auth.Demo.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asos.DotNetCore.Auth.Demo.Controllers;

[Route("demo")]
[ApiController]
public class DemoController : Controller
{
    private readonly IOrderRetriever _orderRetriever;

    public DemoController(IOrderRetriever orderRetriever)
    {
        _orderRetriever = orderRetriever;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("basic-authz")]
    public IActionResult BasicAuthorization()
    {
        return Ok();
    }

    [Authorize("RouteMustMatchSubject")]
    [HttpGet("route-based/{customerId}")]
    public async Task<IActionResult> RouteBasedLogic(int customerId)
    {
        var order = await _orderRetriever.GetOrderForCustomer(customerId);
        var claimResults = User.Claims.Select(claim => new ClaimDetailResponse(claim.Type, claim.Value)).ToList();

        var response = new OrderDetailResponse
        {
            Order = order, Claims = claimResults
        };

        return Ok(response);
    }
}
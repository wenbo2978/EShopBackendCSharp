using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    private string GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? throw new UnauthorizedAccessException("Missing user id claim.");

    // GET /api/orders/me
    [HttpGet("me")]
    public async Task<IActionResult> GetMyOrders()
    {
        var list = await _service.GetMyOrdersAsync(GetUserId());
        return Ok(list);
    }

    // GET /api/orders/me/{orderId}
    [HttpGet("me/{orderId:int}")]
    public async Task<IActionResult> GetMyOrderById(int orderId)
    {
        var (ok, error, data) = await _service.GetMyOrderByIdAsync(GetUserId(), orderId);
        if (!ok)
        {
            if (error == "Order not found.") return NotFound(error);
            if (error == "Forbidden.") return Forbid();
            return BadRequest(error);
        }

        return Ok(data);
    }

    // POST /api/orders/checkout
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout()
    {
        var (ok, error, data) = await _service.CheckoutAsync(GetUserId());
        if (!ok) return BadRequest(error);

        return Ok(data);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/cart")]
[Authorize] // must have JWT
public class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    private string GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? throw new UnauthorizedAccessException("Missing user id claim.");

    // GET /api/cart/me
    [HttpGet("me")]
    public async Task<IActionResult> GetMyCart()
    {
        var cart = await _service.GetMyCartAsync(GetUserId());
        return Ok(cart);
    }

    // POST /api/cart/items
    [HttpPost("items")]
    public async Task<IActionResult> AddItem(AddCartItemRequest req)
    {
        var (ok, error, cart) = await _service.AddItemAsync(GetUserId(), req);
        if (!ok) return BadRequest(error);
        return Ok(cart);
    }

    // PUT /api/cart/items/{cartItemId}
    [HttpPut("items/{cartItemId:int}")]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, UpdateCartItemQuantityRequest req)
    {
        var (ok, error, cart) = await _service.UpdateItemQuantityAsync(GetUserId(), cartItemId, req.Quantity);
        if (!ok)
        {
            if (error?.StartsWith("Forbidden") == true) return Forbid();
            if (error == "Cart item not found.") return NotFound(error);
            return BadRequest(error);
        }

        return Ok(cart);
    }

    // DELETE /api/cart/items/{cartItemId}
    [HttpDelete("items/{cartItemId:int}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        var (ok, error) = await _service.RemoveItemAsync(GetUserId(), cartItemId);
        if (!ok)
        {
            if (error?.StartsWith("Forbidden") == true) return Forbid();
            if (error == "Cart item not found.") return NotFound(error);
            return BadRequest(error);
        }

        return NoContent();
    }
}

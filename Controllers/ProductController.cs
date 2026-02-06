using Microsoft.AspNetCore.Mvc;

public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    // GET /api/products?categoryId=1&search=coffee
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? categoryId, [FromQuery] string? search)
    {
        var list = await _service.GetAllAsync(categoryId, search);
        return Ok(list);
    }

    // GET /api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null) return NotFound("Product not found.");
        return Ok(product);
    }

    // POST /api/products
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest req)
    {
        var (ok, error, data) = await _service.CreateAsync(req);
        if (!ok) return BadRequest(error);

        return CreatedAtAction(nameof(GetById), new { id = data!.Id }, data);
    }

    // PUT /api/products/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest req)
    {
        var (ok, error, data) = await _service.UpdateAsync(id, req);
        if (!ok)
        {
            if (error == "Product not found.") return NotFound(error);
            return BadRequest(error);
        }

        return Ok(data);
    }

    // DELETE /api/products/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (ok, error) = await _service.DeleteAsync(id);
        if (!ok) return NotFound(error);

        return NoContent();
    }
}
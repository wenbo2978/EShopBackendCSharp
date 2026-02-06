using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    // GET /api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    // GET /api/categories/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null) return NotFound("Category not found.");
        return Ok(category);
    }

    // POST /api/categories
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest req)
    {
        var (ok, error, data) = await _service.CreateAsync(req);
        if (!ok) return BadRequest(error);

        // 201 Created with Location header
        return CreatedAtAction(nameof(GetById), new { id = data!.Id }, data);
    }

    // PUT /api/categories/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryRequest req)
    {
        var (ok, error, data) = await _service.UpdateAsync(id, req);
        if (!ok)
        {
            if (error == "Category not found.") return NotFound(error);
            return BadRequest(error);
        }

        return Ok(data);
    }

    // DELETE /api/categories/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (ok, error) = await _service.DeleteAsync(id);
        if (!ok) return NotFound(error);

        return NoContent();
    }
}
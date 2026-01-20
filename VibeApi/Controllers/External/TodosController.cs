using Microsoft.AspNetCore.Mvc;
using VibeApi.Models.External;
using VibeApi.Services.External;

namespace VibeApi.Controllers.External;

[ApiController]
[Route("api/external/[controller]")]
public class TodosController : ControllerBase
{
    private readonly IExternalTodoService _todoService;

    public TodosController(IExternalTodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExternalTodo>>> GetTodos()
    {
        var todos = await _todoService.GetTodosAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExternalTodo>> GetTodo(int id)
    {
        var todo = await _todoService.GetTodoByIdAsync(id);
        return todo == null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<ExternalTodo>> CreateTodo(ExternalTodo todo)
    {
        var createdTodo = await _todoService.CreateTodoAsync(todo);
        return CreatedAtAction(nameof(GetTodo), new { id = createdTodo.Id }, createdTodo);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExternalTodo>> UpdateTodo(int id, ExternalTodo todo)
    {
        var updatedTodo = await _todoService.UpdateTodoAsync(id, todo);
        return updatedTodo == null ? NotFound() : Ok(updatedTodo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var success = await _todoService.DeleteTodoAsync(id);
        return success ? NoContent() : NotFound();
    }
}
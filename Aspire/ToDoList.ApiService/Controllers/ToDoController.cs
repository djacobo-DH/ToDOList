using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using toDoList.ApiService.Data;
using toDoList.ApiService.Models;


namespace toDoList.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] TodoItem item)
        {
            _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodo), new { id = item.Id }, item);
        }

        // READ ONE
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodo(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);

            if (todo == null)
                return NotFound();

            return todo;
        }

        // READ ALL
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetAll()
        {
            return _context.ToDoItems.ToList();
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            var existingTodo = await _context.ToDoItems.FindAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            existingTodo.Title = item.Title;
            existingTodo.IsCompleted = item.IsCompleted;

            _context.Entry(existingTodo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ToDoItems.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // TEST DATABASE CONNECTION
        [HttpGet("testdb")]
        public IActionResult TestDatabase()
        {
            try
            {
                bool canConnect = _context.Database.CanConnect();

                if (canConnect)
                    return Ok("✅ Conectado correctamente a la base de datos.");
                else
                    return StatusCode(500, "❌ No se pudo conectar a la base de datos.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error al conectar con la BD: {ex.Message}");
            }
        }

    }

    
}
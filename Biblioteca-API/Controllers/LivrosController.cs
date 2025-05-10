using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LivrosController(AppDbContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livros>>> GetLivros()
        {
            return await _context.TabelaLivros.ToListAsync();
        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<Livros>> GetLivro(int id)
        {
            var livro = await _context.TabelaLivros.FindAsync(id);
            return livro == null ? NotFound() : Ok(livro);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<Livros>> CreateLivro(Livros livro)
        {
            _context.TabelaLivros.Add(livro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLivro), new { id = livro.Id }, livro);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLivro(int id, Livros livro)
        {
            if (id != livro.Id) return BadRequest();

            _context.Entry(livro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TabelaLivros.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivro(int id)
        {
            var livro = await _context.TabelaLivros.FindAsync(id);
            if (livro == null) return NotFound();

            _context.TabelaLivros.Remove(livro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

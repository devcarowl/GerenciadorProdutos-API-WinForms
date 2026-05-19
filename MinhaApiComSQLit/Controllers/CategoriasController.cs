using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Services;

namespace MinhaApiComSQLite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _categoriaService.ListarTodasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var categoria = await _categoriaService.BuscarPorIdAsync(id);
            if (categoria == null) return NotFound(new { mensagem = "Categoria não encontrada." });
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CategoriaDTO categoriaDto)
        {
            try
            {
                var novaCategoria = await _categoriaService.CriarAsync(categoriaDto);
                return CreatedAtAction(nameof(GetPorId), new { id = novaCategoria.Id }, novaCategoria);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, CategoriaDTO categoriaDto)
        {
            var sucesso = await _categoriaService.AtualizarAsync(id, categoriaDto);
            if (!sucesso) return NotFound(new { mensagem = "Categoria não encontrada." });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _categoriaService.DeletarAsync(id);
            if (!sucesso) return NotFound(new { mensagem = "Categoria não encontrada." });
            return NoContent();
        }
    }
}

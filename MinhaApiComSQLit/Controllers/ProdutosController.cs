using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Services;

namespace MinhaApiComSQLite.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Rota automática: api/produtos
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _service;

        public ProdutosController(IProdutoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
        {
            // O [FromQuery] permite que o usuário digite na URL: api/produtos?pagina=1&tamanho=5
            var produtos = await _service.ListarTodosPaginadosAsync(pagina, tamanho);
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var produto = await _service.BuscarPorIdAsync(id);
            if (produto == null) return NotFound(new { mensagem = "Produto não encontrado." });
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                var novoProduto = await _service.CriarAsync(produtoDto);
                return CreatedAtAction(nameof(GetPorId), new { id = novoProduto.Id }, novoProduto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                var atualizado = await _service.AtualizarAsync(id, produtoDto);
                if (!atualizado) return NotFound(new { mensagem = "Produto não encontrado para atualização." });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletado = await _service.DeletarAsync(id);
            if (!deletado) return NotFound(new { mensagem = "Produto não encontrado para exclusão." });
            return NoContent();
        }
    }
}

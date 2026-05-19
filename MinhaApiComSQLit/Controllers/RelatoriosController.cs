using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.Services;

namespace MinhaApiComSQLite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public RelatoriosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet("estatisticas")]
        public async Task<IActionResult> GetEstatisticas()
        {
            var relatorio = await _produtoService.ObterRelatorioEstatisticasAsync();
            return Ok(relatorio);
        }
    }
}

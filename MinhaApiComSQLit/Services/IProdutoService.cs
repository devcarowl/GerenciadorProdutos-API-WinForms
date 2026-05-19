using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Services
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> ListarTodosPaginadosAsync(int pagina, int tamanho);
        Task<Produto?> BuscarPorIdAsync(int id);
        Task<Produto> CriarAsync(ProdutoDTO produtoDto);
        Task<bool> AtualizarAsync(int id, ProdutoDTO produtoDto);
        Task<bool> DeletarAsync(int id);

       
        Task<object> ObterRelatorioEstatisticasAsync();
    }
}

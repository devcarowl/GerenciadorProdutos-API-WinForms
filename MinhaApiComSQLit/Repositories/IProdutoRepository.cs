using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ListarTodosPaginadosAsync(int pagina, int tamanho);
        Task<Produto?> BuscarPorIdAsync(int id);
        Task<Produto> CriarAsync(Produto produto);
        Task<bool> AtualizarAsync(Produto produto);
        Task<bool> DeletarAsync(int id);

        
        Task<IEnumerable<Produto>> ListarTodosAsync();
    }
}

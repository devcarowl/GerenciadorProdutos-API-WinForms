using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Repositories
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> ListarTodasAsync();
        Task<Categoria?> BuscarPorIdAsync(int id);
        Task<Categoria> CriarAsync(Categoria categoria);
        Task<bool> AtualizarAsync(Categoria categoria);
        Task<bool> DeletarAsync(int id);
    }
}

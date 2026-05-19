using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ListarTodasAsync();
        Task<Categoria?> BuscarPorIdAsync(int id);
        Task<Categoria> CriarAsync(CategoriaDTO categoriaDto);
        Task<bool> AtualizarAsync(int id, CategoriaDTO categoriaDto);
        Task<bool> DeletarAsync(int id);
    }
}

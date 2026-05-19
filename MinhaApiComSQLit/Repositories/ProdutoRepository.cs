using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Data;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> ListarTodosPaginadosAsync(int pagina, int tamanho)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nome)
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .ToListAsync();
        }

        public async Task<Produto?> BuscarPorIdAsync(int id)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produto> CriarAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<bool> AtualizarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var produto = await BuscarPorIdAsync(id);
            if (produto == null) return false;

            _context.Produtos.Remove(produto);
            return await _context.SaveChangesAsync() > 0;
        }

        
        public async Task<IEnumerable<Produto>> ListarTodosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }
    }
}

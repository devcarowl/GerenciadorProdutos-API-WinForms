using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Repositories;

namespace MinhaApiComSQLite.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<CategoriaService> _logger;

        public CategoriaService(ICategoriaRepository categoriaRepository, ILogger<CategoriaService> logger)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Categoria>> ListarTodasAsync()
        {
            _logger.LogInformation("Listando todas as categorias.");
            return await _categoriaRepository.ListarTodasAsync();
        }

        public async Task<Categoria?> BuscarPorIdAsync(int id)
        {
            _logger.LogInformation($"Buscando categoria com ID: {id}");
            return await _categoriaRepository.BuscarPorIdAsync(id);
        }

        public async Task<Categoria> CriarAsync(CategoriaDTO categoriaDto)
        {
            _logger.LogInformation($"Tentando criar categoria: {categoriaDto.Nome}");

            var categorias = await _categoriaRepository.ListarTodasAsync();
            if (categorias.Any(c => c.Nome.Equals(categoriaDto.Nome, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Já existe uma categoria cadastrada com este nome.");
            }

            var categoria = new Categoria { Nome = categoriaDto.Nome };
            return await _categoriaRepository.CriarAsync(categoria);
        }

        public async Task<bool> AtualizarAsync(int id, CategoriaDTO categoriaDto)
        {
            _logger.LogInformation($"Tentando atualizar categoria ID: {id}");
            var categoria = await _categoriaRepository.BuscarPorIdAsync(id);
            if (categoria == null) return false;

            categoria.Nome = categoriaDto.Nome;
            return await _categoriaRepository.AtualizarAsync(categoria);
        }

        public async Task<bool> DeletarAsync(int id)
        {
            _logger.LogInformation($"Tentando deletar categoria ID: {id}");
            return await _categoriaRepository.DeletarAsync(id);
        }
    }
}

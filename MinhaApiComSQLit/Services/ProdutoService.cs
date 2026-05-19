using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Repositories;

namespace MinhaApiComSQLite.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<ProdutoService> _logger;

        public ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, ILogger<ProdutoService> logger)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Produto>> ListarTodosPaginadosAsync(int pagina, int tamanho)
        {
            _logger.LogInformation($"Buscando produtos paginados. Página: {pagina}, Tamanho: {tamanho}");
            return await _produtoRepository.ListarTodosPaginadosAsync(pagina, tamanho);
        }

        public async Task<Produto?> BuscarPorIdAsync(int id)
        {
            _logger.LogInformation($"Buscando produto com ID: {id}");
            return await _produtoRepository.BuscarPorIdAsync(id);
        }

        public async Task<Produto> CriarAsync(ProdutoDTO produtoDto)
        {
            _logger.LogInformation($"Tentando criar produto: {produtoDto.Nome}");

            if (produtoDto.Preco <= 0)
            {
                throw new ArgumentException("O preço do produto deve ser maior que zero.");
            }

            var categoriaExiste = await _categoriaRepository.BuscarPorIdAsync(produtoDto.CategoriaId);
            if (categoriaExiste == null)
            {
                throw new ArgumentException("A categoria informada não existe.");
            }

            string nomeFormatado = FormatarPrimeiraLetraMaiuscula(produtoDto.Nome);

            var produto = new Produto
            {
                Nome = nomeFormatado,
                Preco = produtoDto.Preco,
                CategoriaId = produtoDto.CategoriaId
            };

            var novoProduto = await _produtoRepository.CriarAsync(produto);
            _logger.LogInformation($"Produto ID {novoProduto.Id} criado com sucesso.");
            return novoProduto;
        }

        public async Task<bool> AtualizarAsync(int id, ProdutoDTO produtoDto)
        {
            _logger.LogInformation($"Tentando atualizar produto com ID: {id}");

            var produto = await _produtoRepository.BuscarPorIdAsync(id);
            if (produto == null) return false;

            if (produtoDto.Preco <= 0)
            {
                throw new ArgumentException("O preço do produto deve ser maior que zero.");
            }

            var categoriaExiste = await _categoriaRepository.BuscarPorIdAsync(produtoDto.CategoriaId);
            if (categoriaExiste == null)
            {
                throw new ArgumentException("A categoria informada não existe.");
            }

            produto.Nome = FormatarPrimeiraLetraMaiuscula(produtoDto.Nome);
            produto.Preco = produtoDto.Preco;
            produto.CategoriaId = produtoDto.CategoriaId;

            await _produtoRepository.AtualizarAsync(produto);
            _logger.LogInformation($"Produto ID {id} atualizado com sucesso.");
            return true;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            _logger.LogInformation($"Tentando deletar produto com ID: {id}");
            var produto = await _produtoRepository.BuscarPorIdAsync(id);
            if (produto == null) return false;

            await _produtoRepository.DeletarAsync(id);
            _logger.LogInformation($"Produto ID {id} removido.");
            return true;
        }

        
        public async Task<object> ObterRelatorioEstatisticasAsync()
        {
            _logger.LogInformation("Gerando relatório de estatísticas dos produtos.");

            var produtos = await _produtoRepository.ListarTodosAsync();

            if (!produtos.Any())
            {
                return new
                {
                    totalProdutos = 0,
                    mediaPrecos = 0.00,
                    valorTotalEstoque = 0.00
                };
            }

            var totalProdutos = produtos.Count();
            var mediaPrecos = produtos.Average(p => p.Preco);
            var valorTotalEstoque = produtos.Sum(p => p.Preco);

            return new
            {
                totalProdutos = totalProdutos,
                mediaPrecos = Math.Round(mediaPrecos, 2),
                valorTotalEstoque = valorTotalEstoque
            };
        }

        private string FormatarPrimeiraLetraMaiuscula(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return texto;
            texto = texto.Trim();
            return char.ToUpper(texto[0]) + texto.Substring(1);
        }
    }
}

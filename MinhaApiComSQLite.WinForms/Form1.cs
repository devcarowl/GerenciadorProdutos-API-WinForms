using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MinhaApiComSQLite.WinForms
{
    public partial class Form1 : Form
    {
        // Já conferi no seu print que a porta HTTPS do seu projeto é exatamente a 7142!
        private readonly string _apiUrl = "https://localhost:7142/api/";
        private readonly HttpClient _httpClient;

        public Form1()
        {
            InitializeComponent();

            // Ignora checagem estrita de SSL local para evitar erros em ambiente de testes
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _httpClient = new HttpClient(handler);

            // Vincula os métodos lógicos aos botões visuais
            btnSalvar.Click += async (s, e) => await CriarProdutoAsync();
            btnAtualizar.Click += async (s, e) => await AtualizarProdutoAsync();
            btnExcluir.Click += async (s, e) => await ExcluirProdutoAsync();
            dgvProdutos.SelectionChanged += DgvProdutos_SelectionChanged;

            // Executa a busca de informações logo após a janela carregar
            this.Load += async (s, e) => await InicializarDadosAsync();
        }

        private async Task InicializarDadosAsync()
        {
            try
            {
                await CarregarCategoriasAsync();
                await CarregarProdutosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar com a API local. Certifique-se de que ela está executando!\nErro: {ex.Message}", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CarregarCategoriasAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}Categorias");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var categorias = JsonConvert.DeserializeObject<List<CategoriaViewModel>>(json);

                cbCategorias.DataSource = categorias;
                cbCategorias.DisplayMember = "Nome";
                cbCategorias.ValueMember = "Id";
            }
        }

        private async Task CarregarProdutosAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}Produtos?pagina=1&tamanho=100");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var produtos = JsonConvert.DeserializeObject<List<ProdutoViewModel>>(json);
                dgvProdutos.DataSource = produtos;

                if (dgvProdutos.Columns["Id"] != null) dgvProdutos.Columns["Id"].Width = 60;
                if (dgvProdutos.Columns["CategoriaId"] != null) dgvProdutos.Columns["CategoriaId"].Visible = false;
            }
        }

        private async Task CriarProdutoAsync()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || !decimal.TryParse(txtPreco.Text, out decimal preco))
            {
                MessageBox.Show("Preencha o nome e o preço corretamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var produtoDto = new
            {
                Nome = txtNome.Text.Trim(),
                Preco = preco,
                CategoriaId = (int)cbCategorias.SelectedValue
            };

            var json = JsonConvert.SerializeObject(produtoDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}Produtos", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                await CarregarProdutosAsync();
            }
            else
            {
                MessageBox.Show("Erro ao cadastrar produto. Garanta que o preço seja maior que 0.", "Erro na API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task AspNetCoreUpdateAsync()
        {
            if (dgvProdutos.CurrentRow?.DataBoundItem is ProdutoViewModel produtoSelecionado)
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text) || !decimal.TryParse(txtPreco.Text, out decimal preco)) return;

                var produtoDto = new
                {
                    Nome = txtNome.Text.Trim(),
                    Preco = preco,
                    CategoriaId = (int)cbCategorias.SelectedValue
                };

                var json = JsonConvert.SerializeObject(produtoDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiUrl}Produtos/{produtoSelecionado.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    await CarregarProdutosAsync();
                }
            }
        }

        private async Task AtualizarProdutoAsync()
        {
            await AspNetCoreUpdateAsync();
        }

        private async Task ExcluirProdutoAsync()
        {
            if (dgvProdutos.CurrentRow?.DataBoundItem is ProdutoViewModel produtoSelecionado)
            {
                var confirma = MessageBox.Show($"Deseja excluir permanentemente o produto '{produtoSelecionado.Nome}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirma == DialogResult.Yes)
                {
                    var response = await _httpClient.DeleteAsync($"{_apiUrl}Produtos/{produtoSelecionado.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Produto removido com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        await CarregarProdutosAsync();
                    }
                }
            }
        }

        private void DgvProdutos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow?.DataBoundItem is ProdutoViewModel produto)
            {
                txtNome.Text = produto.Nome;
                txtPreco.Text = produto.Preco.ToString("F2");
                cbCategorias.SelectedValue = produto.CategoriaId;
            }
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtPreco.Clear();
            if (cbCategorias.Items.Count > 0) cbCategorias.SelectedIndex = 0;
        }
    }

    public class CategoriaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class ProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int CategoriaId { get; set; }
    }
}

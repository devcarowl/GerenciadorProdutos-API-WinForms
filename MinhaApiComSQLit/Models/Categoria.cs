using System.ComponentModel.DataAnnotations;

namespace MinhaApiComSQLite.Models
{
    public class Categoria
    {
        [Key] // Diz ao banco que este é o Id primário e auto-incrementado
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode passar de 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        // Propriedade de navegação: avisa o Entity Framework que uma categoria pode ter vários produtos
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}

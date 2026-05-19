using System.ComponentModel.DataAnnotations;

namespace MinhaApiComSQLite.DTOs
{
    public class CategoriaDTO
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode passar de 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;
    }
}

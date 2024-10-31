using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PapelariaMVC.Models
{
    public class Venda
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O cliente é obrigatório.")]

        [ForeignKey("ClienteId")]
        public Guid ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        [ForeignKey("ProdutoId")]

        [Required(ErrorMessage = "A lista de produtos é obrigatória.")]
        public Guid ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        [Required(ErrorMessage = "O valor total é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor total deve ser maior que zero.")]
        public decimal ValorTotal { get; set; }

        public DateTime DataEmissao { get; set; } = DateTime.Now; // Data atual por padrão
    }
}

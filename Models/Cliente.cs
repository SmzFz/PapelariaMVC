using System.ComponentModel.DataAnnotations;

namespace PapelariaMVC.Models
{
    public class Cliente
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF ou CNPJ é obrigatório.")]
        [RegularExpression(@"^\d{11}$|^\d{14}$", ErrorMessage = "CPF deve ter 11 dígitos e CNPJ 14 dígitos.")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Número de telefone inválido.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

      
    }
}
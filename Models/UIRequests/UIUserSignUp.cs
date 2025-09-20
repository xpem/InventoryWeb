using System.ComponentModel.DataAnnotations;

namespace Models.UIRequests
{
    public class UIUserSignUp
    {
        [MaxLength(150)]
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public required string Name { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid Email")]
        [StringLength(250, MinimumLength = 4)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [MaxLength(350)]
        [MinLength(3)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "O campo Confirmação de Senha é obrigatório")]
        [MaxLength(350)]
        [MinLength(3)]
        [Compare("Password", ErrorMessage = "As senhas não coincidem")]
        public required string ConfirmPassword { get; set; }
    }
}

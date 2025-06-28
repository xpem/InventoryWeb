using System.ComponentModel.DataAnnotations;

namespace Models.UIRequests
{
    public class UIUserSignIn
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid Email")]
        [StringLength(250, MinimumLength = 4)]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [MaxLength(350)]
        public string Password { get; set; }
    }
}

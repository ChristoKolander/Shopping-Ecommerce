using System.ComponentModel.DataAnnotations;


namespace Shopping.Shared.Dtos.Auths
{
    public class UserAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

      
    }
}

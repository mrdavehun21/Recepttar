using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.BLL.DTOs.User
{
    public class LogInUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}

using Recepttar.Server.BLL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.BLL.DTOs.User
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public UserRanksEnum Rank { get; set; }
    }
}

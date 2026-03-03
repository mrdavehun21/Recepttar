using Recepttar.Server.BLL.Enums;

namespace Recepttar.Server.BLL.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public UserRanksEnum Rank { get; set; }
    }
}

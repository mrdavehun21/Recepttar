using Recepttar.Server.Enums;

namespace Recepttar.Server.DTOs.User
{
    public class ProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public UserRanksEnum Rank { get; set; }
    }
}

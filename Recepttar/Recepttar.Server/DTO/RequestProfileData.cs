using Recepttar.Server.Constants;

namespace Recepttar.Server.DTO
{
    public class RequestProfileData
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; } = ProfilePicturePath.Path;
        public Enums.UserRanksEnum Rank { get; set; } = Enums.UserRanksEnum.Hobbi_szakács;
    }
}

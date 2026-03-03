namespace Recepttar.Server.BLL.Constants
{
    public class ProfilePicturePath
    {
        public static string GetPath(int userId)
        {
            return $"api/user/profile/profilepicture/{userId}";
        }
    }
}

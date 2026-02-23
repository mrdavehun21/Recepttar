namespace Recepttar.Server.Constants
{
    public class ProfilePicturePath
    {
        public static string GetPath(int userId)
        {
            return $"api/user/profile/profilepicture/{userId}";
        }
    }
}

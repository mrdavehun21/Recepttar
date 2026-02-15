namespace Recepttar.Server.Constants
{
    public class ProfilePicturePath
    {
        public static string GetPath(int userId)
        {
            return $"profile/profilepicture/{userId}";
        }
    }
}

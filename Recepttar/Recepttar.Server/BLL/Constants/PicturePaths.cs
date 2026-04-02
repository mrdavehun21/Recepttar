namespace Recepttar.Server.BLL.Constants
{
    public static class PicturePaths
    {
        public static class DishPicturePath
        {
            public static string GetPath(int recipeId)
            {
                return $"api/recipe/{recipeId}/image";
            }
        }
        public static class ProfilePicturePath
        {
            public static string GetPath(int userId)
            {
                return $"api/user/profile/profilepicture/{userId}";
            }
        }
    }
}

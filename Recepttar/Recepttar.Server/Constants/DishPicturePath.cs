namespace Recepttar.Server.Constants
{
    public class DishPicturePath
    {
        public static string GetPath(int recipeId)
        {
            return $"recipes/{recipeId}/image";
        }
    }
}

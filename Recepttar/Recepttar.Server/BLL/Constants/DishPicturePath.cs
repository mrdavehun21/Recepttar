namespace Recepttar.Server.BLL.Constants
{
    public class DishPicturePath
    {
        public static string GetPath(int recipeId)
        {
            return $"api/recipe/{recipeId}/image";
        }
    }
}

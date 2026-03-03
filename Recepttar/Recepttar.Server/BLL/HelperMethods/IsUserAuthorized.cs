using Recepttar.Server.BLL.Constants;

namespace Recepttar.Server.BLL.HelperMethods
{
    public class IsUserAuthorized
    {
        public static bool IsAuthorized(HttpContext context)
        {
            int? UserId = context.Session.GetInt32(SessionKeys.UserId);
            return UserId != null;
        }
    }
}

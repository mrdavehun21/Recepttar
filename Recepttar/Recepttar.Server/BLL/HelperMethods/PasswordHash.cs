using System.Security.Cryptography;
using System.Text;

namespace Recepttar.Server.BLL.HelperMethods
{
    public class PasswordHash
    {
        public static string PasswordHasher(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}

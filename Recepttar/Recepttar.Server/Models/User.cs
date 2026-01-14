using Recepttar.Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Bio { get; set; }
        public byte[] ProfilePicture { get; set; }
        public UserRanksEnum Rank { get; set; }
    }
}
using Recepttar.Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(128)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Bio { get; set; }

        public byte[]? ProfilePicture { get; set; }

        public UserRanksEnum Rank { get; set; }

        // Navigation
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<Poll> Polls { get; set; } = new List<Poll>();
    }
}
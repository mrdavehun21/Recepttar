using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.Models
{
    public class Poll
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        [Required, MaxLength(255)]
        public string Question { get; set; } = string.Empty;

        // Navigation
        public User Author { get; set; } = null!;
        public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.DAL.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(Option))]
        public int OptionId { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public PollOption Option { get; set; } = null!;
    }
}

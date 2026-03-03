using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.DAL.Models
{
    public class PollOption
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Poll))]
        public int PollId { get; set; }

        [Required, MaxLength(255)]
        public string OptionText { get; set; } = string.Empty;

        // Navigation
        public Poll Poll { get; set; } = null!;
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}

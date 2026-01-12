using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PollId { get; set; }
        public Poll Poll { get; set; }

        public int OptionId { get; set; }
        public PollOption Option { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class PollOption
    {
        [Key]
        public int Id { get; set; }

        public int PollId { get; set; }
        public Poll Poll { get; set; }

        public string OptionText { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class Poll
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
    }
}
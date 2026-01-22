using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class Poll
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
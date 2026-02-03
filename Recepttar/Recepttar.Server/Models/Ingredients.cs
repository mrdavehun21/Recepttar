using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class Ingredients
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

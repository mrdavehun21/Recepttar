using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

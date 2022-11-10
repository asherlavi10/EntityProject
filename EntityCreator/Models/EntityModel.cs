using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class EntityModel
    {
        [Required]

        public string Name { get; set; }
        [Required]
        
        public int X { get; set; }
        [Required]
        public int Y { get; set; }

        public string ReurnMessage { get; set; }
    }
}

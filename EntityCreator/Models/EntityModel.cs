using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class EntityModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Name length can't be more than 10.")]
        public string Name { get; set; }
        
        [Required]
        [Range(0, 999.99)]
        public float X { get; set; }
        [Required]
        [Range(0, 999.99)]
        public float Y { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace EntityDataContract
{
    public class EntityDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 999.99)]
        
        public float X { get; set; }
        [Required]
        [Range(0, 999.99)]
        public float Y { get; set; }

        [Required]
        public string AppKey { get; set; }
    }
}
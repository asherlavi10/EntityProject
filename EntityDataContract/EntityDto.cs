using System.ComponentModel.DataAnnotations;

namespace EntityDataContract
{
    public class EntityDto
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
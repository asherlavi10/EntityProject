namespace EntityPresentorProj.Models
{
    public class SignalROptions
    {
        public const string Name = "SignalR";
        public string HubUrl { get; set; }
        
    }
    public class ImageDrawOptions
    {
        public const string Name = "ImageDraw";
        public float Width { get; set; }    
        public float Height { get; set; }
        public float FontSize { get; set; }
        public string FontFamily { get; set; }
        public Pen Pen { get; set; }
    }
    public class Pen
    {
        public float Width { get; set; }
    }
}

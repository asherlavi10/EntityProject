using EntityDataContract;
using EntityPresentorProj.Models;
using Microsoft.Extensions.Options;
using System.Drawing;
using System.Drawing.Imaging;

namespace EntityPresentorProj.Services
{
    public class DrawPpointService : IDrawPpointService
    {
        ImageDrawOptions _imageDrawOptions;

        public DrawPpointService(IOptions<ImageDrawOptions> imageDrawOptions)
        {
            _imageDrawOptions = imageDrawOptions.Value;
        }

        public EntityDto DrawEntity(EntityDto entityDto, string imgPath, string newPath)
        {
            Bitmap originalBmp = (Bitmap)Image.FromFile(imgPath);

            // Create a blank bitmap with the same dimensions
            Bitmap tempBitmap = new Bitmap(originalBmp.Width, originalBmp.Height);

            // From this bitmap, the graphics can be obtained, because it has the right PixelFormat
            using (Graphics g = Graphics.FromImage(tempBitmap))
            {
                // Draw the original bitmap onto the graphics of the new bitmap
                g.DrawImage(originalBmp, 0, 0);
                // Use g to do whatever you like
                //Pen pen = new Pen(Brushes.Red, 15);
                System.Drawing.Pen pen = new System.Drawing.Pen(Brushes.Red, _imageDrawOptions.Pen.Width);
                //Rectangle rect = new Rectangle((int)entityDto.X, (int)entityDto.X, (int)entityDto.Y, (int)entityDto.Y);

                g.DrawEllipse(pen, entityDto.X,entityDto.Y, _imageDrawOptions.Width, _imageDrawOptions.Height);
                

                // Create string to draw.
                String drawString = entityDto.Name;

                // Create font and brush.
                Font drawFont = new Font(_imageDrawOptions.FontFamily, _imageDrawOptions.FontSize);
                SolidBrush drawBrush = new SolidBrush(Color.Red);

                // Create point for upper-left corner of drawing.
                float x = entityDto.X;
                float y = entityDto.Y;

                // Set format of string.
                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                // Draw string to screen.
                g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);



            }
            tempBitmap.Save(newPath, System.Drawing.Imaging.ImageFormat.Gif);
            return entityDto;
        }
    }
}

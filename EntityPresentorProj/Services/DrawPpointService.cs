using EntityDataContract;
using System.Drawing;
using System.Drawing.Imaging;

namespace EntityPresentorProj.Services
{
    public class DrawPpointService : IDrawPpointService
    {
      
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
                Pen pen = new Pen(Brushes.Red, 15);
                Rectangle rect = new Rectangle(entityDto.X, entityDto.X,100, 100);
                g.DrawEllipse(pen, rect);
                

                // Create string to draw.
                String drawString = entityDto.Name;

                // Create font and brush.
                Font drawFont = new Font("Arial", 20);
                SolidBrush drawBrush = new SolidBrush(Color.Red);

                // Create point for upper-left corner of drawing.
                float x = 150.0F;
                float y = 50.0F;

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

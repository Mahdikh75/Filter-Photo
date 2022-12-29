using System.Drawing;

namespace Filter_Photo
{
    public class Photo
    {
        // Basic changes ==> resize - screenshot - crop image - write text on images
        public Image Resize(Image image, Size size)
        {
            return new Bitmap(image, size);
        }

        public Image Crop(Image image, int x, int y, int W, int H)
        {
            return new Bitmap(image).Clone(new Rectangle(x, y, W, H), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public Image SetTextImage(string text, Font font, Color color, Image image, Point point)
        {
            Bitmap bitmap = new Bitmap(image);
            Graphics graphics = Graphics.FromImage(bitmap);
            SolidBrush solidBrush = new SolidBrush(color);
            graphics.DrawString(text, font, solidBrush, point);
            return bitmap;
        }

        public Image ScreenShot(int W, int H)
        {
            Bitmap bitmap = new Bitmap(W, H);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(Point.Empty, Point.Empty, new Size(W, H));
            return bitmap;
        }

        public Image RemoveBackgroundImage(Image Image, Color Old_Background, Color New_Background)
        {
            Bitmap bitmap = new Bitmap(Image);
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var old_color = bitmap.GetPixel(i, j);
                    if (old_color == Old_Background)
                        bitmap.SetPixel(i, j, New_Background);
                }
            }
            return bitmap;
        }
    }
}
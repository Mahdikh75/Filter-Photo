using System;
using System.Drawing;

namespace Filter_Photo
{
    public class Filter
    {
        // ints : Color Filter
        private int PxR, PxG, PxB;
        private Image image;

        public Filter(Image image)
        {
            this.image = image;
        }
        // خاکستری کردن تصویر
        public Image GrayScale()
        {
            Bitmap bitmap = new Bitmap(image);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color Avg_Color = bitmap.GetPixel(i, j);
                    int PX_Color_New = (int)((Avg_Color.R + Avg_Color.B + Avg_Color.G) / 3);
                    bitmap.SetPixel(i, j, Color.FromArgb(PX_Color_New, PX_Color_New, PX_Color_New));
                }
            }
            return bitmap;
        }
        // نگاتیو کردن تصویر - وارونه سازی
        public Image Invert()
        {
            Bitmap bitmap = new Bitmap(image);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color Old = bitmap.GetPixel(i, j);
                    Color New = Color.FromArgb(255 - Old.R, 255 - Old.G, 255 - Old.B);
                    bitmap.SetPixel(i, j, New);
                }
            }
            return bitmap;
        }

        public enum TypeColor
        {
            red, green, blue
        }

        // int alpha [0 to 255]
        public Image Alpha(int alpha)
        {
            Bitmap bitmap = new Bitmap(image);
            if (alpha >= 1 && alpha <= 255)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        Color color = bitmap.GetPixel(i, j);
                        bitmap.SetPixel(i, j, Color.FromArgb(alpha, color.R, color.G, color.B));
                    }
                }
                return bitmap;
            }
            return null;
        }

        public Image ClearColor(int R, int G, int B, Color new_set)
        {
            try
            {
                Bitmap bitmap = new Bitmap(image);
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        var color_old = bitmap.GetPixel(i, j);
                        Color color = color_old;
                        if (color_old.R == R && color_old.G == G && color_old.B == B)
                        {
                            color = new_set;
                        }
                        bitmap.SetPixel(i, j, color);
                    }
                }
                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ColorFilter [0 to 255] (value color)
        public Image ColorFilter(TypeColor typeColor, int value_color)
        {
            Bitmap bitmap = new Bitmap(image);
            if (value_color >= 0 && value_color <= 255)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        Color color = bitmap.GetPixel(i, j);
                        switch (typeColor)
                        {
                            case TypeColor.red:
                                PxR = color.R;
                                PxG = color.G - value_color;
                                PxB = color.B - value_color;
                                break;
                            case TypeColor.green:
                                PxR = color.R - value_color;
                                PxG = color.G;
                                PxB = color.B - value_color;
                                break;
                            case TypeColor.blue:
                                PxR = color.R - value_color;
                                PxG = color.G - value_color;
                                PxB = color.B;
                                break;
                            default:
                                break;
                        }
                        PxR = Math.Max(PxR, 0);
                        PxR = Math.Min(PxR, 255);

                        PxG = Math.Max(PxG, 0);
                        PxG = Math.Min(PxG, 255);

                        PxB = Math.Max(PxB, 0);
                        PxB = Math.Min(PxB, 255);

                        bitmap.SetPixel(i, j, Color.FromArgb(PxR, PxG, PxB));
                    }
                }
            }
            return bitmap;
        }

        // Gamma [0.2 to 5] (colors)
        private byte[] GM_Array(double dfcolor)
        {
            byte[] gm = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                gm[i] = (byte)Math.Min(255, (int)(255.0 * Math.Pow(i / 255.0, 1.0 / dfcolor) + 0.5));
            }
            return gm;
        }

        public Image Gamma(double red, double green, double blue)
        {
            Bitmap bitmap = new Bitmap(image);

            byte[] red_gm = GM_Array(red);
            byte[] green_gm = GM_Array(green);
            byte[] blue_gm = GM_Array(blue);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color old = bitmap.GetPixel(i, j);
                    bitmap.SetPixel(i, j, Color.FromArgb(red_gm[old.R], green_gm[old.G], blue_gm[old.B]));
                }
            }
            return bitmap;
        }

        // Brightness [-255 to +255] (value)
        public Image Brightness(int value)
        {
            Bitmap bitmap = new Bitmap(image);

            if (value > 255)
                value = 255;

            if (value < -255)
                value = -255;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    int R = color.R + value;
                    int G = color.G + value;
                    int B = color.B + value;

                    R = R > 255 ? 255 : R;
                    R = R < 0 ? 1 : R;

                    G = G > 255 ? 255 : G;
                    G = G < 0 ? 1 : G;

                    B = B > 255 ? 255 : B;
                    B = B < 0 ? 1 : B;

                    bitmap.SetPixel(i, j, Color.FromArgb(R, G, B));
                }
            }
            return bitmap;
        }

        // Contrast [-100 to 100] (Value)
        public Image Contrast(double value)
        {
            Bitmap bitmap = new Bitmap(image);
            if (value > 100)
                value = 100;
            if (value < -100)
                value = -100;

            value = (100.0 + value) / 100.0;
            value *= value;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);

                    double R = color.R / 255.0;
                    R -= 0.5;
                    R *= value;
                    R += 0.5;
                    R *= 255;
                    R = R > 255 ? 255 : R;
                    R = R < 0 ? 0 : R;

                    double G = color.G / 255.0;
                    G -= 0.5;
                    G *= value;
                    G += 0.5;
                    G *= 255;
                    G = G > 255 ? 255 : G;
                    G = G < 0 ? 0 : G;

                    double B = color.B / 255.0;
                    B -= 0.5;
                    B *= value;
                    B += 0.5;
                    B *= 255;
                    B = B > 255 ? 255 : B;
                    B = B < 0 ? 0 : B;

                    bitmap.SetPixel(i, j, Color.FromArgb((int)R, (int)G, (int)B));
                }
            }

            return bitmap;
        }

    }
}

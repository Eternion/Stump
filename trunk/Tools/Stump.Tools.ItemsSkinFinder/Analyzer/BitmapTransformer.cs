using System.Drawing;

namespace Stump.Tools.ItemsSkinFinder.Analyzer
{
    public static class BitmapTransformer
    {
        public static void Crop(ref Bitmap bitmap)
        {
            Color oldPixel = Color.Empty;

            Point p1;
            Point p2;
            Point p3;
            Point p4;

            for (int j = 0; j < bitmap.Height; j++)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    if (oldPixel == Color.Empty)
                    {
                        oldPixel = bitmap.GetPixel(i, j);
                    }
                    else
                    {
                        if (oldPixel != bitmap.GetPixel(i, j))
                        {
                            p1 = new Point(i, j);
                            goto Break1;
                        }
                    }

                }
            }

            p1 = new Point((bitmap.Width/2) - 1, 0);

            Break1:

            oldPixel = Color.Empty;

            for (int i = (bitmap.Width - 1); i >= 0; i--)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (oldPixel == Color.Empty)
                    {
                        oldPixel = bitmap.GetPixel(i, j);
                    }
                    else
                    {
                        if (oldPixel != bitmap.GetPixel(i, j))
                        {
                            p2 = new Point(i, j);
                            goto Break2;
                        }
                    }

                }
            }

            p2 = new Point(bitmap.Width - 1, (bitmap.Height/2) - 1);

            Break2:

            oldPixel = Color.Empty;

            for (int j = (bitmap.Height - 1); j >= 0; j--)
            {
                for (int i = (bitmap.Width - 1); i >= 0; i--)
                {

                    if (oldPixel == Color.Empty)
                    {
                        oldPixel = bitmap.GetPixel(i, j);
                    }
                    else
                    {
                        if (oldPixel != bitmap.GetPixel(i, j))
                        {
                            p3 = new Point(i, j);
                            goto Break3;
                        }
                    }

                }
            }

            p3 = new Point((bitmap.Width/2) - 1, bitmap.Height - 1);

            Break3:

            oldPixel = Color.Empty;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = (bitmap.Height - 1); j >= 0; j--)
                {
                    if (oldPixel == Color.Empty)
                    {
                        oldPixel = bitmap.GetPixel(i, j);
                    }
                    else
                    {
                        if (oldPixel != bitmap.GetPixel(i, j))
                        {
                            p4 = new Point(i, j);
                            goto Break4;
                        }
                    }

                }
            }

            p4 = new Point(0, (bitmap.Height/2) - 1);

            Break4:

            int x = p4.X;
            int y = p1.Y;
            int width = bitmap.Width - ((bitmap.Width - p2.X) + p4.X);
            int height = bitmap.Height - (p1.Y + (bitmap.Height - p3.Y));

            bitmap = bitmap.Clone(new Rectangle(x, y, width + 1, height + 1), bitmap.PixelFormat);
        }

        public static void Resize(ref Bitmap bitmap, int height, int width)
        {
            bitmap = new Bitmap(bitmap, height, width);
        }
    }
}

using System;
using System.Drawing;
using Stump.Core.Attributes;

namespace Stump.Tools.ItemsSkinFinder.Analyzer
{
    public static class BitmapComparer
    {
        [Variable(true)]
        public static int ErrorMargin = 30;

        public static sbyte Compare(this Bitmap bitmap, Bitmap specimen)
        {
            uint total = 0;
            uint success = 0;

            for (int j = 0; j < bitmap.Height; j++)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    var pixel1 = bitmap.GetPixel(i, j);
                    var pixel2 = specimen.GetPixel(i, j);

                    int redMin = pixel1.R - (((pixel1.R * ErrorMargin) / 100));
                    int redMax = pixel1.R + (((pixel1.R * ErrorMargin) / 100));
                    if (redMin <= pixel2.R && redMax >= pixel2.R)
                    {
                        int blueMin = pixel1.B - (((pixel1.B * ErrorMargin) / 100));
                        int blueMax = pixel1.B + (((pixel1.B * ErrorMargin) / 100));
                        if (blueMin <= pixel2.B && blueMax >= pixel2.B)
                        {
                            int greenMin = pixel1.G - (((pixel1.G * ErrorMargin) / 100));
                            int greenMax = pixel1.G + (((pixel1.G * ErrorMargin) / 100));
                            if (greenMin <= pixel2.G && greenMax >= pixel2.G)
                            {

                                /*
                                 * i don't think we need to manage alpha
                                 * 
                                int alphaMin = pixel1.A - (((pixel1.A * ErrorMargin) / 100));
                                int alphaMax = pixel1.A + (((pixel1.A * ErrorMargin) / 100));
                                if (alphaMin <= pixel2.A && alphaMax >= pixel2.A)
                                {

                                }
                                */

                                success++;
                            }
                        }
                    }
                    total++;
                }
            }

            return (sbyte)((success * 100) / total); // does not exceed 100
        }
    }
}

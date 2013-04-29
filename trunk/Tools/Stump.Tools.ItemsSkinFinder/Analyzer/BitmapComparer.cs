using System;
using System.Drawing;

namespace Stump.Tools.ItemsSkinFinder.Analyzer
{
    public class BitmapComparer
    {
        public sbyte ErrorMargin
        {
            get; 
            private set;
        }

        public Bitmap Bitmap1
        {
            get; 
            private set;
        }

        public Bitmap Bitmap2
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor of BitmapComparer.
        /// </summary>
        /// <param name="errorMargin">error margin, value must be between 0 and 100 included </param>
        /// <param name="bitmap1">bitmap 1</param>
        /// <param name="bitmap2">bitmap 2</param>
        public BitmapComparer(sbyte errorMargin, Bitmap bitmap1, Bitmap bitmap2)
        {
            if (errorMargin >= 0 || errorMargin <= 100)
                throw new ArgumentException("", "errorMargin");

            ErrorMargin = errorMargin;

            if (bitmap1 == null)
                throw new ArgumentNullException("bitmap1");

            if (bitmap2 == null)
                throw new ArgumentNullException("bitmap2");

            //TODO Crop&Resize bitmap1&bitmap2

        }

        /// <summary>
        /// Compare two bitmaps and references percentage of validity.
        /// 
        /// TODO : try to get a faster method ...
        /// </summary>
        /// <returns>percentage</returns>
        public sbyte Compare()
        {
            uint total = 0;
            uint success = 0;

            for (int j = 0; j < Bitmap1.Height; j++)
            {
                for (int i = 0; i < Bitmap1.Width; i++)
                {
                    var pixel1 = Bitmap1.GetPixel(i, j);
                    var pixel2 = Bitmap2.GetPixel(i, j);

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

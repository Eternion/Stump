#region License GNU GPL
// BitmapHelper.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Stump.Tools.ItemSkinFinderWPF
{
    public static class BitmapHelper
    {
        public static BitmapSource ResizeBitmap(this BitmapSource source, int nWidth, int nHeight)
        {
            var tbBitmap = new TransformedBitmap(source,
                                                      new ScaleTransform(nWidth / (double)source.PixelWidth,
                                                                         nHeight / (double)source.PixelHeight,
                                                                         0, 0));

            
            return tbBitmap;
        }

        public static BitmapSource CropBitmap(this BitmapSource original)
        {
            int stride = original.PixelWidth * 4;
            int size = original.PixelHeight * stride;
            byte[] pixels = new byte[size];
            original.CopyPixels(pixels, stride, 0);

            var min = new Point(int.MaxValue, int.MaxValue);
            var max = new Point(int.MinValue, int.MinValue);

            for (int x = 0; x < original.PixelWidth; ++x)
            {
                for (int y = 0; y < original.PixelHeight; ++y)
                {
                    int index = y * stride + 4 * x;


                    byte red = pixels[index];
                    byte green = pixels[index + 1];
                    byte blue = pixels[index + 2];
                    byte alpha = pixels[index + 3];

                    if (( red != 255 || green != 255 || blue != 255 )
                        && alpha > 0)
                    {
                        if (x < min.X) min.X = x;
                        if (y < min.Y) min.Y = y;

                        if (x > max.X) max.X = x;
                        if (y > max.Y) max.Y = y;
                    }
                }
            }

            var bmp = new CroppedBitmap(original, new Int32Rect((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y))); ;
            return bmp;
        }

        public unsafe static BitmapSource ReplaceTransparentToWhite(this BitmapSource bitmap)
        {
            var writeable = new WriteableBitmap(bitmap);

            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);

            var pBackBuffer = (int)writeable.BackBuffer;
            unsafe
            {
                for (int x = 0; x < bitmap.PixelWidth; ++x)
                {
                    for (int y = 0; y < bitmap.PixelHeight; ++y)
                    {
                        int index = y * stride + 4 * x;

                       
                        byte red =  *(byte*)(pBackBuffer + index);
                        byte green = *(byte*)(pBackBuffer + index + 1);
                        byte blue = *(byte*)(pBackBuffer + index + 2);
                        byte alpha = *(byte*)(pBackBuffer + index + 3);

                        if (alpha == 0)
                        {
                            *(byte*)( pBackBuffer + index ) = 0;
                            *(byte*)( pBackBuffer + index + 1 ) = 0;
                            *(byte*)( pBackBuffer + index + 2 ) = 0;
                            *(byte*)( pBackBuffer + index + 3 ) = byte.MaxValue;
                        }
                    }
                }
            }
           
            return writeable;
        }

        public static double Compare(this BitmapSource bitmap, BitmapSource specimen, double margin)
        {
            uint total = 0;
            uint success = 0;

            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels1 = new byte[size];
            bitmap.CopyPixels(pixels1, stride, 0);

            stride = specimen.PixelWidth * 4;
            size = specimen.PixelHeight * stride;
            byte[] pixels2 = new byte[size];
            specimen.CopyPixels(pixels2, stride, 0);

            for (int j = 0; j < bitmap.Height; j++)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    int index = i * stride + 4 * j;

                    byte red1 = pixels1[index];
                    byte green1 = pixels1[index + 1];
                    byte blue1 = pixels1[index + 2];

                    byte red2 = pixels2[index];
                    byte green2 = pixels2[index + 1];
                    byte blue2 = pixels2[index + 2];

                    var redMin = red1 - (((red1 * margin) / 100));
                    var redMax = red1 + ( ( ( red1 * margin ) / 100 ) );
                    if (redMin <= red2 && redMax >= red2)
                    {
                        var blueMin = blue1 - ( ( ( blue1 * margin ) / 100 ) );
                        var blueMax = blue1 + ( ( ( blue1 * margin ) / 100 ) );
                        if (blueMin <= blue2 && blueMax >= blue2)
                        {
                            var greenMin = green1 - (((green1 * margin) / 100));
                            var greenMax = green1 + (((green1 * margin) / 100));
                            if (greenMin <= green2 && greenMax >= green2)
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

            return ( success * 100d ) / total; // does not exceed 100
        }
    }
}
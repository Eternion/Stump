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
using Stump.Tools.ItemSkinFinderWPF.Colors;

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


        public static BitmapSource CropBitmap(this BitmapSource original, double tolerance)
        {
            int w = original.PixelWidth, h = original.PixelHeight;

            int stride = original.PixelWidth * 4;
            int size = original.PixelHeight * stride;
            byte[] pixels = new byte[size];
            original.CopyPixels(pixels, stride, 0);

            var whiteTolerance = 255 * tolerance;
            var transparentTolerance = 255 - 255 * tolerance;

            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
                {
                    int index = row * stride + 4 * i;
                    byte blue = pixels[index];
                    byte green = pixels[index + 1];
                    byte red = pixels[index + 2];
                    byte alpha = pixels[index + 3];

                    if (( (red + blue + green / 3) < whiteTolerance) && alpha > transparentTolerance)
                        return false;
                }
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                {
                    int index = i * stride + 4 * col;
                    byte blue = pixels[index];
                    byte green = pixels[index + 1];
                    byte red = pixels[index + 2];
                    byte alpha = pixels[index + 3];

                    if (( ( red + blue + green / 3 ) < whiteTolerance ) && alpha > transparentTolerance)
                        return false;
                }
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = h - 1;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }

            int leftmost = 0, rightmost = w - 1;
            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedHeight < 0 || croppedWidth < 0)
                return original;

            var bmp = new CroppedBitmap(original, new Int32Rect(leftmost, topmost, croppedWidth, croppedHeight));
            return bmp;
        }

        public unsafe static BitmapSource ReplaceWhiteToTransparent(this BitmapSource bitmap)
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

                        byte blue = *(byte*)( pBackBuffer + index  );
                        byte green = *(byte*)( pBackBuffer + index + 1 );
                        byte red = *(byte*)( pBackBuffer + index + 2 );
                        byte alpha = *(byte*)(pBackBuffer + index + 3);

                        if (red == 255 && green == 255 && blue == 255)
                        {
                            *(byte*)( pBackBuffer + index + 3 ) = 0;
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

            int stride1 = bitmap.PixelWidth * 4;
            int size1 = bitmap.PixelHeight * stride1;
            byte[] pixels1 = new byte[size1];
            bitmap.CopyPixels(pixels1, stride1, 0);

            int stride2 = specimen.PixelWidth * 4;
            int size2 = specimen.PixelHeight * stride2;
            byte[] pixels2 = new byte[size2];
            specimen.CopyPixels(pixels2, stride2, 0);

            for (int j = 0; j < bitmap.Height; j++)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    int index1 = j * stride1 + 4 * i;
                    int index2 = j * stride2 + 4 * i;

                    byte blue1 = pixels1[index1];
                    byte green1 = pixels1[index1 + 1];
                    byte red1 = pixels1[index1 + 2];
                    byte alpha1 = pixels1[index1 + 3];

                    byte blue2 = pixels2[index2];
                    byte green2 = pixels2[index2 + 1];
                    byte red2 = pixels2[index2 + 2];
                    byte alpha2 = pixels2[index2 + 3];

                    if (alpha1 == 0 || alpha2 == 0)
                        continue;

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

            return (double)success / total * 100d;
        }

        public static Color GetPixelColor(this BitmapSource source, int x, int y)
        {
            Color c = System.Windows.Media.Colors.White;
            if (source != null)
            {
                try
                {
                    CroppedBitmap cb = new CroppedBitmap(source, new Int32Rect(x, y, 1, 1));
                    var pixels = new byte[4];
                    cb.CopyPixels(pixels, 4, 0);
                    c = Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
                }
                catch (Exception)
                {
                }
            }
            return c;
        }

        public static RGB GetAverageColor(this BitmapSource bitmap)
        {
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);

            int sumRed = 0;
            int sumGreen = 0;
            int sumBlue = 0;
            int pixelCount = 0;

            for (int x = 0; x < bitmap.PixelWidth; ++x)
            {
                for (int y = 0; y < bitmap.PixelHeight; ++y)
                {
                    int index = y * stride + 4 * x;

                    if (pixels[index + 3] == 0)
                        continue;

                    sumBlue += pixels[index];
                    sumGreen += pixels[index + 1];
                    sumRed += pixels[index + 2];
                    pixelCount++;
                }
            }

            return new RGB(sumRed / pixelCount, sumGreen / pixelCount, sumBlue / pixelCount);
        }

        public static ulong AverageHash(this BitmapSource src)
        {
            // Squeeze the image down to an 8x8 image.

            // Chant the ancient incantations to create the correct data structures.
            var squeezedImage = ResizeBitmap(src, 8, 8);

            byte[] grayScaleImage = new byte[64];

            uint averageValue = 0;
            ulong finalHash = 0;

            int stride = squeezedImage.PixelWidth * 4;
            int size = squeezedImage.PixelHeight * stride;
            byte[] pixels = new byte[size];
            squeezedImage.CopyPixels(pixels, stride, 0);

            // Reduce to 8-bit grayscale and alculate the average pixel value.
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int index = y * stride + 4 * x;

                    byte red = pixels[index];
                    byte green = pixels[index + 1];
                    byte blue = pixels[index + 2];

                    uint grayTone = ( (uint)( ( red * 0.3 ) + ( green * 0.59 ) + ( blue * 0.11 ) ) );

                    grayScaleImage[x + y * 8] = (byte)grayTone;
                    averageValue += grayTone;
                }
            }
            averageValue /= 64;

            // Return 1-bits when the tone is equal to or above the average,
            // and 0-bits when it's below the average.
            for (int k = 0; k < 64; k++)
            {
                //if(k % 8 == 0)
                //	Console.WriteLine();

                if (grayScaleImage[k] >= averageValue)
                {
                    finalHash |= ( 1UL << ( 63 - k ) );
                    //	Console.Write(" ");
                }
                //else
                //	Console.Write("#");
            }

            return finalHash;
        }

    }
}
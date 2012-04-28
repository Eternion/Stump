using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Stump.Tools.Toolkit.Helpers
{
    public static class ImageHelper
    {
        public static BitmapSource GetBitmapSource(System.Drawing.Bitmap bitmap)
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            return bitmapSource;
        }
    }
}
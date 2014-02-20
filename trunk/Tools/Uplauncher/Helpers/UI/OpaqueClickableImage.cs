#region License GNU GPL
// OpaqueClickableImage.cs
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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Uplauncher.Helpers.UI
{
    public class OpaqueClickableImage : Image
    {
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            var source = (BitmapSource)Source;
            var x = (int)( hitTestParameters.HitPoint.X / ActualWidth * source.PixelWidth );
            var y = (int)( hitTestParameters.HitPoint.Y / ActualHeight * source.PixelHeight );
            if (x == source.PixelWidth)
                x--;
            if (y == source.PixelHeight)
                y--;
            var pixels = new byte[4];
            source.CopyPixels(new Int32Rect(x, y, 1, 1), pixels, 4, 0);
            return pixels[3] < 1 ? null : new PointHitTestResult(this, hitTestParameters.HitPoint);
        }
    }
}
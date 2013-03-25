#region License GNU GPL
// CanvasAutoSize.cs
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WorldEditor.Helpers
{
    public class CanvasAutoSize : Canvas
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (InternalChildren.Count == 0)
                return new Size(0, 0);

            double minX = Math.Min(0,
                InternalChildren
                .OfType<UIElement>()
                .Min(i => (double)i.GetValue(Canvas.LeftProperty)));

            double minY = Math.Min(0,
                InternalChildren
                .OfType<UIElement>()
                .Min(i => (double)i.GetValue(Canvas.TopProperty)));

            foreach (UIElement child in InternalChildren)
            {
                double left = Canvas.GetLeft(child);
                double top = Canvas.GetTop(child);
                child.Arrange(new Rect(new Point(left - minX, top - minY), child.DesiredSize));
            }
            return arrangeSize;
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            base.MeasureOverride(constraint);

            var size = new Size(0, 0);
            var minPoint = new Point(0, 0);
            foreach (var child in InternalChildren.OfType<UIElement>())
            {
                var left = (double)child.GetValue(LeftProperty);
                var top = (double)child.GetValue(TopProperty);

                if (left < 0 && left < minPoint.X)
                    minPoint.X = left;
                
                if (top < 0 && top < minPoint.Y)
                    minPoint.Y = top;

                if (child.DesiredSize.Width + left > size.Width)
                    size.Width = child.DesiredSize.Width + left;

                if (child.DesiredSize.Height + top > size.Height)
                    size.Height = child.DesiredSize.Height + top;
            }

            size.Width += -minPoint.X;
            size.Height += -minPoint.Y;

            return size;
        }
    }
}
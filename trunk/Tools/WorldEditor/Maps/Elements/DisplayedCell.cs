#region License GNU GPL
// DisplayedCell.cs
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorldEditor.Maps.Layers;

namespace WorldEditor.Maps.Elements
{
    public sealed class DisplayedCell : DisplayedElement
    {
        public DisplayedCell(Layer layer, Brush color, double posX, double posY)
        {
            Layer = layer;
            PosX = posX;
            PosY = posY;
            CellBrush = color;
        }

        private Brush m_cellBrush;

        public Brush CellBrush
        {
            get { return m_cellBrush; }
            set
            {
                m_cellBrush = value;
                UpdateCell();
            }
        }

        public override BitmapSource ImageSource
        {
            get;
            protected set;
        }

        public override Layer Layer
        {
            get;
            protected set;
        }

        public override double PosX
        {
            get;
            set;
        }

        public override double PosY
        {
            get;
            set;
        }

        public void UpdateCell()
        {
            var points = new PointCollection
                    {
                        new Point(MapEditorModelView.CellHalfWidth, 0),
                        new Point(MapEditorModelView.CellWidth, MapEditorModelView.CellHalfHeight),
                        new Point(MapEditorModelView.CellHalfWidth, MapEditorModelView.CellHeight),
                        new Point(0, MapEditorModelView.CellHalfHeight)
                    };

            var polygon = new Polygon
            {
                Points = points,
                Stroke = Brushes.Gray,
                StrokeThickness = 4,
                Fill = m_cellBrush,
            };

            polygon.Measure(new System.Windows.Size((int)MapEditorModelView.CellWidth, (int)MapEditorModelView.CellHeight));
            polygon.Arrange(new Rect(new System.Windows.Size((int)MapEditorModelView.CellWidth, (int)MapEditorModelView.CellHeight)));

            var render = new RenderTargetBitmap((int)MapEditorModelView.CellWidth,
                                                 (int)MapEditorModelView.CellHeight, 96, 96, PixelFormats.Pbgra32);
            render.Render(polygon);
            ImageSource = render;

        }
    }
}
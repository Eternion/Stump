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

using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;
using Stump.DofusProtocol.D2oClasses.Tools.Ele.Datas;
using Stump.Server.WorldServer.Game.Maps.Cells;
using WorldEditor.Maps.Layers;

namespace WorldEditor.Maps.Elements
{
    public sealed class GfxElement : DisplayedElement
    {
        public GfxElement(DlmGraphicalElement element, NormalGraphicalElementData graphicalElement, BitmapSource gfx, ObjectsLayer layer)
        {
            Cell = element.Cell.Id;
            ColorMultiplicator = element.ColorMultiplicator;
            Altitude = element.Altitude;
            m_elementId = element.ElementId;
            Identifier = element.Identifier;
            Offset = new Point(element.Offset.X, element.Offset.Y);
            PixelOffset = new Point(element.PixelOffset.X, element.PixelOffset.Y);
            FinalTeint = element.FinalTeint;
            Hue = element.Hue;
            Shadow = element.Shadow;
            Layer = layer;
            Template = new GfxTemplate(graphicalElement);
            ImageSource = gfx;
        }

        public short Cell
        {
            get;
            set;
        }

        public Point CellPixel
        {
            get
            {
                return MapModelView.GetCellPixelCoords(Cell);
            }
        }

        public ColorMultiplicator ColorMultiplicator
        {
            get;
            set;
        }

        public int Altitude
        {
            get;
            set;
        }

        private uint m_elementId;
        private double? m_posX;
        private double? m_posY;

        public uint ElementId
        {
            get { return m_elementId; }
            set
            {
                var template = WorldGFXManager.GetElement((int)value) as NormalGraphicalElementData;

                if (template == null)
                    throw new Exception(string.Format("NormalGraphicalElementData with id {0} not found", value));

                Template = new GfxTemplate(template);
                m_elementId = value;
            }
        }

        public ColorMultiplicator FinalTeint
        {
            get;
            set;
        }

        public ColorMultiplicator Hue
        {
            get;
            set;
        }

        public uint Identifier
        {
            get;
            set;
        }

        public Point Offset
        {
            get;
            set;
        }

        public Point PixelOffset
        {
            get;
            set;
        }

        public GfxTemplate Template
        {
            get;
            set;
        }

        public ColorMultiplicator Shadow
        {
            get;
            set;
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
            get
            {
                return m_posX ?? CellPixel.X + PixelOffset.X + Offset.X - Template.Origin.X;
            }
            set { m_posX = value; }
        }

        public override double PosY
        {
            get
            {
                return m_posY ?? CellPixel.Y + PixelOffset.Y + Offset.Y - Template.Origin.Y - 
                    (Altitude != 0 ? MapEditorModelView.AltitudePixelUnit * Altitude : 0);
            }
            set { m_posY = value; }
        }

        public void UpdateElementPosition()
        {
            if (m_posX == null && m_posY == null)
                return;

            var id = MapModelView.GetCellByPixel(new Point(PosX, PosY));
            Cell = id;

            m_posX = null;
            m_posY = null;
        }
    }
}
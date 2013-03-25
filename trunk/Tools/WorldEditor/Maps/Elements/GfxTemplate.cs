#region License GNU GPL
// GfxTemplate.cs
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

using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using Stump.DofusProtocol.D2oClasses.Tools.Ele.Datas;

namespace WorldEditor.Maps.Elements
{
    public class GfxTemplate : INotifyPropertyChanged
    {
        public GfxTemplate(NormalGraphicalElementData element)
        {
            GfxId = element.Gfx;
            Height = element.Height;
            HorizontalSymmetry = element.HorizontalSymmetry;
            Origin = new Point(element.Origin.X, element.Origin.Y);
            Size = new Point(element.Size.X, element.Size.Y);
        }

        public BitmapSource Gfx
        {
            get;
            private set;
        }

        private int m_gfxId;

        public int GfxId
        {
            get { return m_gfxId; }
            set
            {
                m_gfxId = value;
                Gfx = WorldGFXManager.GetGfx(value);
            }
        }

        public uint Height
        {
            get;
            set;
        }

        public bool HorizontalSymmetry
        {
            get;
            set;
        }

        public Point Origin
        {
            get;
            set;
        }

        public Point Size
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
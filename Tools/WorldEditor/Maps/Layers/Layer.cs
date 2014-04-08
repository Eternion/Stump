#region License GNU GPL
// Layer.cs
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

using WorldEditor.Helpers;
using WorldEditor.Maps.Elements;

namespace WorldEditor.Maps.Layers
{
    public abstract class Layer
    {

        protected Layer()
        {
            Opacity = 1d;
        }

        private ObservableCollectionRange<DisplayedElement> m_elements = new ObservableCollectionRange<DisplayedElement>();

        public ObservableCollectionRange<DisplayedElement> DisplayedElements
        {
            get
            {
                return m_elements;
            }
        }

        public double Opacity
        {
            get;
            set;
        }

        public virtual int Index
        {
            get;
            set;
        }

        public virtual int SubIndex
        {
            get;
            set;
        }

        public abstract string Name
        {
            get;
        }

        public bool IsVisible
        {
            get;
            set;
        }
    }
}
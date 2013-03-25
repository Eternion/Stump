#region License GNU GPL
// ObjectsLayer.cs
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

using System.Collections.Specialized;
using System.Linq;
using WorldEditor.Helpers;
using WorldEditor.Maps.Elements;

namespace WorldEditor.Maps.Layers
{
    public class ObjectsLayer : Layer
    {
        public ObjectsLayer(int id)
        {
            Id = id;
            m_elements = new ObservableCollectionRange<GfxElement>();
            m_elements.CollectionChangedRange += OnCollectionChanged;
        }

        public int Id
        {
            get;
            set;
        }

        public override string Name
        {
            get
            {
                return "Layer " + Id;
            }
        }

        private ObservableCollectionRange<GfxElement> m_elements;

        public ObservableCollectionRange<GfxElement> Elements
        {
            get
            {
                return m_elements;
            }
        }


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                DisplayedElements.AddRange(e.NewItems.OfType<GfxElement>());
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (GfxElement element in e.OldItems)
                {
                    DisplayedElements.Remove(element);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
                DisplayedElements.Clear();
        }
    }
}
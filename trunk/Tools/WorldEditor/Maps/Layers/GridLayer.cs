#region License GNU GPL
// GridLayer.cs
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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;
using Stump.Server.WorldServer.Game.Maps.Cells;
using WorldEditor.Helpers;
using WorldEditor.Maps.Elements;

namespace WorldEditor.Maps.Layers
{
    public class GridLayer : Layer
    {
        private string m_name;
        public GridLayer()
        {
            m_cells = new ObservableCollectionRange<DisplayedCell>();
            m_cells.CollectionChangedRange += OnCollectionChanged;
            Reset();
        }

        public GridLayer(string name)
            : this()
        {
            m_name = name;
        }

        private ObservableCollectionRange<DisplayedCell> m_cells;

        public ObservableCollectionRange<DisplayedCell> Cells
        {
            get
            {
                return m_cells;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                DisplayedElements.AddRange(e.NewItems.OfType<DisplayedCell>());
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (DisplayedCell element in e.OldItems)
                {
                    DisplayedElements.Remove(element);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
                DisplayedElements.Clear();
        }

        public override string Name
        {
            get
            {
                return m_name ?? "Grid";
            }
        }

        public void Reset()
        {
            m_cells.Clear();

            var list = new List<DisplayedCell>();

            for (int i = 0; i < MapEditorModelView.MapCellsCount; i++)
            {
                var pixelCoords = MapModelView.GetCellPixelCoords((short) i);
                list.Add(new DisplayedCell(this, Brushes.Transparent, pixelCoords.X, pixelCoords.Y));
            }

            m_cells.AddRange(list);
        }
    }
}
#region License GNU GPL
// MapModelView.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;
using Stump.DofusProtocol.D2oClasses.Tools.Ele.Datas;
using Stump.Server.WorldServer.Game.Maps.Cells;
using WorldEditor.Helpers;
using WorldEditor.Helpers.Collections;
using WorldEditor.Maps.Elements;
using WorldEditor.Maps.Layers;

namespace WorldEditor.Maps
{
    public class MapModelView : INotifyPropertyChanged
    {
        private readonly MapControl m_view;
        private readonly DlmReader m_dlmFile;
        private ObservableCollectionRange<DisplayedElement> m_elements = new ObservableCollectionRange<DisplayedElement>();
        private ObservableCollectionRange<Layer> m_layers = new ObservableCollectionRange<Layer>();

        private ReadOnlyObservableCollection<DisplayedElement> m_readOnlyElements;

        public MapModelView(MapControl view, DlmReader dlmFile)
        {
            m_view = view;
            m_dlmFile = dlmFile;
            m_readOnlyElements = new ReadOnlyObservableCollection<DisplayedElement>(m_elements);
            m_selectedItems.CollectionChangedRange += OnSelectedItemsChanged;
            m_layers.CollectionChangedRange += OnLayersChanged;

            BuildMap();
        }


        public DlmReader DlmFile
        {
            get { return m_dlmFile; }
        }

        public ReadOnlyObservableCollection<DisplayedElement> Elements
        {
            get
            {
                return m_readOnlyElements;
            }
        }

        public ObservableCollectionRange<Layer> Layers
        {
            get
            {
                return m_layers;
            }
        }

        private ObservableCollectionRange<DisplayedElement> m_selectedItems = new ObservableCollectionRange<DisplayedElement>();

        public ObservableCollectionRange<DisplayedElement> SelectedItems
        {
            get { return m_selectedItems; }
        }

        private GfxElement m_selectedItem;

        public GfxElement SelectedItem
        {
            get { return m_selectedItem; }
            set { m_selectedItem = value;
            SelectedItems.Clear();
            SelectedItems.Add(value);
            }
        }

        private GfxElement m_overElement;

        public GfxElement OverElement
        {
            get { return m_overElement; }
            private set {
                if (m_overElement != null)
                    m_overElement.IsMouseOver = false;

                m_overElement = value;
                if (m_overElement != null)
                    m_overElement.IsMouseOver = true;
            }
        }


        private void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                bool first = true;
                foreach (GfxElement element in e.NewItems)
                {
                    if (first && m_selectedItem != element)
                    {
                        m_selectedItem = element;
                        OnPropertyChanged("SelectedItem");
                    }

                    element.IsSelected = true;
                    first = false;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (GfxElement element in e.OldItems)
                    element.IsSelected = false;
            } 
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (GfxElement element in e.OldItems)
                    element.IsSelected = false;
            }
        }

        private void OnLayersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Layer layer in e.NewItems)
                {
                    m_elements.AddRange(layer.DisplayedElements);
                    layer.DisplayedElements.CollectionChangedRange += OnLayerElementsChanged;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Layer layer in e.OldItems)
                {
                    foreach (var element in layer.DisplayedElements)
                    {
                        m_elements.Remove(element);
                    }
                    layer.DisplayedElements.CollectionChangedRange -= OnLayerElementsChanged;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (Layer layer in e.OldItems)
                {
                    foreach (var element in layer.DisplayedElements)
                    {
                        m_elements.Remove(element);
                    }

                    layer.DisplayedElements.CollectionChangedRange -= OnLayerElementsChanged;
                }
            }
        }

        private void OnLayerElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                m_elements.AddRange(e.NewItems.OfType<DisplayedElement>());

            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (DisplayedElement element in e.OldItems)
                {
                    m_elements.Remove(element);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (DisplayedElement element in e.OldItems)
                {
                    m_elements.Remove(element);
                }
            }
        }

        #region MakeMap
        public void BuildMap()
        {
            Map = DlmFile.ReadMap();

            foreach (var dlmLayer in Map.Layers)
            {
                var layer = new ObjectsLayer(dlmLayer.LayerId);
                m_layers.Add(layer);
                var elements = new List<GfxElement>();
                foreach (var cell in dlmLayer.Cells)
                {
                    elements.AddRange(from element in cell.Elements.OfType<DlmGraphicalElement>()
                                      let graphicElement =
                                          WorldGFXManager.GetElement((int) element.ElementId) as
                                          NormalGraphicalElementData
                                      where graphicElement != null
                                      let gfx = WorldGFXManager.GetGfx(graphicElement.Gfx)
                                      where gfx != null
                                      select new GfxElement(element, graphicElement, gfx, layer));
                }

                layer.IsVisible = true;
                layer.Elements.AddRange(elements);
            }

            m_layers.Add(new GridLayer()
            {
                IsVisible = true
            });


        }

        public DlmMap Map
        {
            get;
            private set;
        }

        #endregion

        #region OverElementChangedCommand

        private DelegateCommand m_overElementChangedCommand;

        public DelegateCommand OverElementChangedCommand
        {
            get { return m_overElementChangedCommand ?? (m_overElementChangedCommand = new DelegateCommand(OnOverElementChanged, CanOverElementChanged)); }
        }

        private bool CanOverElementChanged(object parameter)
        {
            return true;
        }

        private void OnOverElementChanged(object parameter)
        {
            OverElement = parameter as GfxElement;
        }

        #endregion

        #region SelectedSingleElementCommand

        private DelegateCommand m_selectedSingleElementCommand;

        public DelegateCommand SelectedSingleElementCommand
        {
            get { return m_selectedSingleElementCommand ?? (m_selectedSingleElementCommand = new DelegateCommand(OnSelectedSingleElement, CanSelectedSingleElement)); }
        }

        private bool CanSelectedSingleElement(object parameter)
        {
            return parameter is GfxElement;
        }

        private void OnSelectedSingleElement(object parameter)
        {
            if (parameter == null || !CanSelectedSingleElement(parameter))
                return;

            var element = (GfxElement)parameter;
            SelectedItem = element;
        }

        #endregion



        public static Point GetCellCoords(short cellId)
        {
            return new Point((int)( cellId % MapEditorModelView.MapWidth ), (int)Math.Floor(cellId / (double)MapEditorModelView.MapWidth));
        }

        public static Point GetCellPixelCoords(short cellId)
        {
            var coords = GetCellCoords(cellId);

            coords.X = (int)( coords.X * MapEditorModelView.CellWidth + ( coords.Y % 2 == 1 ? ( MapEditorModelView.CellHalfWidth ) : ( 0 ) ) );
            coords.Y = (int)( coords.Y * MapEditorModelView.CellHalfHeight );

            return coords;
        }

        public static short GetCellByPixel(Point pixel)
        {
            var y = (int)Math.Floor(pixel.Y / MapEditorModelView.CellHalfHeight);
            var x = (int)( y % 2 == 1 ? Math.Floor(( pixel.X - MapEditorModelView.CellHalfWidth ) / MapEditorModelView.CellWidth) : Math.Floor(pixel.X / MapEditorModelView.CellWidth) );

            return (short)MapPoint.CoordToCellId(x, y);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged; 
    }
}
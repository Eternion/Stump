#region License GNU GPL
// MapEditorModelView.cs
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
    public class MapEditorModelView : INotifyPropertyChanged
    {
        public static uint MapWidth = 14;
        public static uint MapHeight = 20;
        public static uint MapCellsCount = 560;
        public static uint CellWidth = 86;
        public static uint CellHalfWidth = 43;
        public static uint CellHeight = 43;
        public static double CellHalfHeight = 21.5;
        public static uint AltitudePixelUnit = 10;
        public static double OverlayModeAlpha = 0.7;
        public static uint MaxZoom = 4;
        public static Point ResolutionHightQuality = new Point(1276, 876);
        public static Point ResolutionMediumQuality = new Point(957, 657);
        public static Point ResolutionLowQuality = new Point(638, 438);

        private readonly MapEditor m_editor;

        public MapEditorModelView(MapEditor editor, DlmReader map)
        {
            m_editor = editor;
            m_gfxs = new VirtualizingCollection<BrowsableGfx>(WorldGFXManager.GetGfxProvider(), 50);
            AddMap(map);
        }

        private VirtualizingCollection<BrowsableGfx> m_gfxs;

        public VirtualizingCollection<BrowsableGfx> Gfxs
        {
            get
            {
                return m_gfxs;
            }
        }

        private ObservableCollection<MapModelView> m_maps = new ObservableCollection<MapModelView>();

        public ObservableCollection<MapModelView> Maps
        {
            get { return m_maps; }
        }

        public MapControl AddMap(DlmReader map)
        {
            var control = new MapControl(map);
            m_editor.AddTab(control.ModelView.Map.Id.ToString(), control);
            return control;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
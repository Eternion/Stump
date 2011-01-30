// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.Classes.Custom
{	
	public class Map
	{
		public const uint MaximumCellsCount = 560;

		public uint Version
		{
			get;
			internal set;
		}

		public int Id
		{
			get;
			internal set;
		}

		public uint RelativeId
		{
			get;
			internal set;
		}

		public int MapType
		{
			get;
			internal set;
		}

		public uint SubAreaId
		{
			get;
			internal set;
		}

		public int TopNeighbourId
		{
			get;
			internal set;
		}

		public int BottomNeighbourId
		{
			get;
			internal set;
		}

		public int LeftNeighbourId
		{
			get;
			internal set;
		}

		public int RightNeighbourId
		{
			get;
			internal set;
		}

		public int ShadowBonusOnEntities
		{
			get;
			internal set;
		}

		public bool UseLowpassFilter
		{
			get;
			internal set;
		}

		public bool UseReverb
		{
			get;
			internal set;
		}

		public int PresetId
		{
			get;
			internal set;
		}

		public List<CellData> CellsData
		{
			get;
			internal set;
		}

		public Dictionary<uint, MapObjectElement> MapElementsPositions
		{
			get;
			set;
		}

	}
}

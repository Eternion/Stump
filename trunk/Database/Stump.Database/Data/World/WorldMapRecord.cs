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
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("world_maps")]
    [AttributeAssociatedFile("WorldMaps")]
    public sealed class WorldMapRecord : DataBaseRecord<WorldMapRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("OrigineX")]
        public int OrigineX
        {
            get;
            set;
        }

        [Property("OrigineY")]
        public int OrigineY
        {
            get;
            set;
        }

        [Property("MapWidth")]
        public double MapWidth
        {
            get;
            set;
        }

        [Property("MapHeight")]
        public double MapHeight
        {
            get;
            set;
        }

        [Property("HorizontalChunck")]
        public uint HorizontalChunck
        {
            get;
            set;
        }

        [Property("VerticalChunck")]
        public uint VerticalChunck
        {
            get;
            set;
        }

        [Property("ViewableEverywhere")]
        public bool ViewableEverywhere
        {
            get;
            set;
        }

        [Property("MinScale")]
        public double MinScale
        {
            get;
            set;
        }

        [Property("MaxScale")]
        public double MaxScale
        {
            get;
            set;
        }

        [Property("StartScale")]
        public double StartScale
        {
            get;
            set;
        }

        [Property("CenterX")]
        public int CenterX
        {
            get;
            set;
        }

        [Property("CenterY")]
        public int CenterY
        {
            get;
            set;
        }

        [Property("TotalWidth")]
        public int TotalWidth
        {
            get;
            set;
        }

        [Property("TotalHeight")]
        public int TotalHeight
        {
            get;
            set;
        }
    }
}
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

namespace Stump.DofusProtocol.D2oClasses
{
    [AttributeAssociatedFile("WorldMaps")]
    public class WorldMap
    {
        public int centerX;
        public int centerY;
        public uint horizontalChunck;
        public int id;
        public double mapHeight;
        public double mapWidth;
        public double maxScale;
        public double minScale;
        public int origineX;
        public int origineY;
        public double startScale;
        public int totalHeight;
        public int totalWidth;
        public uint verticalChunck;
        public Boolean viewableEverywhere;
    }
}
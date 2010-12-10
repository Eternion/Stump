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
namespace Stump.DofusProtocol.D2oClasses
{
    [AttributeAssociatedFile("Areas")]
    public class Area
    {
        public bool _hasVisibleSubAreas;
        public bool _hasVisibleSubAreasInitialized;
        public string _name;
        private Rectangle _superArea;
        public object bounds;
        public bool containHouses;
        public bool containPaddocks;
        public int id;
        public int nameId;
        public int superAreaId;

        //private static var _allAreas:Array;
    }

    public class Rectangle
    {
        public int bottom;
        public Point bottomRight;
        public int height;
        public int left;
        public int right;
        public Point size;
        public int top;
        public Point topLeft;
        public int width;
        public int x;
        public int y;
    }

    public class Point
    {
        public double length;
        public int x;
        public int y;
    }
}
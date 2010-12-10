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
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
    [AttributeAssociatedFile("SubAreas")]
    public class SubArea
    {
        public List<AmbientSound> ambientSounds;
        public int areaId;
        public Rectangle bounds;
        public List<int> customWorldMap;
        public int id;
        public List<int> mapIds;
        public int nameId;
        public List<int> shape;
        public bool visibleOnProduction;
        public bool visibleOnTesting;
    }
}
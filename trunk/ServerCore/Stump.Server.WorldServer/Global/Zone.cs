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
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global
{
    public class Zone : WorldSpace
    {
        public override WorldSpaceType Type
        {
            get { return WorldSpaceType.Zone; }
        }

        public override void AddEntity(Entity entity)
        {
            base.AddEntity(entity);

            ParentSpace.AddEntity(entity);
        }

        public override void RemoveEntity(Entity entity)
        {
            base.RemoveEntity(entity);

            ParentSpace.RemoveEntity(entity);
        }

        #region Properties

        public Map[] Maps
        {
            get;
            set;
        }

        public int[] MapsId
        {
            get;
            set;
        }

        public Map[] CustomWorldMaps
        {
            get;
            set;
        }

        public int[] CustomWorldMapsId
        {
            get;
            set;
        }

        #endregion
    }
}
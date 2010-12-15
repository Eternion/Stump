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
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Entities
{
    public partial class LivingEntity
    {
        #region Delegates

        public delegate void EntityChangingMapHandler(LivingEntity entity, Map lastMap);

        public delegate void EntityMoveOperationHandler(LivingEntity entity, VectorIso positionInfo);
        public delegate void EntityCompressedMoveOperationHandler(LivingEntity entity, List<uint> movementKeys);

        #endregion

        public event EntityCompressedMoveOperationHandler EntityCompressedMovingStart;

        public void NotifyEntityCompressedMovingStart(List<uint> movementKeys)
        {
            EntityCompressedMoveOperationHandler handler = EntityCompressedMovingStart;

            if (handler != null)
                handler(this, movementKeys);
        }

        public event EntityMoveOperationHandler EntityMovingStart;

        public void NotifyEntityMovingStart(VectorIso positioninfo)
        {
            EntityMoveOperationHandler handler = EntityMovingStart;

            if (handler != null)
                handler(this, positioninfo);
        }

        public event EntityMoveOperationHandler EntityMovingEnd;

        public void NotifyEntityMovingEnd(VectorIso positioninfo)
        {
            EntityMoveOperationHandler handler = EntityMovingEnd;

            if (handler != null)
                handler(this, positioninfo);
        }

        public event EntityMoveOperationHandler EntityMovingStop;

        public void NotifyEntityMovingStop(VectorIso positioninfo)
        {
            EntityMoveOperationHandler handler = EntityMovingStop;

            if (handler != null)
                handler(this, positioninfo);
        }

        public event EntityMoveOperationHandler EntityTeleport;

        public void NotifyEntityTeleport(VectorIso positioninfo)
        {
            EntityMoveOperationHandler handler = EntityTeleport;

            if (handler != null)
                handler(this, positioninfo);
        }


        public event EntityChangingMapHandler EntityChangeMap;

        internal void NotifyChangeMap(Map lastmap)
        {
            EntityChangingMapHandler handler = EntityChangeMap;

            if (handler != null)
                handler(this, lastmap);
        }
    }
}
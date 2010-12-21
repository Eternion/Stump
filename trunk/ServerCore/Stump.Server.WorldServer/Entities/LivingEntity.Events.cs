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
using Stump.Server.WorldServer.Global.Pathfinding;

namespace Stump.Server.WorldServer.Entities
{
    public partial class LivingEntity
    {
        #region Delegates

        public delegate void EntityChangeMapHandler(LivingEntity entity, Map lastMap);

        public delegate void EntityMoveOperationHandler(LivingEntity entity, MovementPath movementPath);
        public delegate void EntityMoveCancelHandler(LivingEntity entity, VectorIsometric positionInfo);

        public delegate void EntityTeleportHandler(LivingEntity entity, VectorIsometric positionInfo);

        #endregion

        public event EntityMoveOperationHandler EntityMovingStart;

        public void NotifyEntityMovingStart(MovementPath movementPath)
        {
            EntityMoveOperationHandler handler = EntityMovingStart;

            if (handler != null)
                handler(this, movementPath);
        }

        public event EntityMoveOperationHandler EntityMovingEnd;
        
        public void NotifyEntityMovingEnd(MovementPath movementPath)
        {
            EntityMoveOperationHandler handler = EntityMovingEnd;

            if (handler != null)
                handler(this, movementPath);
        }

        public event EntityMoveCancelHandler EntityMovingCancel;

        public void NotifyEntityMovingCancel(VectorIsometric positioninfo)
        {
            EntityMoveCancelHandler handler = EntityMovingCancel;

            if (handler != null)
                handler(this, positioninfo);
        }


        public event EntityTeleportHandler EntityTeleport;

        public void NotifyEntityTeleport(VectorIsometric positioninfo)
        {
            EntityTeleportHandler handler = EntityTeleport;

            if (handler != null)
                handler(this, positioninfo);
        }


        public event EntityChangeMapHandler EntityChangeMap;

        internal void NotifyChangeMap(Map lastmap)
        {
            EntityChangeMapHandler handler = EntityChangeMap;

            if (handler != null)
                handler(this, lastmap);
        }
    }
}
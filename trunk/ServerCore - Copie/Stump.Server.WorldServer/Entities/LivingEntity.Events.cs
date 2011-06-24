
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Fights;
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
        public delegate void EntityMoveCancelHandler(LivingEntity entity, ObjectPosition positionInfo);
        public delegate void EntityTeleportHandler(LivingEntity entity, ObjectPosition positionInfo);

        public delegate void EntityFightHandler(LivingEntity entity, Fight fight);

        #endregion

        public event EntityMoveOperationHandler EntityMovingStart;

        private void NotifyEntityMovingStart(MovementPath movementPath)
        {
            EntityMoveOperationHandler handler = EntityMovingStart;

            if (handler != null)
                handler(this, movementPath);
        }

        public event EntityMoveOperationHandler EntityMovingEnd;

        protected void NotifyEntityMovingEnd(MovementPath movementPath)
        {
            EntityMoveOperationHandler handler = EntityMovingEnd;

            if (handler != null)
                handler(this, movementPath);
        }

        public event EntityMoveCancelHandler EntityMovingCancel;

        protected void NotifyEntityMovingCancel(ObjectPosition positioninfo)
        {
            EntityMoveCancelHandler handler = EntityMovingCancel;

            if (handler != null)
                handler(this, positioninfo);
        }


        public event EntityTeleportHandler EntityTeleport;

        protected void NotifyEntityTeleport(ObjectPosition positioninfo)
        {
            EntityTeleportHandler handler = EntityTeleport;

            if (handler != null)
                handler(this, positioninfo);
        }


        public event EntityChangeMapHandler EntityChangeMap;

        protected void NotifyChangeMap(Map lastmap)
        {
            EntityChangeMapHandler handler = EntityChangeMap;

            if (handler != null)
                handler(this, lastmap);
        }

        public event EntityFightHandler EntityEnterFight;

        protected void NotifyEntityEnterFight(Fight fight)
        {
            EntityFightHandler handler = EntityEnterFight;

            if (handler != null)
                handler(this, fight);
        }

        public event EntityFightHandler EntityLeaveFight;

        protected void NotifyEntityLeaveFight(Fight fight)
        {
            EntityFightHandler handler = EntityLeaveFight;

            if (handler != null)
                handler(this, fight);
        }

        public delegate void EntityEmoteHandler(LivingEntity entity, EmotesEnum emote);

        public event EntityEmoteHandler EntityEmoteStart;

        protected void NotifyEntityEmoteStart(EmotesEnum emote)
        {
            EntityEmoteHandler handler = EntityEmoteStart;
            if (handler != null)
                handler(this, emote);
        }

        public event EntityEmoteHandler EntityEmoteStop;

        protected void NotifyEntityEmoteStop(EmotesEnum emote)
        {
            EntityEmoteHandler handler = EntityEmoteStop;
            if (handler != null)
                handler(this, emote);
        }
    }
}
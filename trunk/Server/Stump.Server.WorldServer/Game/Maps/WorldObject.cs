using System;
using NLog;
using Stump.Core.Collections;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Exceptions;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Maps
{
    public abstract class WorldObject : IDisposable
    {
        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        internal readonly LockFreeQueue<IMessage> m_messageQueue = new LockFreeQueue<IMessage>();

        protected WorldObject()
        {
            CreationTime = DateTime.Now;
        }

        public abstract int Id
        {
            get;
            protected set;
        }

        public DateTime CreationTime
        {
            get;
            protected set;
        }

        public DateTime LastUpdateTime
        {
            get;
            protected internal set;
        }

        public virtual ObjectPosition Position
        {
            get;
            protected set;
        }

        public Cell Cell
        {
            get { return Position != null ? Position.Cell : null; }
            set { Position.Cell = value; }
        }

        public DirectionsEnum Direction
        {
            get { return Position != null ? Position.Direction : default(DirectionsEnum); }
            set { Position.Direction = value; }
        }

        public Map LastMap
        {
            get;
            internal set;
        }

        public Map NextMap
        {
            get;
            internal set;
        }

        public Map Map
        {
            get { return Position != null ? Position.Map : null; }
            set { Position.Map = value; }
        }

        public SubArea SubArea
        {
            get { return Position != null ? Position.Map.SubArea : null; }
        }

        public Area Area
        {
            get { return Position != null ? Position.Map.Area : null; }
        }

        public SuperArea SuperArea
        {
            get { return Position != null ? Position.Map.SuperArea : null; }
        }

        public virtual bool IsInWorld
        {
            get { return Position != null && Map != null && Area != null; }
        }

        public bool IsTeleporting
        {
            get;
            internal set;
        }

        public bool IsDeleted
        {
            get;
            protected set;
        }

        public bool IsDisposed
        {
            get;
            protected set;
        }

        public virtual UpdatePriority UpdatePriority
        {
            get { return UpdatePriority.LowPriority; }
        }

        public LockFreeQueue<IMessage> MessageQueue
        {
            get { return m_messageQueue; }
        }

        public virtual IContextHandler Context
        {
            get { return Area; }
        }

      

        #region IDisposable Members

        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            OnDisposed();
        }

        protected virtual void OnDisposed()
        {
            Position = null;
        }

        #endregion

        public void Delete()
        {
            Dispose();
        }

        public bool IsGonnaChangeZone()
        {
            return NextMap == null || NextMap.Area.Id != Area.Id;
        }

        public bool HasChangedZone()
        {
            return LastMap == null || LastMap.Area.Id != Area.Id;
        }

        public virtual bool CanBeSee(WorldObject byObj)
        {
            return byObj != null && !IsDeleted && !IsDisposed && byObj.Map != null && byObj.Map == Map;
        }

        public virtual bool CanSee(WorldObject obj)
        {
            if (obj == null || obj.IsDeleted || obj.IsDisposed)
                return false;

            return obj.CanBeSee(this);
        }
    }
}
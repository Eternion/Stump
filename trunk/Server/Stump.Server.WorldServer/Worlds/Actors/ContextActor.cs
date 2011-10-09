using System;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers.Chat;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Worlds.Actors
{
    public abstract class ContextActor
    {
        protected ContextActor()
        {
        }

        public virtual int Id
        {
            get;
            protected set;
        }

        public virtual EntityLook Look
        {
            get;
            protected set;
        }

        public virtual IContext Context
        {
            get
            {
                return Position.Map;
            }
        }

        public virtual ObjectPosition Position
        {
            get;
            protected set;
        }

        public Map Map
        {
            get { return Position.Map; }
        }

        public Cell Cell
        {
            get { return Position.Cell; }
        }

        public DirectionsEnum Direction
        {
            get { return Position.Direction; }
        }

        #region Network

        #region EntityDispositionInformations

        public virtual EntityDispositionInformations GetEntityDispositionInformations()
        {
            return new EntityDispositionInformations(Position.Cell.Id, (sbyte)Position.Direction);
        }

        #endregion

        #region GameContextActorInformations

        public virtual GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameContextActorInformations(
                Id,
                Look,
                GetEntityDispositionInformations());
        }

        public virtual IdentifiedEntityDispositionInformations GetIdentifiedEntityDispositionInformations()
        {
            return new IdentifiedEntityDispositionInformations(Position.Cell.Id, (sbyte) Position.Direction, Id);
        }

        #endregion

        #endregion

        #region Actions

        #region Chat

        public void DisplaySmiley(sbyte smileyId)
        {
            Context.ForEach(entry => ChatHandler.SendChatSmileyMessage(entry.Client, this, smileyId));
        }

        #endregion

        #region Moving
        public event Action<ContextActor, MovementPath> StartMoving;

        protected void NotifyStartMoving(MovementPath path)
        {
            Action<ContextActor, MovementPath> handler = StartMoving;
            if (handler != null) handler(this, path);
        }

        public event Action<ContextActor, MovementPath, bool> StopMoving;

        protected void NotifyStopMoving(MovementPath path, bool canceled)
        {
            Action<ContextActor, MovementPath, bool> handler = StopMoving;
            if (handler != null) handler(this, path, canceled);
        }

        public event Action<ContextActor, ObjectPosition> InstantMoved;

        protected void NotifyTeleported(ObjectPosition path)
        {
            Action<ContextActor, ObjectPosition> handler = InstantMoved;
            if (handler != null) handler(this, path);
        }

        private bool m_isMoving;

        public MovementPath MovementPath
        {
            get;
            private set;
        }

        public virtual bool IsMoving()
        {
            return m_isMoving && MovementPath != null;
        }

        public virtual bool CanMove()
        {
            return !IsMoving();
        }

        public virtual bool StartMove(MovementPath movementPath)
        {
            if (!CanMove())
                return false;

            m_isMoving = true;
            MovementPath = movementPath;

            NotifyStartMoving(movementPath);

            return true;
        }

        public virtual bool MoveInstant(ObjectPosition destination)
        {
            if (!CanMove())
                return true;

            Position = destination;

            NotifyTeleported(destination);

            return true;
        }

        public virtual bool StopMove()
        {
            if (!IsMoving())
                return false;

            Position = MovementPath.End;
            m_isMoving = false;

            NotifyStopMoving(MovementPath, false);
            MovementPath = null;

            return true;
        }

        public virtual bool StopMove(ObjectPosition currentObjectPosition)
        {
            if (!IsMoving() || !MovementPath.Contains(currentObjectPosition.Cell.Id))
                return false;

            Position = currentObjectPosition;
            m_isMoving = false;

            NotifyStopMoving(MovementPath, true);
            MovementPath = null;

            return true;
        }
        #endregion

        #endregion
    }
}
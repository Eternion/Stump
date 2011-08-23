using System;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
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
            Context.DoForAll(entry => ChatHandler.SendChatSmileyMessage(entry.Client, this, smileyId));
        }

        #endregion

        #region Moving
        public event Action<ContextActor, MovementPath> StartMoving;

        public void NotifyStartMoving(MovementPath path)
        {
            Action<ContextActor, MovementPath> handler = StartMoving;
            if (handler != null) handler(this, path);
        }

        public event Action<ContextActor, MovementPath, bool> StopMoving;

        public void NotifyStopMoving(MovementPath path, bool canceled)
        {
            Action<ContextActor, MovementPath, bool> handler = StopMoving;
            if (handler != null) handler(this, path, canceled);
        }

        public event Action<ContextActor, ObjectPosition> InstantMoved;

        public void NotifyTeleported(ObjectPosition path)
        {
            Action<ContextActor, ObjectPosition> handler = InstantMoved;
            if (handler != null) handler(this, path);
        }

        private bool m_isMoving;
        protected MovementPath m_movingPath;

        public bool IsMoving
        {
            get
            {
                return m_isMoving && m_movingPath != null;
            }
            protected set
            {
                m_isMoving = value;
            }
        }

        public virtual bool CanMove()
        {
            return !IsMoving;
        }

        public bool StartMove(MovementPath movementPath)
        {
            if (!CanMove())
                return false;

            IsMoving = true;
            m_movingPath = movementPath;

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
            if (!IsMoving)
                return false;

            Position = m_movingPath.End;
            IsMoving = false;

            NotifyStopMoving(m_movingPath, false);
            m_movingPath = null;

            return true;
        }

        public virtual bool StopMove(ObjectPosition currentObjectPosition)
        {
            if (!IsMoving || !m_movingPath.Contains(currentObjectPosition.Cell.Id))
                return false;

            Position = currentObjectPosition;
            IsMoving = false;

            NotifyStopMoving(m_movingPath, true);
            m_movingPath = null;

            return true;
        }
        #endregion

        #endregion
    }
}
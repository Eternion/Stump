using System;
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
        private ObjectPosition m_position;

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
            get { return Position.Map; }
        }

        public ObjectPosition Position
        {
            get { return m_position; }
            protected set
            {
                if (m_position != null)
                    m_position.PositionChanged -= NotifyPositionChanged;

                m_position = value;
                m_position.PositionChanged += NotifyPositionChanged;
            }
        }

        public Map Map
        {
            get { return Position.Map; }
            set { Position.Map = value; }
        }

        public Cell Cell
        {
            get { return Position.Cell; }
            set { Position.Cell = value; }
        }

        public DirectionsEnum Direction
        {
            get { return Position.Direction; }
            set { Position.Direction = value; }
        }

        #region Network

        #region EntityDispositionInformations

        public virtual EntityDispositionInformations GetEntityDispositionInformations()
        {
            return new EntityDispositionInformations(Position.Cell.Id, (sbyte) Position.Direction);
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

        private bool m_isMoving;
        private ObjectPosition m_lastPosition;

        public Path MovementPath
        {
            get;
            private set;
        }

        public event Action<ContextActor, Path> StartMoving;

        protected void NotifyStartMoving(Path path)
        {
            Action<ContextActor, Path> handler = StartMoving;
            if (handler != null) handler(this, path);
        }

        public event Action<ContextActor, Path, bool> StopMoving;

        protected void NotifyStopMoving(Path path, bool canceled)
        {
            NotifyPositionChanged(Position);

            Action<ContextActor, Path, bool> handler = StopMoving;
            if (handler != null)
                handler(this, path, canceled);
        }

        public event Action<ContextActor, ObjectPosition> InstantMoved;

        protected void NotifyTeleported(ObjectPosition path)
        {
            Action<ContextActor, ObjectPosition> handler = InstantMoved;
            if (handler != null) handler(this, path);
        }

        public event Action<ContextActor, ObjectPosition> PositionChanged;

        protected void NotifyPositionChanged(ObjectPosition position)
        {
            Action<ContextActor, ObjectPosition> handler = PositionChanged;
            if (handler != null) handler(this, position);
        }

        public virtual bool IsMoving()
        {
            return m_isMoving && MovementPath != null;
        }

        public virtual bool CanMove()
        {
            return !IsMoving();
        }

        public ObjectPosition GetPositionBeforeMove()
        {
            if (m_lastPosition != null)
                return m_lastPosition;

            return Position;
        }

        public virtual bool StartMove(Path movementPath)
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

            m_lastPosition = Position;
            Position = destination;

            NotifyTeleported(destination);

            return true;
        }

        public virtual bool StopMove()
        {
            if (!IsMoving())
                return false;

            m_lastPosition = Position;
            Position = MovementPath.EndPathPosition;
            m_isMoving = false;

            NotifyStopMoving(MovementPath, false);
            MovementPath = null;

            return true;
        }

        public virtual bool StopMove(ObjectPosition currentObjectPosition)
        {
            if (!IsMoving() || !MovementPath.Contains(currentObjectPosition.Cell.Id))
                return false;

            m_lastPosition = Position;
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
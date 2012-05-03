using System;
using System.Diagnostics.Contracts;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Handlers.Chat;

namespace Stump.Server.WorldServer.Game.Actors
{
    public abstract class ContextActor : WorldObject
    {
        private ObjectPosition m_position;

        public override int Id
        {
            get;
            protected set;
        }

        public virtual EntityLook Look
        {
            get;
            set;
        }

        public virtual ICharacterContainer CharacterContainer
        {
            get { return Position.Map; }
        }

        public override ObjectPosition Position
        {
            get { return m_position; }
            protected set
            {
                if (m_position != null)
                    m_position.PositionChanged -= OnPositionChanged;

                m_position = value;
                OnPositionChanged(m_position);

                if (m_position != null)
                    m_position.PositionChanged += OnPositionChanged;
            }
        }

        #region Network

        #region EntityDispositionInformations

        public virtual EntityDispositionInformations GetEntityDispositionInformations()
        {
            return new EntityDispositionInformations(Cell.Id, (sbyte) Direction);
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
            return new IdentifiedEntityDispositionInformations(Cell.Id, (sbyte) Direction, Id);
        }

        #endregion

        #endregion

        #region Actions

        #region Chat

        public void DisplaySmiley(sbyte smileyId)
        {
            CharacterContainer.ForEach(entry => ChatHandler.SendChatSmileyMessage(entry.Client, this, smileyId));
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

        protected virtual void OnStartMoving(Path path)
        {
            Action<ContextActor, Path> handler = StartMoving;
            if (handler != null) handler(this, path);
        }

        public event Action<ContextActor, Path, bool> StopMoving;

        protected virtual void OnStopMoving(Path path, bool canceled)
        {
            Action<ContextActor, Path, bool> handler = StopMoving;
            if (handler != null)
                handler(this, path, canceled);
        }
        
        public event Action<ContextActor, Cell> InstantMoved;

        protected virtual void OnInstantMoved(Cell cell)
        {   
            Action<ContextActor, Cell> handler = InstantMoved;
            if (handler != null) handler(this, cell);
        }

        public event Action<ContextActor, ObjectPosition> PositionChanged;

        protected virtual void OnPositionChanged(ObjectPosition position)
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
            return Map != null && !IsMoving();
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

            OnStartMoving(movementPath);

            return true;
        }

        public virtual bool MoveInstant(ObjectPosition destination)
        {
            if (!CanMove())
                return true;

            m_lastPosition = Position;
            Position = destination;

            OnInstantMoved(destination.Cell);

            return true;
        }

        public virtual bool StopMove()
        {
            if (!IsMoving())
                return false;

            m_lastPosition = Position;
            Position = MovementPath.EndPathPosition;
            m_isMoving = false;

            OnStopMoving(MovementPath, false);
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

            OnStopMoving(MovementPath, true);
            MovementPath = null;

            return true;
        }

        #endregion

        #endregion
    }
}
using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay
{
    public abstract class RolePlayActor : ContextActor
    {
        public event Action<RolePlayActor, Map> EnterMap;

        public virtual void OnEnterMap(Map map)
        {
            var handler = EnterMap;
            if (handler != null) handler(this, map);
        }

        public event Action<RolePlayActor, Map> LeaveMap;
        public virtual void OnLeaveMap(Map map)
        {
            var handler = LeaveMap;
            if (handler != null) handler(this, map);
        }

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
        {
            return new GameRolePlayActorInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations());
        }

        #endregion

        #region Actions

        #region Teleport

        public event Action<ContextActor, ObjectPosition> Teleported;

        protected virtual void OnTeleported(ObjectPosition position)
        {
            var handler = Teleported;
            if (handler != null) handler(this, position);
        }

        protected override void OnInstantMoved(Cell cell)
        {
            Map.Clients.Send(new TeleportOnSameMapMessage(Id, cell.Id));

            base.OnInstantMoved(cell);
        }

        public virtual bool Teleport(MapNeighbour mapNeighbour)
        {
            var neighbour = Position.Map.GetNeighbouringMap(mapNeighbour);

            if (neighbour == null)
                return false;

            var cell = Position.Map.GetCellAfterChangeMap(Position.Cell.Id, mapNeighbour);

            if (cell < 0 || cell >= 560)
            {
                logger.Error("Cell {0} out of range, current={1} neighbour={2}", cell, Cell.Id, mapNeighbour);
                return false;
            }

            var destination = new ObjectPosition(neighbour,
                cell, Position.Direction);

            return Teleport(destination);
        }

        public virtual bool Teleport(Map map, Cell cell)
        {
            return Teleport(new ObjectPosition(map, cell));
        }

        public virtual bool Teleport(ObjectPosition destination, bool performCheck = true)
        {
            if (IsMoving())
                StopMove();

            if (!CanChangeMap() && performCheck)
                return false;

            if (Position.Map == destination.Map)
                return MoveInstant(destination);

            NextMap = destination.Map;
            LastMap = Map;

            Position.Map.Leave(this);
            
            NextMap.Area.ExecuteInContext(() =>
                {
                    Position = destination.Clone();
                    Position.Map.Enter(this);

                    NextMap = null;
                    LastMap = null;

                    OnTeleported(Position);
                });

            return true;
        }

        public virtual bool CanChangeMap()
        {
            return Map != null && Map.IsActor(this);
        }

        #endregion

        #endregion

        protected override void OnDisposed()
        {
            if (Map != null && Map.IsActor(this))
            {
                Map.Area.ExecuteInContext(() =>
                {
                    if (Map != null && Map.IsActor(this))
                        Map.Leave(this);

                    base.OnDisposed();
                });
            }
            else
            {
                base.OnDisposed();
            }
        }
    }
}
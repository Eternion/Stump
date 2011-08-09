using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay
{
    public abstract class RolePlayActor : ContextActor
    {
        #region Events
        public event Action<RolePlayActor, Map> MapChanged;

        public void NotifyMapChanged(Map map)
        {
            Action<RolePlayActor, Map> handler = MapChanged;
            if (handler != null)
                handler(this, map);
        }

        #endregion

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayActorInformations(Id, Look, GetEntityDispositionInformations());
        }

        #endregion

        #region Actions

        #region Teleport

        public bool Teleport(MapNeighbour mapNeighbour)
        {
            if (IsMoving)
                StopMove();

            var destination = new ObjectPosition(Position.Map.GetNeighbouringMap(mapNeighbour),
                Position.Map.GetCellAfterChangeMap(Position.Cell.Id, mapNeighbour), Position.Direction);

            if (destination.Map == null)
                return false;

            return Teleport(destination);
        }

        public virtual bool Teleport(ObjectPosition destination)
        {
            if (IsMoving)
                StopMove();

            if (Position.Map == destination.Map)
                return MoveInstant(destination);

            Position.Map.Leave(this);
            Position = destination;
            Position.Map.Enter(this);

            NotifyMapChanged(Position.Map);

            return true;
        }

        #endregion

        #endregion
    }
}
using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class MoveAction : AIAction
    {
        public MoveAction(AIFighter fighter, Cell destinationCell)
            : base(fighter)
        {
            DestinationCell = destinationCell;
        }

        public MoveAction(AIFighter fighter, MapPoint destination)
            : base(fighter)
        {
            Destination = destination;
        }

        public Cell DestinationCell
        {
            get;
            private set;
        }

        public MapPoint Destination
        {
            get;
            private set;
        }

        public short DestinationId
        {
            get
            {
                return Destination == null ? DestinationCell.Id : Destination.CellId;
            }
        }

        protected override RunStatus Run(object context)
        {
            if (DestinationId == Fighter.Cell.Id)
                return RunStatus.Success;

            var pathfinder = new Pathfinder(new AIFightCellsInformationProvider(Fighter.Fight, Fighter));
            var path = pathfinder.FindPath(Fighter.Position.Cell.Id, DestinationId, false, Fighter.MP);

            if (path == null || path.IsEmpty())
                return RunStatus.Failure;

            if (path.MPCost > Fighter.MP)
                return RunStatus.Failure;

            return Fighter.StartMove(path) ? RunStatus.Success : RunStatus.Failure;
        }
    }
}
using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
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
            if (!Fighter.CanMove())
                return RunStatus.Failure;

            if (DestinationId == Fighter.Cell.Id)
                return RunStatus.Success;

            var pathfinder = new Pathfinder(new AIFightCellsInformationProvider(Fighter.Fight, Fighter));
            var path = pathfinder.FindPath(Fighter.Position.Cell.Id, DestinationId, false, Fighter.MP);

            if (path == null || path.IsEmpty())
                return RunStatus.Failure;

            if (path.MPCost > Fighter.MP)
                return RunStatus.Failure;

            bool success = Fighter.StartMove(path);
            var lastPos = Fighter.Cell.Id;

            // re-attempt to move if we didn't reach the cell i.e as we trigger a trap
            while (success && Fighter.Cell.Id != DestinationId && Fighter.CanMove())
            {
                path = pathfinder.FindPath(Fighter.Position.Cell.Id, DestinationId, false, Fighter.MP);

                if (path == null || path.IsEmpty())
                    return RunStatus.Failure;

                if (path.MPCost > Fighter.MP)
                    return RunStatus.Failure;

                success = Fighter.StartMove(path);

                // the mob didn't move so we give up
                if (Fighter.Cell.Id == lastPos)
                    return RunStatus.Failure;

                lastPos = Fighter.Cell.Id;
            }

            return success ? RunStatus.Success : RunStatus.Failure;
        }
    }
}
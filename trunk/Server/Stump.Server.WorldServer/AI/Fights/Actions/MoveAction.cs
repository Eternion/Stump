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
        public const int MaxMovesTries = 20;

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

            Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
            bool success = Fighter.StartMove(path);
            var lastPos = Fighter.Cell.Id;

            int tries = 0;
            // re-attempt to move if we didn't reach the cell i.e as we trigger a trap
            while (success && Fighter.Cell.Id != DestinationId && Fighter.CanMove() && tries <= MaxMovesTries)
            {
                path = pathfinder.FindPath(Fighter.Position.Cell.Id, DestinationId, false, Fighter.MP);

                if (path == null || path.IsEmpty())
                {
                    Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    return RunStatus.Failure;
                }

                if (path.MPCost > Fighter.MP)
                {
                    Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    return RunStatus.Failure;
                }

                success = Fighter.StartMove(path);

                // the mob didn't move so we give up
                if (Fighter.Cell.Id == lastPos)
                {
                    Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    return RunStatus.Failure;
                }

                lastPos = Fighter.Cell.Id;
                tries++; // avoid infinite loops
            }

            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);

            return success ? RunStatus.Success : RunStatus.Failure;
        }
    }
}
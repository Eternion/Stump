using System;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class MoveAction : AIAction
    {
        public MoveAction(AIFighter fighter, Cell destinationCell)
            : base(fighter)
        {
            DestinationCell = destinationCell;
        }

        public Cell DestinationCell
        {
            get;
            private set;
        }

        public override void Execute()
        {
            var pathfinder = new Pathfinder(new AIFightCellsInformationProvider(Fighter.Fight, Fighter));
            var path = pathfinder.FindPath(Fighter.Position.Cell.Id, DestinationCell.Id, false, Fighter.MP);

            Fighter.StartMove(path);
        }
    }
}
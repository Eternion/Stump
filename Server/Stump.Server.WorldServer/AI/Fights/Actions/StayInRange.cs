using System.Linq;
using Stump.ORM.SubSonic.Query;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class StayInRange : AIAction
    {
        public StayInRange(AIFighter fighter, int minRange, int maxRange, bool los)
            : base(fighter)
        {
            MinRange = minRange;
            MaxRange = maxRange;
            LoS = los;
        }

        public int MinRange
        {
            get;
            set;
        }

        public int MaxRange
        {
            get;
            set;
        }

        public bool LoS
        {
            get;
            set;
        }

        protected override RunStatus Run(object context)
        {
            if (!Fighter.CanMove())
                return RunStatus.Failure;

            var enemies = Fighter.Brain.Environment.GetVisibleEnemies().ToArray();

            if (enemies.Length <= 0)
                return RunStatus.Failure;
            var result = 
                (from enemy in enemies
                from cell in new LozengeSet(enemy.Position.Point, MaxRange, MinRange).EnumerateValidPoints()
                where Fighter.Brain.Environment.CellInformationProvider.IsCellWalkable(cell.CellId) &&
                      (!LoS || Fighter.Fight.CanBeSeen(cell, enemy.Position.Point))
                orderby cell.ManhattanDistanceTo(Fighter.Position.Point)
                select cell).FirstOrDefault();

            // too far away, just try to move closer
            if (result == null)
            {
                var zone = enemies.Select(x => (Set)new LozengeSet(x.Position.Point, MaxRange, MinRange))
                              .Aggregate((set, current) => set.IntersectWith(current));

                result = zone.EnumerateValidPoints().OrderBy(x => x.ManhattanDistanceTo(Fighter.Position.Point)).FirstOrDefault();

                if (result == null)
                    return RunStatus.Failure;
            }

            var move = new MoveAction(Fighter, result) {AttemptOnly = true};
            return move.YieldExecute(context);
        }
    }
}
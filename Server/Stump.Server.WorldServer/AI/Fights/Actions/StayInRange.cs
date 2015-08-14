using System.Linq;
using Stump.ORM.SubSonic.Query;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class StayInRange : AIAction
    {
        public StayInRange(AIFighter fighter, int minRange, int maxRange)
            : base(fighter)
        {
            MinRange = minRange;
            MaxRange = maxRange;
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

        protected override RunStatus Run(object context)
        {
            if (!Fighter.CanMove())
                return RunStatus.Failure;

            var enemies = Fighter.Brain.Environment.GetVisibleEnemies().ToArray();

            if (enemies.Length <= 0)
                return RunStatus.Failure;

            var zone = enemies.Select(x => (Set)new LozengeSet(x.Position.Point, MaxRange, MinRange))
                              .Aggregate((set, current) => set.IntersectWith(current));

            var result = zone.EnumerateValidPoints().Where(x =>
                Fighter.Brain.Environment.CellInformationProvider.IsCellWalkable(x.CellId)).
                              OrderBy(x => x.ManhattanDistanceTo(Fighter.Position.Point)).FirstOrDefault();

            if (result == null)
                return RunStatus.Failure;

            var move = new MoveAction(Fighter, result) {AttemptOnly = true};
            return move.YieldExecute(context);
        }
    }
}
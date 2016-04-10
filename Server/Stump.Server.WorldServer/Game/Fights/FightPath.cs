using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightPath : Path
    {
        public FightPath(FightActor fighter, IFight fight, Path path)
            : base(fight.Map, path.GetPath())
        {
            Fight = fight;
            Fighter = fighter;
            AdjustPath();
        }

        public IFight Fight
        {
            get;
        }

        public FightActor Fighter
        {
            get;
        }

        public int TackledMP
        {
            get;
            private set;
        }

        public int TackledAP
        {
            get;
            private set;
        }

        public FightActor[] Tacklers
        {
            get;
            private set;
        }

        public bool BlockedByObstacle
        {
            get;
            private set;
        }

        private void AdjustPath()
        {
            int mp = Fighter.MP;
            int i = 1;

            var path = new List<Cell>() {StartCell};
            var tacklers = new List<FightActor>();
            var cell = StartCell;

            var obstaclesCells = Fight.GetAllFighters(entry => entry != Fighter && entry.Position.Cell != Fighter.Cell && entry.IsAlive()).Select(entry => entry.Cell.Id).ToList();


            while (i < Cells.Length && mp > 0 && !obstaclesCells.Contains(Cells[i].Id) && !Fight.ShouldTriggerOnMove(cell, Fighter))
            {
                int tackledMP = 0;
                int tackledAP = 0;
                if ((tackledMP = Fighter.GetTackledMP(cell)) > 0)
                {
                    if (tackledMP > mp)
                        tackledMP = mp;

                    mp -= tackledMP;
                    TackledMP += tackledMP;
                }

                if ((tackledAP = Fighter.GetTackledAP(cell)) > 0)
                {
                    TackledAP += tackledAP;
                }

                tacklers.AddRange(Fighter.GetTacklers(cell));

                if (mp > 0)
                {
                    path.Add(Cells[i]);
                    cell = Cells[i];
                    mp--;
                    i++;
                }
            }

            BlockedByObstacle = obstaclesCells.Contains(cell.Id);
            Cells = path.ToArray();
            Tacklers = tacklers.Distinct().ToArray();
        }
    }
}
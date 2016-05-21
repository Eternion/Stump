using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightTackle
    {
        public FightTackle(FightActor[] tacklers, Cell cell, int index, int tackledAp, int tackledMp)
        {
            Tacklers = tacklers;
            Cell = cell;
            PathIndex = index;
            TackledAP = tackledAp;
            TackledMP = tackledMp;
        }

        public FightActor[] Tacklers
        {
            get;
        }

        public Cell Cell
        {
            get;
        }

        public int PathIndex
        {
            get;
        }

        public int TackledAP
        {
            get;
        }

        public int TackledMP
        {
            get;
        }
    }

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

        public FightTackle[] Tackles
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
            int ap = Fighter.AP;
            int i = 1;

            var path = new List<Cell>() {StartCell};
            var tackles = new List<FightTackle>();
            var cell = StartCell;

            var obstaclesCells = Fight.GetAllFighters(entry => entry != Fighter && entry.Position.Cell != Fighter.Cell && entry.IsAlive()).Select(entry => entry.Cell.Id).ToList();


            while (i < Cells.Length && mp > 0 && !obstaclesCells.Contains(Cells[i].Id) && (cell == StartCell || !Fight.ShouldTriggerOnMove(cell, Fighter)))
            {
                int tackledMP = 0;
                int tackledAP = 0;
                if ((tackledMP = Fighter.GetTackledMP(mp, cell)) > 0)
                {
                    if (tackledMP > mp)
                        tackledMP = mp;

                    mp -= tackledMP;
                }

                if ((tackledAP = Fighter.GetTackledAP(ap, cell)) > 0)
                {
                    if (tackledAP > ap)
                        tackledAP = ap;

                    ap -= tackledAP;
                }

                if (tackledAP > 0 || tackledMP > 0)
                    tackles.Add(new FightTackle(Fighter.GetTacklers(cell), cell, i - 1, tackledAP, tackledMP));


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
            Tackles = tackles.ToArray();
        }
    }
}
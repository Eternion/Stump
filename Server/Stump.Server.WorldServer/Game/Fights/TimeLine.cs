using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class TimeLine
    {
        private readonly List<FightActor> m_passedActors = new List<FightActor>();  

        public TimeLine(IFight fight)
        {
            Fight = fight;
            Fighters = new List<FightActor>();
            RoundNumber = 1;
            NewRound = true;
        }

        internal List<FightActor> Fighters
        {
            get;
            private set;
        }

        public IFight Fight
        {
            get;
        }

        public FightActor Current => Index == -1 || Index >= Fighters.Count ? null : Fighters[Index];

        public FightActor Next => Index == -1 || (Index + 1) > Fighters.Count ? Fighters.FirstOrDefault(x => x.IsAlive()) : Fighters.Skip(Index + 1).FirstOrDefault(x => x.IsAlive());

        public int Index
        {
            get;
            private set;
        }

        public int Count => Fighters.Count;

        public short RoundNumber
        {
            get;
            private set;
        }

        public bool NewRound
        {
            get;
            private set;
        }

        public IReadOnlyCollection<FightActor> PassedActors => m_passedActors.AsReadOnly(); 

        public bool RemoveFighter(FightActor fighter)
        {
            if (!Fighters.Contains(fighter))
                return false;

            var index = Fighters.IndexOf(fighter);

            Fighters.Remove(fighter);

            if (index > Index)
                return true;

            if (index > 0)
                Index--;
            else
                Index = Fighters.Count - 1;

            return true;
        }

        public bool InsertFighter(FightActor fighter, int index)
        {
            if (index > Fighters.Count)
                return false;

            Fighters.Insert(index, fighter);

            if (index <= Index)
            {
                Index++;
            }

            return true;
        }

        public bool SelectNextFighter()
        {
            m_passedActors.Clear();

            if (Fighters.Count == 0)
            {
                Index = -1;
                return false;
            }

            var counter = 0;
            var index = ( Index + 1 ) < Fighters.Count ? Index + 1 : 0;

            if (index == 0)
            {
                RoundNumber++;
                NewRound = true;
            }
            else
            {
                NewRound = false;
            }

            while (!Fighters[index].IsAlive() && counter < Fighters.Count)
            {
                m_passedActors.Add(Fighters[index]);
                index = ( index + 1 ) < Fighters.Count ? index + 1 : 0;

                if (index == 0)
                {
                    RoundNumber++;
                    NewRound = true;
                }

                counter++;
            }

            if (!Fighters[index].IsAlive()) // no fighter can play
            {
                Index = -1;
                return false;
            }

            Index = index;

            return true;
        }

        public void OrderLine()
        {
            var redAvgInit = Fight.ChallengersTeam.Fighters.Average(x => x.Stats.Initiative.TotalWithLife);
            var blueAvgInit = Fight.DefendersTeam.Fighters.Average(x => x.Stats.Initiative.TotalWithLife);

            var redFighters = Fight.ChallengersTeam.GetAllFighters().
                OrderByDescending(entry => entry.Stats.Initiative.TotalWithLife);
            var blueFighters = Fight.DefendersTeam.GetAllFighters().
                OrderByDescending(entry => entry.Stats.Initiative.TotalWithLife);

            var redFighterFirst = redAvgInit >= blueAvgInit;

            var redEnumerator = redFighters.GetEnumerator();
            var blueEnumerator = blueFighters.GetEnumerator();
            var timeLine = new List<FightActor>();

            bool hasRed;
            bool hasBlue;
            while (( hasRed = redEnumerator.MoveNext() ) | ( hasBlue = blueEnumerator.MoveNext() ))
            {
                if (redFighterFirst)
                {
                    if (hasRed)
                        timeLine.Add(redEnumerator.Current);

                    if (hasBlue)
                        timeLine.Add(blueEnumerator.Current);
                }
                else
                {
                    if (hasBlue)
                        timeLine.Add(blueEnumerator.Current);

                    if (hasRed)
                        timeLine.Add(redEnumerator.Current);
                }
            }

            Fighters = timeLine;

            Index = 0;
        }
    }
}
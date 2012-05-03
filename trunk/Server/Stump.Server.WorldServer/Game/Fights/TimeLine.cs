using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class TimeLine
    {
        public TimeLine(Fight fight)
        {
            Fight = fight;
            Fighters = new List<FightActor>();
        }

        internal List<FightActor> Fighters
        {
            get;
            private set;
        }

        public Fight Fight
        {
            get;
            private set;
        }

        public FightActor Current
        {
            get
            {
                return Index == -1 ? null : Fighters[Index];
            }
        }

        public int Index
        {
            get;
            private set;
        }

        public int Count
        {
            get { return Fighters.Count; }
        }

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

        public bool RemoveFighter(FightActor fighter)
        {
            if (!Fighters.Contains(fighter))
                return false;

            var index = Fighters.IndexOf(fighter);

            Fighters.Remove(fighter);

            if (index <= Index && index > 0)
            {
                Index--;
            }

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
            if (Fighters.Count == 0)
            {
                Index = -1;
                return false;
            }

            int counter = 0;
            int index = ( Index + 1 ) < Fighters.Count ? Index + 1 : 0;

            if (index == 0)
            {
                RoundNumber++;
                NewRound = true;
            }
            else
            {
                NewRound = false;
            }

            while (!Fighters[index].CanPlay() && counter < Fighters.Count)
            {
                index = ( index + 1 ) < Fighters.Count ? index + 1 : 0;

                if (index == 0)
                {
                    RoundNumber++;
                    NewRound = true;
                }

                counter++;
            }

            if (!Fighters[index].CanPlay()) // no fighter can play
            {
                Index = -1;
                return false;
            }

            Index = index;

            return true;
        }

        public void OrderLine()
        {
            IOrderedEnumerable<FightActor> redFighters = Fight.RedTeam.GetAllFighters().
                OrderByDescending(entry => entry.Stats[PlayerFields.Initiative].Total);
            IOrderedEnumerable<FightActor> blueFighters = Fight.BlueTeam.GetAllFighters().
                OrderByDescending(entry => entry.Stats[PlayerFields.Initiative].Total);

            bool redFighterFirst = redFighters.First().Stats[PlayerFields.Initiative].Total >
                                   blueFighters.First().Stats[PlayerFields.Initiative].Total;

            IEnumerator<FightActor> redEnumerator = redFighters.GetEnumerator();
            IEnumerator<FightActor> blueEnumerator = blueFighters.GetEnumerator();
            var timeLine = new List<FightActor>();

            bool hasRed;
            bool hasBlue = false;
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
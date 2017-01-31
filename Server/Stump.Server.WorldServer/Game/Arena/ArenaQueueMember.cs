using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaQueueMember
    {
        [Variable]
        public static int ArenaMargeIncreasePerMinutes = 30;

        [Variable(true, DefinableRunning = true)]
        public static bool ArenaCheckIP = true;

        public ArenaQueueMember(Character character)
        {
            Character = character;
            InQueueSince = DateTime.Now;
        }

        public ArenaQueueMember(ArenaParty party)
        {
            Party = party;
            InQueueSince = DateTime.Now;
        }

        public Character Character
        {
            get;
            private set;
        }

        public ArenaParty Party
        {
            get;
            private set;
        }

        public int Level
        {
            get { return Party != null ? Party.GroupLevelAverage : Character.Level; }
        }

        public int ArenaRank
        {
            get { return Party != null ? Party.GroupRankAverage : Character.ArenaRank; }
        }

        public int MaxMatchableRank
        {
            get { return (int) (ArenaRank + ArenaMargeIncreasePerMinutes*(DateTime.Now - InQueueSince).TotalMinutes); }
        }
        
        public int MinMatchableRank
        {
            get { return (int) (ArenaRank - ArenaMargeIncreasePerMinutes*(DateTime.Now - InQueueSince).TotalMinutes); }
        }

        public DateTime InQueueSince
        {
            get;
            set;
        }

        public int MembersCount
        {
            get { return Party != null ? Party.MembersCount : 1; }
        }

        public bool IsBusy()
        {
            return EnumerateCharacters().Any(x => !x.CanEnterArena(false));
        }

        public IEnumerable<Character> EnumerateCharacters()
        {
            return Party != null ? Party.Members : Enumerable.Repeat(Character, 1);
        }

        public bool IsCompatibleWith(ArenaQueueMember member)
        {
            return Math.Max(member.MinMatchableRank, MinMatchableRank) <= Math.Max(member.MaxMatchableRank, MaxMatchableRank)
                && Math.Abs(member.Level - Level) < ArenaManager.ArenaMaxLevelDifference && (!ArenaCheckIP || !member.EnumerateCharacters().Any(x => EnumerateCharacters().Any(y => y.Client.IP == x.Client.IP)));
        }
    }
}
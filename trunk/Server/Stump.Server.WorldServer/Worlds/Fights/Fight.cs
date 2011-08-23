using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class Fight : IContext
    {
        public Fight(int id, FightTypeEnum fightType, FightTeam blueTeam, FightTeam redTeam)
        {
            Id = id;
            FightType = fightType;
            BlueTeam = blueTeam;
            RedTeam = redTeam;
        }

        public int Id
        {
            get;
            private set;
        }

        public FightTypeEnum FightType
        {
            get;
            private set;
        }

        public FightTeam BlueTeam
        {
            get;
            private set;
        }

        public FightTeam RedTeam
        {
            get;
            private set;
        }

        public IEnumerable<Character> GetAllCharacters()
        {
            throw new NotImplementedException();
        }

        public void DoForAll(Action<Character> action)
        {
            throw new NotImplementedException();
        }
    }
}
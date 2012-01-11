using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Parties;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class FightManager : EntityManager<FightManager, Fight>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public Fight Create(Map map, FightTypeEnum type)
        {
            var redTeam = new FightTeam(0, map.GetRedFightPlacement(), TeamEnum.TEAM_CHALLENGER);
            var blueTeam = new FightTeam(1, map.GetBlueFightPlacement(), TeamEnum.TEAM_CHALLENGER);

            var fight = new Fight(m_idProvider.Pop(), type, map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public void Remove(Fight fight)
        {
            RemoveEntity(fight.Id);

            m_idProvider.Push(fight.Id);
        }

        public Fight GetFight(int id)
        {
            return GetEntityOrDefault(id);
        }
    }
}
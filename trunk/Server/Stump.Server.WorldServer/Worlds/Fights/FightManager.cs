using System.Collections.Generic;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Parties;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class FightManager : Singleton<FightManager>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();
        private readonly Dictionary<int, Fight> m_fights = new Dictionary<int, Fight>();

        public Fight Create(Map map, FightTypeEnum type)
        {
            var redTeam = new FightTeam(0, new[] { map.Cells[328], map.Cells[356], map.Cells[357] }, TeamEnum.TEAM_CHALLENGER);
            var blueTeam = new FightTeam(1, new[] { map.Cells[370], map.Cells[355], map.Cells[354] }, TeamEnum.TEAM_CHALLENGER);

            var fight = new Fight(m_idProvider.Pop(), type, map, blueTeam, redTeam);

            m_fights.Add(fight.Id, fight);

            return fight;
        }

        public void Remove(Fight fight)
        {
            m_fights.Remove(fight.Id);

            m_idProvider.Push(fight.Id);
        }

        public Fight GetFight(int id)
        {
            return m_fights.ContainsKey(id) ? m_fights[id] : null;
        }
    }
}
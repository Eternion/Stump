using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Spawns;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters
{
    public class MonsterGroupWithAlternatives : MonsterGroup
    {
        private Dictionary<int, List<Monster>> m_monstersByMembersCount = new Dictionary<int, List<Monster>>();

        public MonsterGroupWithAlternatives(int id, ObjectPosition position, SpawningPoolBase spawningPool = null)
            : base(id, position, spawningPool)
        {
        }

        public void AddMonster(Monster monster, int membersCount)
        {
            if (!m_monstersByMembersCount.ContainsKey(membersCount))
                m_monstersByMembersCount.Add(membersCount, new List<Monster>());

            m_monstersByMembersCount[membersCount].Add(monster);
            base.AddMonster(monster);
        }

        public override IEnumerable<MonsterFighter> CreateFighters(FightMonsterTeam team)
        {
            var membersCount = (team.OpposedTeam.Leader as CharacterFighter)?.Character.Party?.MembersCount ?? 1;

            var group = m_monstersByMembersCount.OrderBy(x => x.Key).FirstOrDefault(x => x.Key <= membersCount).Value ?? GetMonsters();

            return group.Select(x => x.CreateFighter(team));
        }

        public override GroupMonsterStaticInformations GetGroupMonsterStaticInformations(Character character)
        {
            return new GroupMonsterStaticInformationsWithAlternatives(Leader.GetMonsterInGroupLightInformations(),
                GetMonstersWithoutLeader().Select(x => x.GetMonsterInGroupInformations()),
                m_monstersByMembersCount.Select(x => new AlternativeMonstersInGroupLightInformations(x.Key, x.Value.Select(y => y.GetMonsterInGroupLightInformations()))));
        }
    }
}
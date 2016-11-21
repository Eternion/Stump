using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Spawns;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters
{
    public class MonsterGroupWithAlternatives : MonsterGroup
    {
        private int m_lastGroupMinPlayers ;
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

        protected override int CountInitialFighters() => m_monstersByMembersCount.OrderByDescending(x => x.Key).FirstOrDefault(x => x.Key <= 1).Value.Count;

        public override IEnumerable<MonsterFighter> CreateFighters(FightMonsterTeam team)
        {
            var group = m_monstersByMembersCount.OrderByDescending(x => x.Key).FirstOrDefault(x => x.Key <= 1).Value ?? GetMonsters();

            team.OpposedTeam.FighterAdded += OnFighterAddedOrRemoved;
            team.OpposedTeam.FighterRemoved += OnFighterAddedOrRemoved;
            team.Fight.FightStarted += FightOnFightStarted;
            m_lastGroupMinPlayers = 1;

            return group.Select(x => x.CreateFighter(team));
        }

        private void FightOnFightStarted(IFight fight)
        {
            fight.ChallengersTeam.FighterAdded -= OnFighterAddedOrRemoved;
            fight.ChallengersTeam.FighterRemoved -= OnFighterAddedOrRemoved;
        }

        private void OnFighterAddedOrRemoved(FightTeam team, FightActor fighter)
        {
            var key = m_monstersByMembersCount.OrderByDescending(x => x.Key).FirstOrDefault(x => x.Key <= team.Fighters.Count).Key;

            if (key > 0 && key != m_lastGroupMinPlayers)
            {
                var group = m_monstersByMembersCount[key];

                team.Fight.DefendersTeam.RemoveAllFighters();

                foreach (var monster in group)
                    team.Fight.DefendersTeam.AddFighter(monster.CreateFighter(team.Fight.DefendersTeam));

                m_lastGroupMinPlayers = key;
            }
        }

        public override GroupMonsterStaticInformations GetGroupMonsterStaticInformations(Character character)
        {
            var displayedGroup = m_monstersByMembersCount.OrderByDescending(x => x.Key).FirstOrDefault().Value;

            return new GroupMonsterStaticInformationsWithAlternatives(displayedGroup.FirstOrDefault()?.GetMonsterInGroupLightInformations() ?? new MonsterInGroupLightInformations(),
                displayedGroup.Skip(1).Select(x => x.GetMonsterInGroupInformations()),
                m_monstersByMembersCount.OrderBy(x => x.Key).Select(x => new AlternativeMonstersInGroupLightInformations(x.Key, x.Value.Select(y => y.GetMonsterInGroupLightInformations()))));
        }
    }
}
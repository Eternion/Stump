using System;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Quests;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Quests.Objectives
{
    public class DefeatMonstersObjective : QuestObjective
    {
        private int m_completion;

        public DefeatMonstersObjective(Character character, QuestObjectiveTemplate template, bool finished, int monsterId, int amount)
            : base(character, template, finished)
        {
            Monster = MonsterManager.Instance.GetTemplate(monsterId);
            Amount = amount;
        }

        public MonsterTemplate Monster
        {
            get;
        }

        public int Amount
        {
            get;
        }

        public int Completion
        {
            get { return m_completion; }
            private set
            {
                if (m_completion + value > Amount)
                    value = Amount - m_completion;

                m_completion = value;

                if (m_completion >= Amount)
                    CompleteObjective();
            }
        }

        public override void EnableObjective()
        {
            Character.FightEnded += OnFightEnded;
        }

        private void OnFightEnded(Character character, CharacterFighter fighter)
        {
            if (!(fighter.Fight is FightPvM))
                return;

            var count = fighter.OpposedTeam.Fighters.OfType<MonsterFighter>().Count(x => x.IsDead() && x.Monster.Template == Monster);

            Completion += count;
        }

        public override void DisableObjective()
        {
            Character.FightEnded -= OnFightEnded;
        }

        public override QuestObjectiveInformations GetQuestObjectiveInformations()
        {
            return new QuestObjectiveInformationsWithCompletion((short)Template.Id, Finished, (short)Completion, (short)Amount);
        }
    }
}
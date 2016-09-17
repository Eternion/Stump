using System;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Quests;

namespace Stump.Server.WorldServer.Game.Quests
{
    public class QuestStep
    {
        public event Action<QuestStep> Finished;


        public QuestStep(Quest quest, QuestStepTemplate template)
        {
            Quest = quest;
            Template = template;
            Objectives = template.Objectives.Select(x => x.GenerateObjective()).ToArray();

            foreach (var objective in Objectives)
            {
                if (!objective.Finished)
                {
                    objective.Completed += OnObjectiveCompleted;
                    objective.EnableObjective();
                }
            }
        }

        public int Id => Template.Id;
        
        public Quest Quest
        {
            get;
            set;
        }

        public QuestStepTemplate Template
        {
            get;
            set;
        }

        public QuestObjective[] Objectives
        {
            get;
            set;
        }

        public QuestReward[] Rewards
        {
            get;
            set;
        }
        
        private void OnObjectiveCompleted(QuestObjective obj)
        {
            if (Objectives.All(x => x.Finished))
                FinishQuest();
        }

        public void FinishQuest()
        {
            GiveRewards();

            OnFinished();
        }

        public void CancelQuest()
        {
            OnFinished();
        }

        private void GiveRewards()
        {
            foreach (var reward in Rewards)
            {
                reward.GiveReward(Quest.Owner);
            }
        }


        public QuestActiveInformations GetQuestActiveInformations()
        {
            return new QuestActiveDetailedInformations((short) Quest.Id, (short) Id, Objectives.Select(x => x.GetQuestObjectiveInformations()));
        }

        protected virtual void OnFinished()
        {
            foreach (var objective in Objectives)
            {
                objective.Completed -= OnObjectiveCompleted;
                objective.DisableObjective();
            }

            Finished?.Invoke(this);
        }
    }
}
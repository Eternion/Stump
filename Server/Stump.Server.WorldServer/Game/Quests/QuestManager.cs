using System.Collections.Generic;
using System.Linq;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Quests;

namespace Stump.Server.WorldServer.Game.Quests
{
    public class QuestManager : DataManager<QuestManager>
    {
        private Dictionary<int, QuestTemplate> m_quests;
        private Dictionary<int, QuestStepTemplate> m_questsSteps;
        private Dictionary<int, QuestRewardTemplate> m_questsRewards;
        private Dictionary<int, QuestObjectiveTemplate> m_questsObjectives;

        [Initialization(InitializationPass.Tenth)]
        public override void Initialize()
        {
            m_quests = Database.Query<QuestTemplate>(QuestTemplateRelator.FecthQuery).ToDictionary(x => x.Id);
            m_questsSteps = Database.Query<QuestStepTemplate>(QuestStepRelator.FetchQuery).ToDictionary(x => x.Id);
            m_questsRewards = Database.Query<QuestRewardTemplate>(QuestRewardRelator.FetchQuery).ToDictionary(x => x.Id);
            m_questsObjectives = Database.Query<QuestObjectiveTemplate>(QuestObjectiveRelator.FetchQuery).ToDictionary(x => x.Id);

            foreach (var quest in m_quests.Values)
            {
                quest.Steps = quest.StepIds.Select(x => m_questsSteps[x]).ToArray();
            }

            foreach (var step in m_questsSteps.Values)
            {
                step.Objectives = step.ObjectiveIds.Select(x => m_questsObjectives[x]).ToArray();
                step.Rewards = step.RewardsIds.Select(x => m_questsRewards[x]).ToArray();
            }

            foreach (var objective in m_questsObjectives.Values)
            {
                objective.ObjectivesToTrigger = m_questsObjectives.Values.Where(x => x.TriggeredByObjectiveId == objective.Id).ToArray();
            }
        }

        public QuestTemplate GetQuestTemplate(int id)
        {
            QuestTemplate template;
            return m_quests.TryGetValue(id, out template) ? template : null;
        }

        public QuestStepTemplate GetQuestStep(int id)
        {
            QuestStepTemplate template;
            return m_questsSteps.TryGetValue(id, out template) ? template : null;
        }
    }
}
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Quests;

namespace Stump.Server.WorldServer.Game.Quests
{
    public class QuestStep
    {
        public QuestStep(Quest quest, QuestStepTemplate template)
        {
            Quest = quest;
            Template = template;
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

        public QuestActiveInformations GetQuestActiveInformations()
        {
            return new QuestActiveDetailedInformations((short) Quest.Id, (short) Id, Objectives.Select(x => x.GetQuestObjectiveInformations()));
        }
    }
}
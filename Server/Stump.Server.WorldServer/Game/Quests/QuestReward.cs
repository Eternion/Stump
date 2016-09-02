using Stump.Server.WorldServer.Database.Quests;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Quests
{
    public class QuestReward
    {
        public QuestReward(QuestRewardTemplate template)
        {
            Template = template;
        }

        public QuestRewardTemplate Template
        {
            get;
            set;
        }

        public void GiveReward(Character character)
        {

        }
    }
}
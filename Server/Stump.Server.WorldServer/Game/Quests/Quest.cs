using NLog;
using Stump.Server.WorldServer.Database.Quests;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Quests
{
    public class Quest
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private QuestRecord m_record;
        private bool m_isDirty;

        public Quest(Character owner, QuestRecord record)
        {
            m_record = record;

            Owner = owner;
            Template = QuestManager.Instance.GetQuestTemplate(record.QuestId);

            if (Template == null)
            {
                logger.Error($"Quest id {record.QuestId} doesn't exist");
                return;
            }


        }

        public Character Owner
        {
            get;
            private set;
        }

        public int Id => Template.Id;

        public QuestTemplate Template
        {
            get;
            private set;
        }

        public bool Finished
        {
            get
            {
                return m_record.Finished;
            }
            set { m_record.Finished = value; }
        }

        public QuestStep CurrentStep
        {
            get;
            set;
        }
     }
}
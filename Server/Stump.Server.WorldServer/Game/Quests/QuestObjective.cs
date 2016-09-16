using System;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Quests;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Quests
{
    public abstract class QuestObjective
    {
        public QuestObjective(Character character, QuestObjectiveTemplate template, bool finished)
        {
            Character = character;
            Template = template;
            Finished = finished;
        }

        public QuestObjectiveTemplate Template
        {
            get;
        }

        public bool Finished
        {
            get;
            set;
        }

        public Character Character
        {
            get;
        }

        public abstract void EnableObjective();
        public abstract void DisableObjective();

        public void CompleteObjective()
        {

        }

        public event Action<QuestObjective> Completed;
        public abstract QuestObjectiveInformations GetQuestObjectiveInformations();

        protected virtual void OnCompleted(QuestObjective obj)
        {
            Completed?.Invoke(obj);
        }
    }
}
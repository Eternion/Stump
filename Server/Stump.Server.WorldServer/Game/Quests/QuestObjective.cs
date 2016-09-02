using System;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Quests
{
    public abstract class QuestObjective
    {
        public event Action<QuestObjective> Completed;
        public abstract QuestObjectiveInformations GetQuestObjectiveInformations();

        protected virtual void OnCompleted(QuestObjective obj)
        {
            Completed?.Invoke(obj);
        }
    }
}
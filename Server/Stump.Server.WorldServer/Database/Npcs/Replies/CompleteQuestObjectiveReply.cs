using System.Linq;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("Objective", typeof (NpcReply), typeof (NpcReplyRecord))]
    public class CompleteQuestObjectiveReply : NpcReply
    {
        public int ObjectiveId
        {
            get { return Record.GetParameter<int>(0); }
            set { Record.SetParameter(0, value); }
        }

        public override bool CanExecute(Npc npc, Character character)
        {
            return base.CanExecute(npc, character);
        }

        public override bool Execute(Npc npc, Character character)
        {
            var objective = character.Quests.SelectMany(x => x.CurrentStep.Objectives).FirstOrDefault(x => x.Template.Id == ObjectiveId);

            objective.

            return base.Execute(npc, character);
        }
    }
}
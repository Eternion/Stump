using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{

    [Discriminator("EndDialog", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class EndDialogReply : NpcReply
    {
        public EndDialogReply(NpcReplyRecord record) : base(record)
        {
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            character.LeaveDialog();

            return true;
        }
    }
}
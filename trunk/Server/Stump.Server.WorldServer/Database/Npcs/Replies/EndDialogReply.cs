using System.Data.Entity.ModelConfiguration;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database
{
    public class EndDialogReplyConfiguration : EntityTypeConfiguration<EndDialogReply>
    {
        public EndDialogReplyConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("EndDialog"));
        }
    }

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
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
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

    public class EndDialogReply : Npcs.NpcReply
    {
        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            character.LeaveDialog();

            return true;
        }


    }
}
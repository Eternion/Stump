using System;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [ActiveRecord(DiscriminatorValue = "Dialog")]
    public class NpcContinueDialog : NpcReply
    {
        [BelongsTo("NextMessageId", Cascade = CascadeEnum.Delete)]
        public NpcMessage NextMessage
        {
            get;
            set;
        }

        public override void Execute(Npc npc, Character character)
        {
        }
    }
}
using System;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [ActiveRecord(DiscriminatorValue = "EndDialog")]
    public class NpcEndDialog : NpcReply
    {
        public override void Execute(Npc npc, Character character)
        {
            character.LeaveDialog();
        }
    }
}
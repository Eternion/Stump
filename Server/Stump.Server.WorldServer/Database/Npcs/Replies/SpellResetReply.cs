using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("SpellRestat", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class SpellResetReply : NpcReply
    {
        public SpellResetReply(NpcReplyRecord record)
            : base(record)
        {
            
        }

        public override bool Execute(Npc npc, Character character)
        {
            character.Spells.ForgetAllSpells();
            return true;
        }
    }
}
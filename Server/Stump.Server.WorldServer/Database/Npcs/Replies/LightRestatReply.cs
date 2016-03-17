using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("LightRestats", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class LightRestatReply : NpcReply
    {
        public LightRestatReply(NpcReplyRecord record) : base(record)
        {
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            character.Stats.Agility.Base = 0;
            character.Stats.Strength.Base = 0;
            character.Stats.Vitality.Base = 0;
            character.Stats.Wisdom.Base = 0;
            character.Stats.Intelligence.Base = 0;
            character.Stats.Chance.Base = 0;

            character.StatsPoints = (ushort) (character.Level*5);

            character.RefreshStats();

            if (RestatReply.RestatOnce)
                character.CanRestat = false;

            return true;
        }
    }
}
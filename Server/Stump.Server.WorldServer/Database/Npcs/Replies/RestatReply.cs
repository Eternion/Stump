using Stump.Core.Attributes;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("Restat", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class RestatReply : NpcReply
    {
        [Variable] public static bool RestatOnce = true;

        public RestatReply(NpcReplyRecord record) : base(record)
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

            character.Stats.Agility.Additional = 0;
            character.Stats.Strength.Additional = 0;
            character.Stats.Vitality.Additional = 0;
            character.Stats.Wisdom.Additional = 0;
            character.Stats.Intelligence.Additional = 0;
            character.Stats.Chance.Additional = 0;

            character.StatsPoints = (ushort) (character.Level*5);

            character.RefreshStats();

            if (RestatOnce)
                character.CanRestat = false;

            return true;
        }
    }
}
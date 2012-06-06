using System.Drawing;
using Castle.ActiveRecord;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [ActiveRecord(DiscriminatorValue = "LightRestat")]
    public class LightRestatReply : NpcReply
    {
        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            character.Stats.Agility.Base = character.PermanentAddedAgility;
            character.Stats.Strength.Base = character.PermanentAddedStrength;
            character.Stats.Vitality.Base = character.PermanentAddedVitality;
            character.Stats.Wisdom.Base = character.PermanentAddedWisdom;
            character.Stats.Intelligence.Base = character.PermanentAddedIntelligence;
            character.Stats.Chance.Base = character.PermanentAddedChance;

            character.StatsPoints = (ushort)( character.Level * 5 );

            character.RefreshStats();

            if (RestatReply.RestatOnce)
                character.CanRestat = false;

            return true;
        }
    }
}
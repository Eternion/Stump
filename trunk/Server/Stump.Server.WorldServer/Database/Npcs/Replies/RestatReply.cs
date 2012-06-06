using System.Drawing;
using Castle.ActiveRecord;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [ActiveRecord(DiscriminatorValue = "Restat")]
    public class RestatReply : NpcReply
    {
        [Variable]
        public static bool RestatOnce = true;

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

            character.PermanentAddedAgility = 0;
            character.PermanentAddedStrength = 0;
            character.PermanentAddedVitality = 0;
            character.PermanentAddedWisdom = 0;
            character.PermanentAddedIntelligence = 0;
            character.PermanentAddedChance = 0;

            character.StatsPoints = (ushort)( character.Level * 5 );

            character.RefreshStats();

            if (RestatOnce)
                character.CanRestat = false;

            return true;
        }
    }
}
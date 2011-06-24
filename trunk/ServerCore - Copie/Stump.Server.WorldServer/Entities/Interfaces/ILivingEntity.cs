
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public interface ILivingEntity
    {
        /// <summary>
        ///   Set or get Level of the character.
        /// </summary>
        int Level
        {
            get;
            set;
        }

        int BaseHealth
        {
            get;
            set;
        }

        int DamageTaken
        {
            get;
            set;
        }

        StatsFields Stats
        {
            get;
        }

        /// <summary>
        ///   Spell container of this entity.
        /// </summary>
        SpellInventory Spells
        {
            get;
        }

        bool IsInFight
        {
            get;
        }

        Fight Fight
        {
            get;
        }

        FightGroup FightGroup
        {
            get;
        }

        FightGroupMember Fighter
        {
            get;
        }
    }
}
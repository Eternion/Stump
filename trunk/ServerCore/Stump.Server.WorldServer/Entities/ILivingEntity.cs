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
        SpellCollection Spells
        {
            get;
        }

        GroupMember GroupMember
        {
            get;
            set;
        }

        Group Group
        {
            get;
        }

        /// <summary>
        ///   Indicate if the character is in a group.
        /// </summary>
        bool IsInGroup
        {
            get;
        }

        bool IsInFight
        {
            get;
        }

        Fight CurrentFight
        {
            get;
        }

        FightGroupMember CurrentFighter
        {
            get;
        }
    }
}
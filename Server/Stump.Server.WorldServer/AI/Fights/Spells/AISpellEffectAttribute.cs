using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class AISpellEffectAttribute : Attribute
    {
        public AISpellEffectAttribute(SpellCategory category)
        {
            Category = category;
        }
        public AISpellEffectAttribute(SpellCategory category, EffectsEnum effect)
        {
            Category = category;
            Effect = effect;
        }

        public SpellCategory Category
        {
            get;
            set;
        }

        // if null -> all effects bound to the effect handler class
        public EffectsEnum? Effect
        {
            get;
            set;
        }
    }
}
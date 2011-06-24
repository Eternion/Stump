
using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Effects.Executor
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class FightEffectAttribute : Attribute
    {
        public FightEffectAttribute(EffectsEnum effect)
        {
            Effect = effect;
            Generate = true;
        }

        public FightEffectAttribute(EffectsEnum effect, bool generate)
        {
            Effect = effect;
            Generate = generate;
        }

        public EffectsEnum Effect
        {
            get;
            set;
        }

        public bool Generate
        {
            get;
            set;
        }
    }
}
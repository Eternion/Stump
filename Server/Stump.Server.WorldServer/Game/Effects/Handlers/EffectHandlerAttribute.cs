using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Effects.Handlers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited = false)]
    public class EffectHandlerAttribute : Attribute
    {
        public EffectHandlerAttribute(EffectsEnum effect)
        {
            Effect = effect;
        }

        public EffectsEnum Effect
        {
            get;
            private set;
        }
    }
}
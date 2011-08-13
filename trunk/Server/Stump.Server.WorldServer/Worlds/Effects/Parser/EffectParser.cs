using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Worlds.Effects.Parser
{
    public abstract class EffectParser
    {
        public abstract EffectsEnum Effect
        {
            get;
        }

        public abstract void ParseItemEffect();
        public abstract void ParseFightEffect();
        public abstract void ParseUseableEffect();
    }
}
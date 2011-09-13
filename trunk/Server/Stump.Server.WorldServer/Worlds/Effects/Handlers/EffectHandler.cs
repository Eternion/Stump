using Stump.Server.WorldServer.Worlds.Effects.Instances;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers
{
    public abstract class EffectHandler
    {
        protected EffectHandler(EffectBase effect)
        {
            Effect = effect;   
        }

        public virtual EffectBase Effect
        {
            get;
            private set;
        }

        public abstract void Apply();
    }
}
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Effects.Handlers
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

        public abstract bool Apply();
    }
}
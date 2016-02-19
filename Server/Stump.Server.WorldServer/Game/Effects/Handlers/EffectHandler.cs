using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Effects.Handlers
{
    public abstract class EffectHandler
    {
        protected EffectHandler(EffectBase effect)
        {
            Effect = effect;
            Efficiency = 1.0;
        }

        public virtual EffectBase Effect
        {
            get;
            private set;
        }

        public bool Applied
        {
            get;
            private set;
        }

        /// <summary>
        /// Efficiency factor, increase or decrease effect's efficiency. Default is 1.0
        /// </summary>
        public double Efficiency
        {
            get;
            set;
        }

        public virtual bool CanApply()
        {
            return true;
        }

        public bool Apply()
        {
            return Applied = InternalApply();
        }

        protected abstract bool InternalApply();
    }
}
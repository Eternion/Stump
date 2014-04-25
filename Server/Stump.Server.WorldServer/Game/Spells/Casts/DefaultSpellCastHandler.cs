using System.Collections.Generic;
using System.Linq;
using Stump.Core.Threading;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [DefaultSpellCastHandler]
    public class DefaultSpellCastHandler : SpellCastHandler
    {
        protected bool m_initialized;

        public DefaultSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public SpellEffectHandler[] Handlers
        {
            get;
            protected set;
        }

        public override bool SilentCast
        {
            get { return m_initialized && Handlers.Any(entry => entry.RequireSilentCast()); }
        }

        public override void Initialize()
        {
            var random = new AsyncRandom();

            var effects = Critical ? SpellLevel.CriticalEffects : SpellLevel.Effects;
            var handlers = new List<SpellEffectHandler>();

            var rand = random.NextDouble();
            double randSum = effects.Sum(entry => entry.Random);
            var stopRand = false;
            foreach (var effect in effects)
            {
                if (effect.Random > 0)
                {
                    if (stopRand)
                        continue;

                    if (rand > effect.Random/randSum)
                    {
                        // effect ignored
                        rand -= effect.Random/randSum;
                        continue;
                    }

                    // random effect found, there can be only one
                    stopRand = true;
                }

                var handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, Spell, TargetedCell, Critical);

                if (MarkTrigger != null)
                    handler.MarkTrigger = MarkTrigger;

                handlers.Add(handler);
            }

            Handlers = handlers.ToArray();
            m_initialized = true;
       } 

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            foreach (var handler in Handlers)
            {
                handler.Apply();
            }
        }

        public override IEnumerable<SpellEffectHandler> GetEffectHandlers()
        {
            return Handlers;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Threading;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;

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

            List<EffectDice> effects = Critical ? SpellLevel.CritialEffects : SpellLevel.Effects;
            var handlers = new List<SpellEffectHandler>();

            double rand = random.NextDouble();
            double randSum = effects.Sum(entry => entry.Random);
            bool stopRand = false;
            foreach (EffectDice effect in effects)
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

                SpellEffectHandler handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, Spell,
                                                                                          TargetedCell, Critical);

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

            foreach (SpellEffectHandler handler in Handlers)
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
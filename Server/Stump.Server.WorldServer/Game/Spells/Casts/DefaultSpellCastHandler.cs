using System.Collections.Generic;
using System.Linq;
using Stump.Core.Threading;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;

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

        public override bool Initialize()
        {
            var random = new AsyncRandom();

            var effects = Critical ? SpellLevel.CriticalEffects : SpellLevel.Effects;
            var handlers = new List<SpellEffectHandler>();

            var groups = effects.GroupBy(x => x.Group);

            foreach(var groupEffects in groups)
            {
                var rand = random.NextDouble();
                double randSum = groupEffects.Sum(entry => entry.Random);
                var stopRand = false;
                foreach (var effect in groupEffects.OrderByDescending(x => x.Priority))
                {
                    if (effect.Random > 0)
                    {
                        if (stopRand)
                            continue;

                        if (rand > effect.Random / randSum)
                        {
                            // effect ignored
                            rand -= effect.Random / randSum;
                            continue;
                        }

                        // random effect found, there can be only one
                        stopRand = true;
                    }

                    var handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, Spell, TargetedCell, Critical);

                    if (MarkTrigger != null)
                        handler.MarkTrigger = MarkTrigger;

                    if (!handler.CanApply())
                        return false;

                    handlers.Add(handler);
                }
            }


            Handlers = handlers.ToArray();
            m_initialized = true;

            return true;
       } 

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            foreach (var handler in Handlers)
            {
                if (handler.Dice.Delay > 0)
                {
                    var affectedActors = handler.GetAffectedActors().ToArray();
                    handler.SetAffectedActors(affectedActors);

                    foreach (var target in affectedActors)
                    {
                        var id = target.PopNextBuffId();
                        var buff = new DelayBuff(id, target, Caster, handler.Dice, Spell, false, false, BuffTrigger)
                        {
                            Duration = (short)handler.Dice.Delay,
                            Token = handler
                        };

                        target.AddAndApplyBuff(buff, bypassMaxStack: true);
                    }
                }
                else
                    handler.Apply();
            }
        }

        public void BuffTrigger(DelayBuff buff, object token)
        {
            if (Fight.State == FightState.Ended)
                return;

            if (token is SpellEffectHandler)
                ((SpellEffectHandler) token).Apply();
        }

        public override IEnumerable<SpellEffectHandler> GetEffectHandlers()
        {
            return Handlers;
        }
    }
}
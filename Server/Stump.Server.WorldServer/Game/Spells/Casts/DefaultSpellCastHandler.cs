using System.Collections.Generic;
using System.Linq;
using Stump.Core.Threading;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [DefaultSpellCastHandler]
    public class DefaultSpellCastHandler : SpellCastHandler
    {
        protected bool m_initialized;

        public DefaultSpellCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public SpellEffectHandler[] Handlers
        {
            get;
            protected set;
        }

        public override bool SilentCast => m_initialized && Handlers.Any(entry => entry.RequireSilentCast());

        public override bool SeeCast(Character character) => Handlers.All(entry => entry.SeeCast(character));

        protected bool CheckWhenExecute;

        public override bool Initialize()
        {
            var random = new AsyncRandom();

            var effects = Critical && SpellLevel.CriticalEffects.Any() ? SpellLevel.CriticalEffects : SpellLevel.Effects;
            var handlers = new List<SpellEffectHandler>();

            var groups = effects.GroupBy(x => x.Group);
            double totalRandSum = effects.Sum(entry => entry.Random);
            var randGroup = random.NextDouble();
            var stopRandGroup = false;

            foreach (var groupEffects in groups)
            {
                double randSum = groupEffects.Sum(entry => entry.Random);

                if (randSum > 0)
                {
                    if (stopRandGroup)
                        continue;

                    if (randGroup > randSum / totalRandSum)
                    {
                        // group ignored
                        randGroup -= randSum / totalRandSum;
                        continue;
                    }

                    // random group found, there can be only one
                    stopRandGroup = true;
                }

                var randEffect = random.NextDouble();
                var stopRandEffect = false;

                foreach (var effect in groupEffects)
                {
                    if (groups.Count() <= 1)
                    {
                        if (effect.Random > 0)
                        {
                            if (stopRandEffect)
                                continue;

                            if (randEffect > effect.Random / randSum)
                            {
                                // effect ignored
                                randEffect -= effect.Random / randSum;
                                continue;
                            }

                            // random effect found, there can be only one
                            stopRandEffect = true;
                        }
                    }

                    var handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, this, TargetedCell, Critical);

                    if (MarkTrigger != null)
                        handler.MarkTrigger = MarkTrigger;

                    if (!handler.CanApply())
                        return false;

                    if (!CheckWhenExecute && handler.Targets.All(x => !x.CheckWhenExecute))
                        handler.SetAffectedActors(handler.GetAffectedActors());

                    handler.Efficiency = Informations.Efficiency;

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

            foreach (var handler in Handlers.OrderBy(x => x.Priority))
            {
                handler.Apply();
            }
        }

        public override IEnumerable<SpellEffectHandler> GetEffectHandlers() => Handlers;
    }
}
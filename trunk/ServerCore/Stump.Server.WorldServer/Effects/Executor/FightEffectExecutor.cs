
using System;
using System.Collections.Generic;
using System.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Effects.Executor
{
    public static partial class FightEffectExecutor
    {
        private class EffectMethod
        {
            public EffectMethod(Delegate @delegate, bool generateEffect)
            {
                Method = @delegate;
                GenerateEffect = generateEffect;
            }

            public Delegate Method
            {
                get;
                set;
            }

            public bool GenerateEffect
            {
                get;
                set;
            }
        }

        private static readonly Dictionary<EffectsEnum, EffectMethod> FunctionsEffects =
            new Dictionary<EffectsEnum, EffectMethod>();

        public static void Initialize()
        {
            foreach (MethodInfo method in typeof(FightEffectExecutor).GetMethods())
            {
                object[] attributs = method.GetCustomAttributes(false);

                foreach (object attribute in attributs)
                {
                    if (attribute is FightEffectAttribute)
                    {
                        FunctionsEffects.Add(( attribute as FightEffectAttribute ).Effect,
                                             new EffectMethod(method.ToDelegate(null), (attribute as FightEffectAttribute).Generate));
                    }
                }
            }
        }

        public static void ExecuteSpellEffects(Fight fight, SpellLevel spellLevel, FightGroupMember caster,
                                               CellLinked target, bool critical)
        {
            Func<EffectBase[]> geteffects = critical
                                                ? spellLevel.GetCriticalEffects
                                                : (Func<EffectBase[]>)spellLevel.GetEffects;

            foreach (EffectBase effect in geteffects())
            {
                if (FunctionsEffects.ContainsKey(effect.EffectId))
                {
                    var generatedEffect = FunctionsEffects[effect.EffectId].GenerateEffect ? effect.GenerateEffect(EffectGenerationContext.Spell) : effect;

                    FunctionsEffects[effect.EffectId].Method.DynamicInvoke(spellLevel, generatedEffect, fight, caster, target, critical);
                }
            }
        }

    }
}
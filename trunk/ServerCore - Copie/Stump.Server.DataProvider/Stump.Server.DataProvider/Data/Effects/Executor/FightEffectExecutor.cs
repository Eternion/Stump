//// /*************************************************************************
////  *
////  *  Copyright (C) 2010 - 2011 Stump Team
////  *
////  *  This program is free software: you can redistribute it and/or modify
////  *  it under the terms of the GNU General Public License as published by
////  *  the Free Software Foundation, either version 3 of the License, or
////  *  (at your option) any later version.
////  *
////  *  This program is distributed in the hope that it will be useful,
////  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
////  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////  *  GNU General Public License for more details.
////  *
////  *  You should have received a copy of the GNU General Public License
////  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
////  *
////  *************************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using Stump.BaseCore.Framework.Extensions;
//using Stump.BaseCore.Framework.Reflection;
//using Stump.DofusProtocol.Enums;
//using Stump.Server.DataProvider.Data.Spells;
//using Stump.Server.WorldServer.Fights;
//using Stump.Server.WorldServer.Global.Maps;
//using Stump.Server.WorldServer.Groups;

//namespace Stump.Server.WorldServer.Effects.Executor
//{
//    public static partial class FightEffectExecutor
//    {
//        private class EffectMethod
//        {
//            public EffectMethod(Delegate @delegate, bool generateEffect)
//            {
//                Method = @delegate;
//                GenerateEffect = generateEffect;
//            }

//            public Delegate Method
//            {
//                get;
//                set;
//            }

//            public bool GenerateEffect
//            {
//                get;
//                set;
//            }
//        }

//        private static readonly Dictionary<EffectsEnum, EffectMethod> FunctionsEffects =
//            new Dictionary<EffectsEnum, EffectMethod>();

//        public static void Initialize()
//        {
//            foreach (MethodInfo method in typeof(FightEffectExecutor).GetMethods())
//            {
//                object[] attributs = method.GetCustomAttributes(false);

//                foreach (object attribute in attributs)
//                {
//                    if (attribute is FightEffectAttribute)
//                    {
//                        FunctionsEffects.Add(( attribute as FightEffectAttribute ).Effect,
//                                             new EffectMethod(method.ToDelegate(null), (attribute as FightEffectAttribute).Generate));
//                    }
//                }
//            }
//        }

//        public static void ExecuteSpellEffects(Fight fight, SpellLevel spellLevel, FightGroupMember caster,
//                                               CellData target, bool critical)
//        {
//            Func<EffectBase[]> geteffects = critical
//                                                ? spellLevel.GetCriticalEffects
//                                                : (Func<EffectBase[]>)spellLevel.GetEffects;

//            foreach (EffectBase effect in geteffects())
//            {
//                if (FunctionsEffects.ContainsKey(effect.EffectId))
//                {
//                    var generatedEffect = FunctionsEffects[effect.EffectId].GenerateEffect ? effect.GenerateEffect(EffectGenerationContext.Spell) : effect;

//                    FunctionsEffects[effect.EffectId].Method.DynamicInvoke(spellLevel, generatedEffect, fight, caster, target, critical);
//                }
//            }
//        }

//    }
//}
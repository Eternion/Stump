using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.AI.Fights.Brain;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    public class SpellSelector
    {
        private readonly EnvironmentAnalyser m_environment;

        public SpellSelector(AIFighter fighter, EnvironmentAnalyser environment)
        {
            m_environment = environment;
            Fighter = fighter;
            Possibilities = new List<SpellCastInformations>();
            Priorities = new Dictionary<SpellCategory, int>()
            {
                {SpellCategory.Summoning, 5},
                {SpellCategory.Buff, 4},
                {SpellCategory.Damages, 3},
                {SpellCategory.Healing, 2},
                {SpellCategory.Curse, 1},
            };
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public List<SpellCastInformations> Possibilities
        {
            get;
            private set;
        }

        public Dictionary<SpellCategory, int> Priorities
        {
            get;
            set;
        }

        public bool CanReach(FightActor fighter, Spell spell, out Cell castCell)
        {
            var diff = Fighter.GetSpellRange(spell.CurrentSpellLevel) -
                       fighter.Position.Point.ManhattanDistanceTo(Fighter.Position.Point);
            if (diff >= 0)
            {
                castCell = Fighter.Cell;
                return true;
            }

            // reachable
            if (-diff <= Fighter.MP)
            {
                // test LoS
                if (spell.CurrentSpellLevel.CastTestLos)
                {
                    // todo : other strategy?
                    var cell =
                        m_environment.GetCellsWithLoS(fighter.Cell, new LozengeSet(fighter.Position.Point, Fighter.MP))
                                     .FirstOrDefault();

                    if (cell == null) // no cell available
                    {
                        castCell = null;
                        return false;
                    }
                    castCell = cell;
                    return true;
                }
                castCell = m_environment.GetCellToCastSpell(fighter.Cell, spell);
                return castCell != null;
            }


            castCell = null;
            return false;
        }

        public void AnalysePossibilities()
        {
            Possibilities = new List<SpellCastInformations>();
            foreach (var spell in Fighter.Spells.Values)
            {
                var category = SpellIdentifier.GetSpellCategories(spell);
                var spellLevel = spell.CurrentSpellLevel;
                var cast = new SpellCastInformations(spell);

                if (Fighter.AP < spellLevel.ApCost)
                    continue;

                if (spellLevel.StatesForbidden.Any(Fighter.HasState))
                    continue;

                if (spellLevel.StatesRequired.Any(state => !Fighter.HasState(state)))
                    continue;

                if (!Fighter.SpellHistory.CanCastSpell(spell.CurrentSpellLevel))
                    continue;

                // summoning is the priority
                if (( category & SpellCategory.Summoning ) != 0 && Fighter.CanSummon())
                {
                    var adjacentCell = m_environment.GetFreeAdjacentCell();

                    if (adjacentCell == null)
                        continue;

                    cast.IsSummoningSpell = true;
                    cast.SummonCell = adjacentCell;
                }
                else
                {
                    foreach (var fighter in Fighter.Fight.Fighters.Where(fighter => fighter.IsAlive() && fighter.IsVisibleFor(Fighter)))
                    {
                        Cell cell;
                        if (!CanReach(fighter, spell, out cell))
                            continue;

                        if (!Fighter.SpellHistory.CanCastSpell(spell.CurrentSpellLevel, fighter.Cell))
                            continue;


                        var impact = ComputeSpellImpact(spell, fighter);
                        

                        if (impact == null)
                            continue;

                        impact.CastCell = cell;
                        impact.Target = fighter;

                        if (impact.Damage < 0)
                            continue; // hurts more allies than boost them

                        cast.Impacts.Add(impact);
                    }
                }

                if (cast.Impacts.Count > 0)
                    Possibilities.Add(cast);
            }

            if (!Brain.Brain.DebugMode)
                return;

            Debug.WriteLine(Fighter.Name);
            foreach (var spell in Fighter.Spells.Values)
            {
                Debug.WriteLine("Spell {0} ({1}) :: {2}", spell.Template.Name, spell.Id, SpellIdentifier.GetSpellCategories(spell));

                var possibility = Possibilities.FirstOrDefault(x => x.Spell == spell);

                if (possibility == null)
                    continue;

                if (possibility.IsSummoningSpell)
                {
                    Debug.WriteLine("\tSummon Spell");
                }
                else
                {
                    var dumper = new ObjectDumper(8)
                    {
                        MemberPredicate = (member) => !member.Name.Contains("Target")
                    };

                    Debug.WriteLine("\t{0} Targets", possibility.Impacts.Count);
                    foreach (var impact in possibility.Impacts)
                    {
                        Debug.Write(dumper.DumpElement(impact));
                        if (impact.Target != null)
                            Debug.WriteLine("\t\tTarget = " + impact.Target is NamedFighter ? ( (NamedFighter)impact.Target ).Name : impact.Target.Id.ToString());
                    }
                }
            }
            Debug.WriteLine("");

            if (Fighter.Fight.AIDebugMode)
            {
                // highlight movements

            }
        }

        public IEnumerable<SpellCast> EnumerateSpellsCast()
        {
            foreach (var priority in Priorities.OrderByDescending(x => x.Value))
            {
                var impactComparer = new SpellImpactComparer(this, priority.Key);
                foreach (var possibleCast in Possibilities.OrderBy(x => x, new SpellCastComparer(this, priority.Key)))
                {
                    var category = SpellIdentifier.GetSpellCategories(possibleCast.Spell);

                    if (( category & priority.Key ) == 0)
                        continue;

                    if (Fighter.AP == 0)
                        yield break;

                    if (possibleCast.IsSummoningSpell)
                    {
                        yield return new SpellCast(possibleCast.Spell, possibleCast.SummonCell);
                    }
                    else
                    {
                        foreach(var impact in possibleCast.Impacts.OrderByDescending(x => x, impactComparer))
                        {
                            Cell castSpell = impact.CastCell;
                            if (impact.CastCell != Fighter.Cell && !CanReach(impact.Target, possibleCast.Spell, out castSpell))
                                continue;

                            var cast = new SpellCast(possibleCast.Spell, impact.Target.Cell);;
                            if (castSpell == Fighter.Cell)
                            {
                                yield return cast;
                            }
                            else
                            {
                                if (MapPoint.GetPoint(impact.CastCell).ManhattanDistanceTo(Fighter.Position.Point) > Fighter.MP)
                                    continue;

                                var cell = impact.Target.Position.Point.GetAdjacentCells(m_environment.CellInformationProvider.IsCellWalkable).
                                    OrderBy(entry => entry.ManhattanDistanceTo(Fighter.Position.Point)).FirstOrDefault();

                                if (cell == null)
                                    cell = impact.Target.Position.Point;

                                var pathfinder = new Pathfinder(m_environment.CellInformationProvider);
                                var path = pathfinder.FindPath(Fighter.Position.Cell.Id, castSpell.Id, false, Fighter.MP);

                                if (path.IsEmpty() || path.MPCost > Fighter.MP)
                                    continue;

                                cast.MoveBefore = path;

                                yield return cast;
                            }
                        }
                    }
                }
            }
        }

        public SpellTarget ComputeSpellImpact(Spell spell, FightActor mainTarget)
        {
            SpellTarget damages = null;
            var cast = SpellManager.Instance.GetSpellCastHandler(Fighter, spell, mainTarget.Cell, false);
            cast.Initialize();

            foreach (var handler in cast.GetEffectHandlers())
            {
                foreach (var target in handler.GetAffectedActors())
                {
                    CumulEffects(handler.Dice, ref damages, target, spell);
                }
            }
            return damages;
        }

        private void CumulEffects(EffectDice effect, ref SpellTarget spellImpact, FightActor target, Spell spell)
        {
            var isFriend = Fighter.Team.Id == target.Team.Id;
            var result = new SpellTarget();
            var targetType = effect.Targets;

            var category = SpellIdentifier.GetEffectCategories(effect.EffectId);

            if (category == 0)
                return;

            double chanceToHappen = 1.0; // 

            // When chances to happen is under 100%, then we reduce spellImpact accordingly, for simplicity, but after having apply damage bonus & reduction. 
            // So average damage should remain exact even if Min and Max are not. 
            if (effect.Random > 0)
                chanceToHappen = effect.Random / 100.0;

            if ((target is SummonedFighter))
                chanceToHappen /= 2; // It's much better to hit non-summoned foes => effect on summons (except allies summon for Osa) is divided by 2. 

            var min = (uint)Math.Min(effect.DiceNum, effect.DiceFace);
            var max = (uint)Math.Max(effect.DiceNum, effect.DiceFace);

            if (( category & SpellCategory.DamagesNeutral ) > 0)
                AdjustDamage(result, min, max, SpellCategory.DamagesNeutral, chanceToHappen,
                    Fighter.Stats.GetTotal(PlayerFields.NeutralDamageBonus) + Fighter.Stats.GetTotal(PlayerFields.DamageBonus) + Fighter.Stats.GetTotal(PlayerFields.PhysicalDamage),
                    Fighter.Stats.GetTotal(PlayerFields.DamageBonusPercent) + Fighter.Stats.GetTotal(PlayerFields.Strength),
                    target.Stats.GetTotal(PlayerFields.NeutralElementReduction),
                    target.Stats.GetTotal(PlayerFields.NeutralResistPercent),
                    isFriend);

            if (( category & SpellCategory.DamagesFire ) > 0)
                AdjustDamage(result, min, max, SpellCategory.DamagesNeutral, chanceToHappen,
                    Fighter.Stats.GetTotal(PlayerFields.FireDamageBonus) + Fighter.Stats.GetTotal(PlayerFields.DamageBonus) + Fighter.Stats.GetTotal(PlayerFields.MagicDamage),
                    Fighter.Stats.GetTotal(PlayerFields.DamageBonusPercent) + Fighter.Stats.GetTotal(PlayerFields.Intelligence),
                    target.Stats.GetTotal(PlayerFields.FireElementReduction),
                    target.Stats.GetTotal(PlayerFields.FireResistPercent),
                    isFriend);


            if (( category & SpellCategory.DamagesAir ) > 0)
                AdjustDamage(result, min, max, SpellCategory.DamagesNeutral, chanceToHappen,
                     Fighter.Stats.GetTotal(PlayerFields.AirDamageBonus) + Fighter.Stats.GetTotal(PlayerFields.DamageBonus) + Fighter.Stats.GetTotal(PlayerFields.MagicDamage),
                     Fighter.Stats.GetTotal(PlayerFields.DamageBonusPercent) + Fighter.Stats.GetTotal(PlayerFields.Agility),
                     target.Stats.GetTotal(PlayerFields.AirElementReduction),
                     target.Stats.GetTotal(PlayerFields.AirResistPercent),
                     isFriend);

            if (( category & SpellCategory.DamagesWater ) > 0)
                AdjustDamage(result, min, max, SpellCategory.DamagesNeutral, chanceToHappen,
                     Fighter.Stats.GetTotal(PlayerFields.WaterDamageBonus) + Fighter.Stats.GetTotal(PlayerFields.DamageBonus) + Fighter.Stats.GetTotal(PlayerFields.MagicDamage),
                     Fighter.Stats.GetTotal(PlayerFields.DamageBonusPercent) + Fighter.Stats.GetTotal(PlayerFields.Chance),
                     target.Stats.GetTotal(PlayerFields.WaterElementReduction),
                     target.Stats.GetTotal(PlayerFields.WaterResistPercent),
                     isFriend);

            if (( category & SpellCategory.DamagesEarth ) > 0)
                AdjustDamage(result, min, max, SpellCategory.DamagesNeutral, chanceToHappen,
                     Fighter.Stats.GetTotal(PlayerFields.EarthDamageBonus) + Fighter.Stats.GetTotal(PlayerFields.DamageBonus) + Fighter.Stats.GetTotal(PlayerFields.PhysicalDamage),
                     Fighter.Stats.GetTotal(PlayerFields.DamageBonusPercent) + Fighter.Stats.GetTotal(PlayerFields.Strength),
                     target.Stats.GetTotal(PlayerFields.EarthElementReduction),
                     target.Stats.GetTotal(PlayerFields.EarthResistPercent),
                     isFriend);

            if (( category & SpellCategory.Healing ) > 0)
            {
                var steal = ( category & SpellCategory.Damages ) > 0;
                if (steal)
                    target = Fighter; // Probably hp steal

                var hptoHeal = (uint)( Math.Max(0, target.MaxLifePoints - target.LifePoints) ); // Can't heal over max
                if (steal)
                {
                    result.MinHeal = Math.Min(hptoHeal, Math.Abs(result.MinDamage));
                    result.MaxHeal = Math.Min(hptoHeal, Math.Abs(result.MaxDamage));
                }
                else
                {
                    if (hptoHeal > 0)
                    {
                        AdjustDamage(result, (uint)Math.Min(effect.DiceNum, hptoHeal), (uint)Math.Min(effect.DiceFace, hptoHeal), SpellCategory.Healing, chanceToHappen,
                             Fighter.Stats.GetTotal(PlayerFields.HealBonus),
                             Fighter.Stats.GetTotal(PlayerFields.Intelligence),
                             0,
                             0, !isFriend);

                        if (result.Heal > hptoHeal)
                            if (isFriend)
                                result.MinHeal = result.MaxHeal = +hptoHeal;
                            else
                                result.MinHeal = result.MaxHeal = -hptoHeal;
                    }
                }
            }

            if (( category & SpellCategory.Buff ) > 0)
                if (isFriend)
                    result.Boost += spell.CurrentLevel * chanceToHappen;
                else
                    result.Boost -= spell.CurrentLevel * chanceToHappen;

            if (( category & SpellCategory.Curse ) > 0)
            {
                var ratio = spell.CurrentLevel * chanceToHappen;

                if (effect.EffectId == EffectsEnum.Effect_SkipTurn) // Let say this effect counts as 2 damage per level of the target
                    ratio = target.Level * 2 * chanceToHappen;

                if (isFriend)
                    result.Curse -= 2 * ratio;
                else
                    result.Curse += ratio;
            }
            if (isFriend)
                result.Add(result); // amplify (double) effects on friends. 


            if (!isFriend && ( ( category & SpellCategory.Damages ) > 0 ) && result.MinDamage > target.LifePoints) // Enough damage to kill the target => affect an arbitrary 50% of max heal (with at least current health), so strong spells are not favored anymore. 
            {
                double ratio = Math.Max(target.MaxLifePoints / 2d, target.LifePoints) / result.MinDamage;
                result.Multiply(ratio);
            }

            if (spellImpact != null)
                spellImpact.Add(result);
            else
                spellImpact = result;
        }

        private static void AdjustDamage(SpellTarget damages, uint damage1, uint damage2, SpellCategory category,
            double chanceToHappen, int addDamage, int addDamagePercent, int reduceDamage, int reduceDamagePercent, bool negativ)
        {
            double minDamage = damage1;
            double maxDamage = damage1 >= damage2 ? damage1 : damage2;
            if (reduceDamagePercent >= 100)
                return; // No damage
            minDamage = ( ( minDamage * ( 1 + ( addDamagePercent / 100.0 ) ) + addDamage ) - reduceDamage ) * ( 1 - ( reduceDamagePercent / 100.0 ) ) * chanceToHappen;
            maxDamage = ( ( maxDamage * ( 1 + ( addDamagePercent / 100.0 ) ) + addDamage ) - reduceDamage ) * ( 1 - ( reduceDamagePercent / 100.0 ) ) * chanceToHappen;

            if (minDamage < 0) minDamage = 0;
            if (maxDamage < 0) maxDamage = 0;


            if (negativ) // or IsFriend
            {
                minDamage *= -1.5; // High penalty for firing on friends
                maxDamage *= -1.5; // High penalty for firing on friends
            }

            switch (category)
            {
                case SpellCategory.DamagesNeutral:
                    damages.MinNeutral += minDamage;
                    damages.MaxNeutral += maxDamage;
                    break;
                case SpellCategory.DamagesFire:
                    damages.MinFire += minDamage;
                    damages.MaxAir += maxDamage;
                    break;
                case SpellCategory.DamagesAir:
                    damages.MinAir += minDamage;
                    damages.MaxAir += maxDamage;
                    break;
                case SpellCategory.DamagesWater:
                    damages.MinWater += minDamage;
                    damages.MaxWater += maxDamage;
                    break;
                case SpellCategory.DamagesEarth:
                    damages.MinEarth += minDamage;
                    damages.MaxEarth += maxDamage;
                    break;
                case SpellCategory.Healing:
                    damages.MinHeal += minDamage;
                    damages.MaxHeal += maxDamage;
                    break;
            }
        }
    }

    public class SpellCastComparer : IComparer<SpellCastInformations>
    {
        private readonly Dictionary<SpellCategory, Func<SpellCastInformations, SpellCastInformations, int>> m_comparers;
            

        private readonly SpellSelector m_spellSelector;

        public SpellCastComparer(SpellSelector spellSelector, SpellCategory category)
        {
            Category = category;
            m_spellSelector = spellSelector;
            m_comparers = new Dictionary<SpellCategory, Func<SpellCastInformations, SpellCastInformations, int>>()
            {
                {SpellCategory.Summoning, CompareSummon},
                {SpellCategory.Buff, CompareBoost},
                {SpellCategory.Damages, CompareDamage},
                {SpellCategory.Healing, CompareHeal},
                {SpellCategory.Curse, CompareCurse},
            };
        }

        public SpellCategory Category
        {
            get;
            set;
        }

        // priority order : summon > boost > damage > heal > curse
        public int Compare(SpellCastInformations cast1, SpellCastInformations cast2)
        {
            return m_comparers[Category](cast1, cast2);
        }

        public int CompareSummon(SpellCastInformations cast1, SpellCastInformations cast2)
        {
            return cast1.IsSummoningSpell.CompareTo(cast2.IsSummoningSpell);
        }

        public int CompareBoost(SpellCastInformations cast1, SpellCastInformations cast2)
        {
            if (cast1.Impacts.Count == 0 || cast2.Impacts.Count == 0)
                return cast1.Impacts.Count.CompareTo(cast2.Impacts.Count);

            var max1 = cast1.Impacts.Max(x => x.Boost);
            var max2 = cast2.Impacts.Max(x => x.Boost);
            var efficiency1 = GetEfficiency(cast1);
            var efficiency2 = GetEfficiency(cast2);

            return ( max1 * efficiency1 ).CompareTo(max2 * efficiency2);
        }

        public int CompareDamage(SpellCastInformations cast1, SpellCastInformations cast2)
        {
            if (cast1.Impacts.Count == 0 || cast2.Impacts.Count == 0)
                return cast1.Impacts.Count.CompareTo(cast2.Impacts.Count);

            var max1 = cast1.Impacts.Max(x => x.Damage);
            var max2 = cast2.Impacts.Max(x => x.Damage);
            var efficiency1 = GetEfficiency(cast1);
            var efficiency2 = GetEfficiency(cast2);

            return ( max1 * efficiency1 ).CompareTo(max2 * efficiency2);
        }

        public int CompareHeal(SpellCastInformations cast1, SpellCastInformations cast2)
        {
            if (cast1.Impacts.Count == 0 || cast2.Impacts.Count == 0)
                return cast1.Impacts.Count.CompareTo(cast2.Impacts.Count);

            var max1 = cast1.Impacts.Max(x => x.Heal);
            var max2 = cast2.Impacts.Max(x => x.Heal);
            var efficiency1 = GetEfficiency(cast1);
            var efficiency2 = GetEfficiency(cast2);

            return ( max1 * efficiency1 ).CompareTo(max2 * efficiency2);
        }

        public int CompareCurse(SpellCastInformations cast1, SpellCastInformations cast2)
        {
            if (cast1.Impacts.Count == 0 || cast2.Impacts.Count == 0)
                return cast1.Impacts.Count.CompareTo(cast2.Impacts.Count);

            var max1 = cast1.Impacts.Max(x => x.Curse);
            var max2 = cast2.Impacts.Max(x => x.Curse);
            var efficiency1 = GetEfficiency(cast1);
            var efficiency2 = GetEfficiency(cast2);

            return ( max1 * efficiency1 ).CompareTo(max2 * efficiency2);
        }

        // numer of cast possible with the current ap
        public int GetEfficiency(SpellCastInformations cast)
        {
            return (int)Math.Floor(m_spellSelector.Fighter.AP / (double)cast.Spell.CurrentSpellLevel.ApCost);
        }
    }

    public class SpellImpactComparer : IComparer<SpellTarget>
    {
        private static readonly Dictionary<SpellCategory, Func<SpellTarget, SpellTarget, int>> m_comparers = 
            new Dictionary<SpellCategory, Func<SpellTarget, SpellTarget, int>>()
            {
                {SpellCategory.Buff, CompareBoost},
                {SpellCategory.Damages, CompareDamage},
                {SpellCategory.Healing, CompareHeal},
                {SpellCategory.Curse, CompareCurse},
            };

        private readonly SpellSelector m_spellSelector;

        public SpellImpactComparer(SpellSelector spellSelector, SpellCategory category)
        {
            Category = category;
            m_spellSelector = spellSelector;
        }

        public SpellCategory Category
        {
            get;
            set;
        }

        public int Compare(SpellTarget cast1, SpellTarget cast2)
        {
            if (!m_comparers.ContainsKey(Category))
                return 0;

            return m_comparers[Category](cast1, cast2);
        }

        public static int CompareBoost(SpellTarget impact1, SpellTarget impact2)
        {
            return impact1.Boost.CompareTo(impact2.Boost);
        }

        public static int CompareDamage(SpellTarget impact1, SpellTarget impact2)
        {
            return impact1.Damage.CompareTo(impact2.Damage);
        }

        public static int CompareHeal(SpellTarget impact1, SpellTarget impact2)
        {
            return impact1.Heal.CompareTo(impact2.Heal);
        }

        public static int CompareCurse(SpellTarget impact1, SpellTarget impact2)
        {
            return impact1.Curse.CompareTo(impact2.Curse);
        }
    }
}
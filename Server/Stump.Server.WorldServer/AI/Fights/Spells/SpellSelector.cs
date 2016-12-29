using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Brain;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    public class SpellSelector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly EnvironmentAnalyser m_environment;

        public SpellSelector(AIFighter fighter, EnvironmentAnalyser environment)
        {
            m_environment = environment;
            Fighter = fighter;
            Possibilities = new List<SpellCastImpact>();
            Priorities = new Dictionary<SpellCategory, int>
            {
                {SpellCategory.Summoning, 5},
                {SpellCategory.Buff, 4},
                {SpellCategory.Damages, 3},
                {SpellCategory.Healing, 2},
                {SpellCategory.Curse, 1}
            };
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public List<SpellCastImpact> Possibilities
        {
            get;
            private set;
        }

        public Dictionary<SpellCategory, int> Priorities
        {
            get;
            set;
        }

        public bool CanReach(TargetCell target, Spell spell, out Cell castCell)
        {
            bool nearFirst = true;
            var targetPoint = new MapPoint(target.Cell);
            var spellRange = Fighter.GetSpellRange(spell.CurrentSpellLevel);
            var minSpellRange = spell.CurrentSpellLevel.MinRange;
            var dist = targetPoint.ManhattanDistanceTo(Fighter.Position.Point);
            var diff = spellRange - dist;

            if (diff >= 0 && dist >= minSpellRange &&
                (target.Direction == DirectionFlagEnum.ALL_DIRECTIONS || target.Direction == DirectionFlagEnum.NONE ||
                (Fighter.Position.Point.OrientationTo(targetPoint).GetFlag() & target.Direction) != 0) &&
                Fighter.IsInCastZone(spell.CurrentSpellLevel, Fighter.Position.Point, target.Cell))
            {
                castCell = Fighter.Cell;
                return true;
            }

            // reachable
            if (-diff <= Fighter.MP)
            {
                castCell = m_environment.GetCellToCastSpell(target, spell, spell.CurrentSpellLevel.CastTestLos, nearFirst);
                return castCell != null;
            }


            castCell = null;
            return false;
        }

        public TargetCell[] ExpandCellsZone(Cell[] cells, Spell spell)
        {
            return cells.SelectMany(x => spell.CurrentSpellLevel.Effects.SelectMany(y => ExpandZone(x, y.ZoneShape, (int)y.ZoneMinSize, (int)y.ZoneSize))).Distinct().ToArray();
        }

        private IEnumerable<TargetCell> ExpandZone(Cell center, SpellShapeEnum shape, int minRange, int maxRange)
        {
            switch (shape)
            {
                case SpellShapeEnum.X:
                    return new CrossSet(new MapPoint(center), maxRange, minRange).
                        EnumerateValidPoints().Select(x => new TargetCell(Fighter.Map.Cells[x.CellId]));
                case SpellShapeEnum.C:
                    return new LozengeSet(new MapPoint(center), maxRange, minRange).
                        EnumerateValidPoints().Select(x => new TargetCell(Fighter.Map.Cells[x.CellId]));
                case SpellShapeEnum.L:
                    return new LineSet(new MapPoint(center).GetCellInDirection(DirectionsEnum.DIRECTION_NORTH_EAST, minRange), maxRange, DirectionsEnum.DIRECTION_NORTH_EAST).
                        EnumerateValidPoints().Select(x => new TargetCell(Fighter.Map.Cells[x.CellId], DirectionFlagEnum.DIRECTION_SOUTH_WEST)).
                        Union(new LineSet(new MapPoint(center).GetCellInDirection(DirectionsEnum.DIRECTION_SOUTH_EAST, minRange),
                            maxRange, DirectionsEnum.DIRECTION_SOUTH_EAST).EnumerateValidPoints().
                        Select(x => new TargetCell(Fighter.Map.Cells[x.CellId], DirectionFlagEnum.DIRECTION_NORTH_WEST))).
                        Union(new LineSet(new MapPoint(center).GetCellInDirection(DirectionsEnum.DIRECTION_SOUTH_WEST, minRange),
                            maxRange, DirectionsEnum.DIRECTION_SOUTH_WEST).EnumerateValidPoints().
                        Select(x => new TargetCell(Fighter.Map.Cells[x.CellId], DirectionFlagEnum.DIRECTION_NORTH_EAST))).
                        Union(new LineSet(new MapPoint(center).GetCellInDirection(DirectionsEnum.DIRECTION_NORTH_WEST, minRange),
                            maxRange, DirectionsEnum.DIRECTION_NORTH_WEST).EnumerateValidPoints().
                        Select(x => new TargetCell(Fighter.Map.Cells[x.CellId], DirectionFlagEnum.DIRECTION_SOUTH_EAST)));
                case SpellShapeEnum.I:
                    return new Complement(new LozengeSet(center, maxRange, minRange), new AllPoints())
                        .EnumerateValidPoints().Select(x => new TargetCell(Fighter.Map.Cells[x.CellId]));
                default:
                    return new[] {new TargetCell(center)};
            }
        }

        public bool GetRangeAttack(out int min, out int max)
        {
            bool hasRangeAttack = false;
            min = 0;
            max = 0;
            foreach (var spell in Fighter.Spells.Values)
            {
                var category = SpellIdentifier.GetSpellCategories(spell);
                if ((category & SpellCategory.Damages) != 0)
                {
                    if (min < spell.CurrentSpellLevel.MinRange)
                        min = (int)spell.CurrentSpellLevel.MinRange;

                    if (spell.CurrentSpellLevel.Range > max)
                        max = (int)spell.CurrentSpellLevel.Range;

                    hasRangeAttack = true;
                }
            }

            return hasRangeAttack;
        }

        public event Action<AIFighter> AnalysePossibilitiesFinished;

        public void AnalysePossibilities()
        {
            Possibilities = new List<SpellCastImpact>();
            foreach (var spell in Fighter.Spells.Values)
            {
                var category = SpellIdentifier.GetSpellCategories(spell);
                var spellLevel = spell.CurrentSpellLevel;
                var cast = new SpellCastImpact(spell);

                if (Fighter.AP < spellLevel.ApCost)
                    continue;

                if (spellLevel.StatesForbidden.Any(Fighter.HasState))
                    continue;

                if (spellLevel.StatesRequired.Any(state => !Fighter.HasState(state)))
                    continue;

                if (!Fighter.SpellHistory.CanCastSpell(spell.CurrentSpellLevel))
                    continue;

                // summoning is the priority
                if ((category & SpellCategory.Summoning) != 0 &&
                    ((category & SpellCategory.Healing) == 0 || Fighter.Team.Fighters.Any(x => x.IsDead())) && // revive effect and an ally is dead
                    (Fighter.CanSummon() || (category & SpellCategory.Healing) != 0 && Fighter.Team.Fighters.Any(x => x.IsDead()))) // can summon or is revive spell
                {
                    var adjacentCell = Fighter.GetCastZoneSet(spell.CurrentSpellLevel, Fighter.Position.Point).EnumerateValidPoints().
                        OrderBy(x => x.ManhattanDistanceTo(Fighter.Position.Point)).
                        FirstOrDefault(x => m_environment.CellInformationProvider.IsCellWalkable(x.CellId));

                    if (adjacentCell == null)
                        continue;

                    cast.IsSummoningSpell = true;
                    cast.SummonCell = Fighter.Map.Cells[adjacentCell.CellId];
                }
                else
                {
                    var cells =
                        ExpandCellsZone(Fighter.Fight.Fighters.Where(fighter => fighter.IsAlive() && fighter.IsVisibleFor(Fighter))
                               .Select(x => x.Cell).ToArray(), spell);

                    foreach (var target in cells)
                    {
                        if (!CanReach(target, spell, out var cell))
                            continue;
                        
                        if (Fighter.CanCastSpell(new SpellCastInformations(Fighter, spell, target.Cell) {CastCell = cell}) != SpellCastResult.OK)
                            continue;

                        var impact = ComputeSpellImpact(spell, target.Cell, cell);

                        if (impact == null)
                            continue;

                        impact.CastCell = cell;
                        impact.Target = target;

                        if (impact.Damage < 0)
                            continue; // hurts more allies than boost them

                        cast.Impacts.Add(impact);
                    }
                }

                if (cast.Impacts.Count > 0 || cast.IsSummoningSpell)
                    Possibilities.Add(cast);
            }

            AnalysePossibilitiesFinished?.Invoke(Fighter);
        }

        public AISpellCastPossibility FindFirstSpellCast()
        {
            var casts = new List<AISpellCastPossibility>();
            var minUsedAP = 0;
            var minUsedPM = 0;
            foreach (var priority in Priorities.OrderByDescending(x => x.Value))
            {
                // find best spell
                var impactComparer = new SpellImpactComparer(this, priority.Key);
                foreach (var possibleCast in Possibilities.OrderByDescending(x => x, new SpellCastComparer(this, priority.Key)))
                {
                    var category = SpellIdentifier.GetSpellCategories(possibleCast.Spell);

                    var dummy = possibleCast;
                    if (( category & priority.Key ) == 0 || casts.Any(x => x.Spell == dummy.Spell)) // spell already used
                        continue;

                    if (Fighter.AP - minUsedAP < possibleCast.Spell.CurrentSpellLevel.ApCost)
                        continue;

                    if (possibleCast.IsSummoningSpell)
                    {
                        var target = new SpellTarget() {Target = new TargetCell(possibleCast.SummonCell), CastCell = Fighter.Cell, AffectedCells = new []{possibleCast.SummonCell}};
                        casts.Add(new AISpellCastPossibility(possibleCast.Spell, target));
                        minUsedAP += (int)possibleCast.Spell.CurrentSpellLevel.ApCost;
                        continue;

                    }

                    // find best target
                    foreach(var impact in possibleCast.Impacts.OrderByDescending(x => x, impactComparer))
                    {
                        if (impactComparer.GetScore(impact) <= 0)
                            continue;

                        Cell castSpell = impact.CastCell;

                        var cast = new AISpellCastPossibility(possibleCast.Spell, impact);
                        if (castSpell == Fighter.Cell)
                        {
                            casts.Add(cast);
                            minUsedAP += (int)possibleCast.Spell.CurrentSpellLevel.ApCost;
                            continue;
                        }

                        var pathfinder = new Pathfinder(m_environment.CellInformationProvider);
                        var path = pathfinder.FindPath(Fighter.Position.Cell.Id, castSpell.Id, false);

                        if (path.IsEmpty() || path.MPCost > Fighter.MP)
                            continue;

                        cast.MoveBefore = path;

                        casts.Add(cast);
                        minUsedAP += (int)possibleCast.Spell.CurrentSpellLevel.ApCost;
                        minUsedPM += path.MPCost;
                        break;
                    }
                }
            }

            if (casts.Count > 1)
            {
                // check if the second spell can be casted before
                var max = MaxConsecutiveSpellCast(casts[0].Spell, Fighter.AP);
                if (casts[1].Spell.CurrentSpellLevel.ApCost <= Fighter.AP - max*casts[0].Spell.CurrentSpellLevel.ApCost &&
                    casts[0].MoveBefore != null)
                {
                    if (casts[1].MoveBefore == null)
                        return casts[1];

                    var pathfinder = new Pathfinder(m_environment.CellInformationProvider);
                    var path = pathfinder.FindPath(casts[1].MoveBefore.EndCell.Id, casts[0].MoveBefore != null ? casts[0].MoveBefore.EndCell.Id : Fighter.Cell.Id, false);

                    if (!path.IsEmpty() && path.MPCost + casts[1].MPCost <= Fighter.MP)
                        return casts[1];
                }
            }

            return casts.FirstOrDefault();
        }

        public int MaxConsecutiveSpellCast(Spell spell, int ap)
        {
            // can be casted indefinitly 
            if (spell.CurrentSpellLevel.ApCost == 0 && spell.CurrentSpellLevel.MinCastInterval == 0)
                return 0; 

            if (spell.CurrentSpellLevel.GlobalCooldown > 0)
                return 1;

            var max = (int)(ap/spell.CurrentSpellLevel.ApCost);
            var category = SpellIdentifier.GetSpellCategories(spell);

            if ((category & SpellCategory.Summoning) != 0)
            {
                return 1;
            }

            if (spell.CurrentSpellLevel.MaxCastPerTarget > 0 &&
                max > spell.CurrentSpellLevel.MaxCastPerTarget)
                max = (int)spell.CurrentSpellLevel.MaxCastPerTarget;

            if (spell.CurrentSpellLevel.MaxCastPerTurn > 0 &&
                max > spell.CurrentSpellLevel.MaxCastPerTurn)
                max = (int)spell.CurrentSpellLevel.MaxCastPerTurn;

            return max;
        }

        public SpellTarget ComputeSpellImpact(Spell spell, Cell targetCell, Cell castCell)
        {
            SpellTarget damages = null;
            var cast = SpellManager.Instance.GetSpellCastHandler(Fighter, spell, targetCell, false);
            cast.CastCell = castCell;
            if (!cast.Initialize())
                return null;
            
            foreach (var handler in cast.GetEffectHandlers())
            {
                if (!handler.CanApply())
                    return null;
                       
                handler.CastCell = castCell;
                foreach (var target in handler.GetAffectedActors())
                {
                    if (target != Fighter || handler.AffectedCells.Contains(castCell)) // we take in account the movement of the caster before the spell cast
                        CumulEffects(handler, ref damages, target, spell);
                }
            }

            if (damages != null)
                damages.AffectedCells = cast.GetEffectHandlers().SelectMany(x => x.AffectedCells).Distinct().ToArray();

            return damages;
        }

        // todo : do something more general (ghost actors)
        private void CumulEffects(SpellEffectHandler handler, ref SpellTarget spellImpact, FightActor target, Spell spell)
        {
            var effect = handler.Dice;
            var isFriend = Fighter.Team.Id == target.Team.Id;
            var result = new SpellTarget();

             var category = SpellIdentifier.GetEffectCategories(effect.EffectId);

            if (category == 0)
                return;

            if (Fighter is SummonedTurret)
            {
                isFriend = category == SpellCategory.Healing;
            }

            var chanceToHappen = 1.0; // 

            // When chances to happen is under 100%, then we reduce spellImpact accordingly, for simplicity, but after having apply damage bonus & reduction. 
            // So average damage should remain exact even if Min and Max are not. 
            if (effect.Random > 0)
                chanceToHappen = effect.Random / 100.0;

            if ((target is SummonedFighter))
                chanceToHappen /= 2; // It's much better to hit non-summoned foes => effect on summons (except allies summon for Osa) is divided by 2. 

            uint min;
            uint max;

            if (handler is DamagePerHPLost)
            {
                min = max = (uint) Math.Round(((Fighter.Stats.Health.DamageTaken*effect.DiceNum)/100d));
            }
            else if (handler is Kill && target != Fighter) // doesn't take sacrifice in account
            {
                min = max = (uint)target.LifePoints;
            }
            else
            {
                min = (uint) Math.Min(effect.DiceNum, effect.DiceFace);
                max = (uint) Math.Max(effect.DiceNum, effect.DiceFace);
            }

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
                minDamage *= -0.3; // High penalty for firing on friends
                maxDamage *= -0.3; // High penalty for firing on friends
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

    public class SpellCastComparer : IComparer<SpellCastImpact>
    {
        private readonly Dictionary<SpellCategory, Func<SpellCastImpact, SpellCastImpact, int>> m_comparers;
            

        private readonly SpellSelector m_spellSelector;

        public SpellCastComparer(SpellSelector spellSelector, SpellCategory category)
        {
            Category = category;
            m_spellSelector = spellSelector;
            m_comparers = new Dictionary<SpellCategory, Func<SpellCastImpact, SpellCastImpact, int>>()
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
        public int Compare(SpellCastImpact cast1, SpellCastImpact cast2)
        {
            return m_comparers[Category](cast1, cast2);
        }

        public int CompareSummon(SpellCastImpact cast1, SpellCastImpact cast2)
        {
            return cast1.IsSummoningSpell.CompareTo(cast2.IsSummoningSpell);
        }

        public int CompareBoost(SpellCastImpact cast1, SpellCastImpact cast2)
        {
            if (cast1.Impacts.Count == 0 || cast2.Impacts.Count == 0)
                return cast1.Impacts.Count.CompareTo(cast2.Impacts.Count);

            var max1 = cast1.Impacts.Max(x => x.Boost);
            var max2 = cast2.Impacts.Max(x => x.Boost);
            var efficiency1 = GetEfficiency(cast1);
            var efficiency2 = GetEfficiency(cast2);

            return ( max1 * efficiency1 ).CompareTo(max2 * efficiency2);
        }

        public int CompareDamage(SpellCastImpact cast1, SpellCastImpact cast2)
        {
            if (cast1.Impacts.Count == 0 || cast2.Impacts.Count == 0)
                return cast1.Impacts.Count.CompareTo(cast2.Impacts.Count);

            var max1 = cast1.Impacts.Max(x => x.Damage);
            var max2 = cast2.Impacts.Max(x => x.Damage);
            var efficiency1 = GetEfficiency(cast1);
            var efficiency2 = GetEfficiency(cast2);

            return ( max1 * efficiency1 ).CompareTo(max2 * efficiency2);
        }

        public int CompareHeal(SpellCastImpact cast1, SpellCastImpact cast2)
        {
            if (cast1.Impacts.Count == 0 || cast2.Impacts.Count == 0)
                return cast1.Impacts.Count.CompareTo(cast2.Impacts.Count);

            var max1 = cast1.Impacts.Max(x => x.Heal);
            var max2 = cast2.Impacts.Max(x => x.Heal);
            var efficiency1 = GetEfficiency(cast1);
            var efficiency2 = GetEfficiency(cast2);

            return ( max1 * efficiency1 ).CompareTo(max2 * efficiency2);
        }

        public int CompareCurse(SpellCastImpact cast1, SpellCastImpact cast2)
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
        public int GetEfficiency(SpellCastImpact cast)
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

            var scoreComparaison = m_comparers[Category](cast1, cast2);
            if (scoreComparaison != 0)
                return scoreComparaison;
            
            // if scores are the same we choose the nearest reachable cell
            // note : inverse comparaison (smaller better not bigger better)
            return new MapPoint(cast2.CastCell).ManhattanDistanceTo(m_spellSelector.Fighter.Position.Point).CompareTo(
                new MapPoint(cast1.CastCell).ManhattanDistanceTo(m_spellSelector.Fighter.Position.Point));
        }

        public double GetScore(SpellTarget cast)
        {
            return cast.Boost + cast.Damage + cast.Heal + cast.Curse;
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
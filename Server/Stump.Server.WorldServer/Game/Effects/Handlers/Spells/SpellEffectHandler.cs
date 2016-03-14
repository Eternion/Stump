using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells
{
    public abstract class SpellEffectHandler : EffectHandler
    {
        FightActor[] m_customAffectedActors;
        List<Cell> m_affectedCells;
        MapPoint m_castPoint;
        Zone m_effectZone;
        Cell m_customCastCell;

        protected SpellEffectHandler(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect)
        {
            Dice = effect;
            Caster = caster;
            CastHandler = castHandler;
            TargetedCell = targetedCell;
            TargetedPoint = new MapPoint(TargetedCell);
            Critical = critical;
            Targets = effect.Targets;
            Category = SpellIdentifier.GetEffectCategories(effect.EffectId);
        }

        public EffectDice Dice
        {
            get;
        }

        public SpellCategory Category
        {
            get;
        }

        public FightActor Caster
        {
            get;
        }

        public SpellCastHandler CastHandler
        {
            get;
        }

        public Spell Spell => CastHandler.Spell;

        public Cell TargetedCell
        {
            get;
            protected set;
        }

        public MapPoint TargetedPoint
        {
            get;
            protected set;
        }

        public bool Critical
        {
            get;
        }

        public MarkTrigger MarkTrigger
        {
            get;
            set;
        }
        
        public virtual int Priority => Effect.Priority;

        public Cell CastCell
        {
            get { return m_customCastCell ?? (MarkTrigger != null ? MarkTrigger.Shape.Cell : Caster.Cell); }
            set { m_customCastCell = value; }
        }

        public MapPoint CastPoint
        {
            get { return m_castPoint ?? (m_castPoint = new MapPoint(CastCell)); }
            set { m_castPoint = value; }
        }
        
        public Zone EffectZone
        {
            get
            {
                return m_effectZone ??
                       (m_effectZone =
                        new Zone(Effect.ZoneShape, (byte) Effect.ZoneSize, (byte) Effect.ZoneMinSize, CastPoint.OrientationTo(TargetedPoint), Effect.ZoneEfficiencyPercent, Effect.ZoneMaxEfficiency));
            }
            set
            {
                m_effectZone = value;

                RefreshZone();
            }
        }

        public TargetCriterion[] Targets
        {
            get;
            set;
        }

        public List<Cell> AffectedCells
        {
            get { return m_affectedCells ?? (m_affectedCells = EffectZone.GetCells(TargetedCell, Map).ToList()); }
            private set { m_affectedCells = value; }
        }

        public IFight Fight => Caster.Fight;

        public Map Map => Fight.Map;

        public bool IsValidTarget(FightActor actor)
        {
            if (actor.GetBuffs(x => x is SpellImmunityBuff && CastHandler.IsCastedBySpell(((SpellImmunityBuff)x).SpellImmune)).Any())
                return false;

            var lookup = Targets.ToLookup(x => x.GetType());

            return lookup.All(x => x.First().IsDisjonction ?
                x.Any(y => y.IsTargetValid(actor, this)) : x.All(y => y.IsTargetValid(actor, this)));
        }

        public void RefreshZone()
        {
            AffectedCells = EffectZone.GetCells(TargetedCell, Map).ToList();
        }

        public IEnumerable<FightActor> GetAffectedActors()
        {
            if (m_customAffectedActors != null)
                return m_customAffectedActors;

            if (Targets.Any(x => x is TargetTypeCriterion && ((TargetTypeCriterion)x).TargetType == SpellTargetType.SELF_ONLY) && !AffectedCells.Contains(Caster.Cell))
                AffectedCells.Add(Caster.Cell);

            return Fight.GetAllFighters(AffectedCells).Where(entry => !entry.IsDead() && !entry.IsCarried() && IsValidTarget(entry)).ToArray();
        }

        public IEnumerable<FightActor> GetAffectedActors(Predicate<FightActor> predicate)
        {
            if (m_customAffectedActors != null)
                return m_customAffectedActors;

            return GetAffectedActors().Where(entry => predicate(entry)).ToArray();
        }
        
        public EffectInteger GenerateEffect()
        {
            var effect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (effect != null)
                effect.Value = (short)(effect.Value*Efficiency);

            return effect;
        }

        public void SetAffectedActors(IEnumerable<FightActor> actors)
        {
            m_customAffectedActors = actors.ToArray();
        }

        public void AddAffectedActor(FightActor actor)
        {
            var tmpActors = new List<FightActor>();
            if (m_customAffectedActors != null)
                tmpActors = m_customAffectedActors.ToList();

            tmpActors.Add(actor);
            m_customAffectedActors = tmpActors.ToArray();
        }

        public bool IsBuff() => Effect.Duration != 0;

        public bool IsTriggerBuff() => Effect.Duration != 0 && Effect.Triggers != "I";

        public Buff AddStatBuff(FightActor target, short value, PlayerFields caracteritic, FightDispellableEnum dispelable,
            short? customActionId = null)
        {
            if (IsTriggerBuff())
                return AddTriggerBuff(target, dispelable, (buff, triggerrer, type, token) => AddStatBuffInternal(target, value, caracteritic, dispelable, customActionId, triggerrer, true));
            
            return AddStatBuffInternal(target, value, caracteritic, dispelable, customActionId);
        }

        Buff AddStatBuffInternal(FightActor target, short value, PlayerFields caracteritic, FightDispellableEnum dispelable,
                                    short? customActionId = null, FightActor caster = null, bool noDelay = false)
        {
            var id = target.PopNextBuffId();
            var buff = new StatBuff(id, target, caster ?? Caster, Effect, Spell, value, caracteritic, Critical, dispelable, Priority, customActionId);

            if (noDelay)
                buff.Delay = 0;

            target.AddBuff(buff);

            return buff;
        }

        public Buff AddTriggerBuff(FightActor target, FightDispellableEnum dispelable, TriggerBuffApplyHandler applyTrigger)
        {
            var id = target.PopNextBuffId();
            var buff = new TriggerBuff(id, target, Caster, Dice, Spell, Spell, Critical, dispelable, Priority, applyTrigger);

            target.AddBuff(buff);

            return buff;
        }

        public TriggerBuff AddTriggerBuff(FightActor target, FightDispellableEnum dispelable, BuffTriggerType type, TriggerBuffApplyHandler applyTrigger)
        {
            var id = target.PopNextBuffId();
            var buff = new TriggerBuff(id, target, Caster, Dice, Spell, Spell, Critical, dispelable, Priority, applyTrigger);
            buff.SetTrigger(type);

            target.AddBuff(buff);

            return buff;
        }

        public TriggerBuff AddTriggerBuff(FightActor target, FightDispellableEnum dispelable, TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger)
        {
            var id = target.PopNextBuffId();
            var buff = new TriggerBuff(id, target, Caster, Dice, Spell, Spell, Critical, dispelable, Priority, applyTrigger, removeTrigger);

            target.AddBuff(buff);

            return buff;
        }

        public Buff AddStateBuff(FightActor target, FightDispellableEnum dispelable, bool bypassMaxStack, SpellState state)
        {
            if (IsTriggerBuff())
                return AddTriggerBuff(target, dispelable, (buff, triggerrer, type, token) => AddStateBuffInternal(target, dispelable, bypassMaxStack, state, triggerrer));

            return AddStateBuffInternal(target, dispelable, bypassMaxStack, state);

        }

        Buff AddStateBuffInternal(FightActor target, FightDispellableEnum dispelable, bool bypassMaxStack, SpellState state, FightActor caster = null)
        {
            var id = target.PopNextBuffId();
            var buff = new StateBuff(id, target, caster ?? Caster, Dice, Spell, dispelable, state);

            target.AddBuff(buff, bypassMaxStack);

            return buff;
        }

        public static bool RemoveStateBuff(FightActor target, SpellStatesEnum stateId)
        {
            foreach (var state in target.GetBuffs(x => x is StateBuff && ((StateBuff)x).State.Id == (int)stateId).ToArray())
            {
                target.RemoveBuff(state);
            }

            return true;
        }

        public virtual bool RequireSilentCast() => false;
    }
}
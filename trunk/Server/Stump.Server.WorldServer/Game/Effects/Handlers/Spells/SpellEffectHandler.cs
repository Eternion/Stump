using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
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
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells
{
    public abstract class SpellEffectHandler : EffectHandler
    {
        private FightActor[] m_customAffectedActors;
        private Cell[] m_affectedCells;
        private MapPoint m_castPoint;
        private Zone m_effectZone;

        protected SpellEffectHandler(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect)
        {
            Dice = effect;
            Caster = caster;
            Spell = spell;
            TargetedCell = targetedCell;
            TargetedPoint = new MapPoint(TargetedCell);
            Critical = critical;
        }

        public EffectDice Dice
        {
            get;
            private set;
        }

        public FightActor Caster
        {
            get;
            private set;
        }

        public Spell Spell
        {
            get;
            private set;
        }

        public Cell TargetedCell
        {
            get;
            private set;
        }

        public MapPoint TargetedPoint
        {
            get;
            private set;
        }

        public bool Critical
        {
            get;
            private set;
        }

        public MarkTrigger MarkTrigger
        {
            get;
            set;
        }

        public Cell CastCell
        {
            get { return MarkTrigger != null && MarkTrigger.Shapes.Length > 0 ? MarkTrigger.Shapes[0].Cell : Caster.Cell; }
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
                        new Zone(Effect.ZoneShape, Effect.ZoneSize, CastPoint.OrientationTo(TargetedPoint)));
            }
            set
            {
                m_effectZone = value;

                RefreshZone();
            }
        }

        public Cell[] AffectedCells
        {
            get { return m_affectedCells ?? (m_affectedCells = EffectZone.GetCells(TargetedCell, Map)); }
            private set { m_affectedCells = value; }
        }

        public Fight Fight
        {
            get { return Caster.Fight; }
        }

        public Map Map
        {
            get { return Fight.Map; }
        }

        public bool IsValidTarget(FightActor actor)
        {
            if (Effect.Targets == SpellTargetType.NONE)
                // return false; note : wtf, why is there spells with Targets = NONE ?
                return true;

            if (Effect.Targets == SpellTargetType.ALL)
                return true;

            if (Caster == actor && Effect.Targets.HasFlag(SpellTargetType.SELF))
                return true;

            if (Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF) && actor != Caster)
                return false;

            if (Caster.IsFriendlyWith(actor) && Caster != actor)
            {
                if ((Effect.Targets.HasFlag(SpellTargetType.ALLY_1) ||
                    Effect.Targets.HasFlag(SpellTargetType.ALLY_2) ||
                    Effect.Targets.HasFlag(SpellTargetType.ALLY_3) ||
                    Effect.Targets.HasFlag(SpellTargetType.ALLY_4) ||
                    Effect.Targets.HasFlag(SpellTargetType.ALLY_5)) && !(actor is SummonedFighter))
                    return true;

                if ((Effect.Targets.HasFlag(SpellTargetType.ALLY_SUMMONS) ||
                    Effect.Targets.HasFlag(SpellTargetType.ALLY_STATIC_SUMMONS)) && actor is SummonedFighter)
                    return true;
            }

            if (Caster.IsEnnemyWith(actor))
            {
                if ((Effect.Targets.HasFlag(SpellTargetType.ENNEMY_1) ||
                    Effect.Targets.HasFlag(SpellTargetType.ENNEMY_2) ||
                    Effect.Targets.HasFlag(SpellTargetType.ENNEMY_3) ||
                    Effect.Targets.HasFlag(SpellTargetType.ENNEMY_4) ||
                    Effect.Targets.HasFlag(SpellTargetType.ENNEMY_5)) && !(actor is SummonedFighter))
                    return true;

                if ((Effect.Targets.HasFlag(SpellTargetType.ENNEMY_SUMMONS) ||
                    Effect.Targets.HasFlag(SpellTargetType.ENNEMY_STATIC_SUMMONS)) && actor is SummonedFighter)
                    return true;
            }

            return false;
        }

        public void RefreshZone()
        {
            AffectedCells = EffectZone.GetCells(TargetedCell, Map);
        }

        public IEnumerable<FightActor> GetAffectedActors()
        {
            if (m_customAffectedActors != null)
                return m_customAffectedActors;

            if (Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF))
                return new[] {Caster};

            return Fight.GetAllFighters(AffectedCells).Where(entry => !entry.IsDead() && IsValidTarget(entry)).ToArray();
        }

        public IEnumerable<FightActor> GetAffectedActors(Predicate<FightActor> predicate)
        {
            if (m_customAffectedActors != null)
                return m_customAffectedActors;

            if (Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF) && predicate(Caster))
                return new[] {Caster};

            if (Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF))
                return new FightActor[0];


            return GetAffectedActors().Where(entry => predicate(entry)).ToArray();
        }

        public void SetAffectedActors(IEnumerable<FightActor> actors)
        {
            m_customAffectedActors = actors.ToArray();
        }

        public StatBuff AddStatBuff(FightActor target, short value, PlayerFields caracteritic, bool dispelable)
        {
            int id = target.PopNextBuffId();
            var buff = new StatBuff(id, target, Caster, Effect, Spell, value, caracteritic, Critical, dispelable);

            target.AddAndApplyBuff(buff);

            return buff;
        }

        public StatBuff AddStatBuff(FightActor target, short value, PlayerFields caracteritic, bool dispelable,
                                    short customActionId)
        {
            int id = target.PopNextBuffId();
            var buff = new StatBuff(id, target, Caster, Effect, Spell, value, caracteritic, Critical, dispelable,
                                    customActionId);

            target.AddAndApplyBuff(buff);

            return buff;
        }

        public TriggerBuff AddTriggerBuff(FightActor target, bool dispelable, BuffTriggerType trigger,
                                          TriggerBuffApplyHandler applyTrigger)
        {
            int id = target.PopNextBuffId();
            var buff = new TriggerBuff(id, target, Caster, Dice, Spell, Critical, dispelable, trigger, applyTrigger);

            target.AddAndApplyBuff(buff);

            return buff;
        }

        public TriggerBuff AddTriggerBuff(FightActor target, bool dispelable, BuffTriggerType trigger,
                                          TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger)
        {
            int id = target.PopNextBuffId();
            var buff = new TriggerBuff(id, target, Caster, Dice, Spell, Critical, dispelable, trigger, applyTrigger,
                                       removeTrigger);

            target.AddAndApplyBuff(buff);

            return buff;
        }

        public StateBuff AddStateBuff(FightActor target, bool dispelable, SpellState state)
        {
            int id = target.PopNextBuffId();
            var buff = new StateBuff(id, target, Caster, Dice, Spell, dispelable, state);

            target.AddAndApplyBuff(buff);

            return buff;
        }

        public virtual bool RequireSilentCast()
        {
            return false;
        }
    }
}
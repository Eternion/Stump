using System;
using System.Collections.Generic;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Effects;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public abstract class FightActor : ContextActor, IStatsOwner
    {
        #region Events

        public event Action<FightActor, bool> ReadyStateChanged;

        private void NotifyReadyStateChanged(bool isReady)
        {
            Action<FightActor, bool> handler = ReadyStateChanged;
            if (handler != null)
                handler(this, isReady);
        }

        public event Action<FightActor, bool> TurnReadyStateChanged;

        private void NotifyTurnReadyStateChanged(bool isReady)
        {
            Action<FightActor, bool> handler = TurnReadyStateChanged;
            if (handler != null)
                handler(this, isReady);
        }

        public event Action<FightActor, Cell> CellShown;

        private void NotifyCellShown(Cell cell)
        {
            Action<FightActor, Cell> handler = CellShown;
            if (handler != null)
                CellShown(this, cell);
        }

        public event Action<FightActor, int, FightActor> LifePointsChanged;

        private void NotifyLifePointsChanged(int delta, FightActor from)
        {
            Action<FightActor, int, FightActor> handler = LifePointsChanged;

            if (handler != null)
                handler(this, delta, from);
        }

        public event Action<FightActor> FighterLeft;

        internal void NotifyFightLeft()
        {
            Action<FightActor> handler = FighterLeft;
            if (handler != null)
                handler(this);
        }

        public event Action<FightActor, ObjectPosition> PrePlacementChanged;

        private void NotifyPrePlacementChanged(ObjectPosition position)
        {
            Action<FightActor, ObjectPosition> handler = PrePlacementChanged;
            if (handler != null)
                handler(this, position);
        }

        public event Action<FightActor> TurnPassed;

        private void NotifyTurnPassed()
        {
            Action<FightActor> handler = TurnPassed;
            if (handler != null)
                handler(this);
        }

        public delegate void SpellCastingHandler(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical);
        public event SpellCastingHandler SpellCasting;

        private void NotifySpellCasting(Spell spell, Cell target, FightSpellCastCriticalEnum critical)
        {
            SpellCastingHandler handler = SpellCasting;
            if (handler != null)
                handler(this, spell, target, critical);
        }

        public event SpellCastingHandler SpellCasted;

        private void NotifySpellCasted(Spell spell, Cell target, FightSpellCastCriticalEnum critical)
        {
            SpellCastingHandler handler = SpellCasted;
            if (handler != null)
                handler(this, spell, target, critical);
        }

        public event Action<FightActor, FightActor> Dead;

        private void NotifyDead(FightActor killedBy)
        {
            Action<FightActor, FightActor> handler = Dead;
            if (handler != null)
                handler(this, killedBy);
        }

        public delegate void FightPointsVariationHandler(FightActor actor, ActionsEnum action, FightActor source, FightActor target, short delta);
        public event FightPointsVariationHandler FightPointsVariation;

        private void NotifyFightPointsVariation(ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            OnFightPointsVariation(action, source, target, delta);

            FightPointsVariationHandler handler = FightPointsVariation;
            if (handler != null)
                handler(this, action, source, target, delta);
        }

        protected virtual void OnFightPointsVariation(ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            switch (action)
            {
                case ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE:
                    NotifyApUsed((short) (-delta));
                    break;
                case ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE:
                    NotifyMpUsed((short) (-delta));
                    break;
            }
        }

        public event Action<FightActor, short> ApUsed;

        private void NotifyApUsed(short amount)
        {
            Action<FightActor, short> handler = ApUsed;
            if (handler != null)
                handler(this, amount);
        }

        public event Action<FightActor, short> MpUsed;

        private void NotifyMpUsed(short amount)
        {
            Action<FightActor, short> handler = MpUsed;
            if (handler != null)
                handler(this, amount);
        }

        public event Action<FightActor, SequenceTypeEnum> SequenceStarted;

        private void NotifySequenceStarted(SequenceTypeEnum sequenceType)
        {
            Action<FightActor, SequenceTypeEnum> handler = SequenceStarted;
            if (handler != null)
                handler(this, sequenceType);
        }

        public event Action<FightActor, SequenceTypeEnum, FightSequenceAction> SequenceEnded;

        private void NotifySequenceEnded(SequenceTypeEnum sequenceType, FightSequenceAction actionEnd)
        {
            Action<FightActor, SequenceTypeEnum, FightSequenceAction> handler = SequenceEnded;
            if (handler != null)
                handler(this, sequenceType, actionEnd);
        }

        public event Action<FightActor, SequenceTypeEnum> SequenceActionEnded;

        private void NotifySequenceActionEnded(SequenceTypeEnum sequenceType)
        {
            Action<FightActor, SequenceTypeEnum> handler = SequenceActionEnded;
            if (handler != null)
                handler(this, sequenceType);
        }

        #endregion

        #region Constructor

        protected FightActor(FightTeam team)
        {
            Team = team;
        }

        #endregion

        #region Properties

        public Fights.Fight Fight
        {
            get { return Team.Fight; }
        }

        public FightTeam Team
        {
            get;
            private set;
        }

        public override IContext Context
        {
            get { return Fight; }
        }

        public abstract ObjectPosition MapPosition
        {
            get;
        }

        public FightActor CarriedActor
        {
            get;
            protected set;
        }

        public SequenceTypeEnum Sequence
        {
            get;
            private set;
        }

        public FightSequenceAction LastSequenceAction
        {
            get;
            set;
        }

        public bool IsSequencing
        {
            get;
            private set;
        }

        public bool IsReady
        {
            get;
            private set;
        }

        public bool IsTurnReady
        {
            get;
            internal set;
        }

        #region Stats

        public int LifePoints
        {
            get { return Stats[CaracteristicsEnum.Health].Total; }
        }

        public int MaxLifePoints
        {
            get { return ((StatsHealth) Stats[CaracteristicsEnum.Health]).TotalMax; }
        }

        public short DamageTaken
        {
            get { return Stats[CaracteristicsEnum.Health].Context; }
            set { Stats[CaracteristicsEnum.Health].Context = value; }
        }

        public int AP
        {
            get { return Stats[CaracteristicsEnum.AP].Total; }
        }

        public short UsedAP
        {
            get;
            private set;
        }

        public int MP
        {
            get { return Stats[CaracteristicsEnum.MP].Total; }
        }

        public short UsedMP
        {
            get;
            private set;
        }

        public abstract StatsFields Stats
        {
            get;
        }

        #endregion

        #endregion

        #region Actions

        #region Pre-Fight

        public void ToggleReady(bool ready)
        {
            IsReady = ready;

            NotifyReadyStateChanged(ready);
        }

        public void ChangePrePlacement(Cell cell)
        {
            if (!Fight.CanChangePosition(this, cell))
                return;

            Position.Cell = cell;

            NotifyPrePlacementChanged(Position);
        }

        #endregion

        #region Turn

        public void ToggleTurnReady(bool ready)
        {
            IsTurnReady = ready;

            NotifyTurnReadyStateChanged(ready);
        }

        public void PassTurn()
        {
            Fight.RequestTurnEnd(this);

            NotifyTurnPassed();
        }

        #endregion

        #region Fighting

        public void ShowCell(Cell cell)
        {
            NotifyCellShown(cell);
        }

        public bool UseAP(short amount)
        {
            if (Stats[CaracteristicsEnum.AP].Total - amount < 0)
                return false;

            Stats[CaracteristicsEnum.AP].Context -= amount;
            UsedAP += amount;

            NotifyFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, this, this, (short) (-amount));

            return true;
        }

        public bool UseMP(short amount)
        {
            if (Stats[CaracteristicsEnum.MP].Total - amount < 0)
                return false;

            Stats[CaracteristicsEnum.MP].Context -= amount;
            UsedMP += amount;

            NotifyFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE, this, this, (short) (-amount));

            return true;
        }

        public void ResetPoints()
        {
            Stats[CaracteristicsEnum.AP].Context += UsedAP;
            UsedAP = 0;

            Stats[CaracteristicsEnum.MP].Context += UsedMP;
            UsedMP = 0;
        }

        public abstract bool CanCastSpell(Spell spell, Cell cell);

        public virtual void CastSpell(Spell spell, Cell cell)
        {
            var spellLevel = spell.CurrentSpellLevel;

            if (!CanCastSpell(spell, cell))
                return;

            var random = new AsyncRandom();

            StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            var critical = FightSpellCastCriticalEnum.NORMAL;

            if (random.Next((int) spellLevel.CriticalFailureProbability) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;

            else if (random.Next((int)spellLevel.CriticalHitProbability) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            NotifySpellCasting(spell, cell, critical);

            UseAP((short) spellLevel.ApCost);

            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
            {
                if (spellLevel.CriticalFailureEndsTurn)
                    PassTurn();

                return;
            }

            var effects = critical == FightSpellCastCriticalEnum.CRITICAL_HIT ? spellLevel.CritialEffects : spellLevel.Effects;

            foreach (var effect in effects)
            {
                if (effect.Random > 0)
                {
                    if (random.NextDouble() > effect.Random / 100d)
                    {
                        // effect ignored
                        continue;
                    }
                }

                var handler = EffectManager.Instance.GetSpellEffectHandler(effect, this, spell, cell);

                handler.Apply();
            }

            NotifySpellCasted(spell, cell, critical);
        }

        public void Die()
        {
            InflictDirectDamage((short) LifePoints);
        }

        public short InflictDirectDamage(short damage, FightActor from)
        {
            if (LifePoints - damage < 0)
                damage = (short) LifePoints;

            DamageTaken += damage;

            NotifyLifePointsChanged(-damage, from);

            if (IsDead())
                NotifyDead(from);

            return damage;
        }

        public short InflictDirectDamage(short damage)
        {
            if (LifePoints - damage < 0)
                damage = (short) LifePoints;

            DamageTaken += damage;

            NotifyLifePointsChanged(-damage, null);

            if (IsDead())
                NotifyDead(null);

            return damage;
        }

        public short InflictDamage(short damage, EffectSchoolEnum school, bool pvp = false)
        {
            damage = CalculateDamage(damage, school);
            damage = CalculateDamageResistance(damage, school, pvp);

            return InflictDirectDamage(damage);
        }

        public short InflictDamage(short damage, EffectSchoolEnum school, FightActor from, bool pvp = false)
        {
            damage = 100;

            damage = CalculateDamage(damage, school);
            damage = CalculateDamageResistance(damage, school, pvp);

            return InflictDirectDamage(damage, from);
        }

        public short HealDirect(short healPoints)
        {
            if (LifePoints + healPoints > MaxLifePoints)
                healPoints = (short) (MaxLifePoints - LifePoints);

            DamageTaken -= healPoints;

            NotifyLifePointsChanged(healPoints, null);

            return healPoints;
        }

        public short Heal(short healPoints)
        {
            return HealDirect(CalculateHeal(healPoints));
        }

        #region Formulas

        public short CalculateDamage(short damage, EffectSchoolEnum type)
        {
            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    return (short) (damage*
                                    (100 + Stats[CaracteristicsEnum.Strength] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total*100)/100d +
                                    (Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.PhysicalDamage].Total));
                case EffectSchoolEnum.Earth:
                    return (short) (damage*
                                    (100 + Stats[CaracteristicsEnum.Strength] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total*100)/100d +
                                    (Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.PhysicalDamage].Total));
                case EffectSchoolEnum.Air:
                    return (short) (damage*
                                    (100 + Stats[CaracteristicsEnum.Agility] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total*100)/100d +
                                    (Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.MagicDamage].Total));
                case EffectSchoolEnum.Water:
                    return (short) (damage*
                                    (100 + Stats[CaracteristicsEnum.Chance] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total*100)/100d +
                                    (Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.MagicDamage].Total));
                case EffectSchoolEnum.Fire:
                    return (short) (damage*
                                    (100 + Stats[CaracteristicsEnum.Intelligence] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total*100)/100d +
                                    (Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.MagicDamage].Total));
                default:
                    return damage;
            }
        }

        public short CalculateDamageResistance(short damage, EffectSchoolEnum type, bool pvp)
        {
            double percentResistance = 0;
            double fixResistance = 0;

            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    percentResistance = Stats[CaracteristicsEnum.NeutralResistPercent].Total + ( pvp ? Stats[CaracteristicsEnum.PvpNeutralResistPercent].Total : 0 );
                    fixResistance = Stats[CaracteristicsEnum.NeutralElementReduction].Total + ( pvp ? Stats[CaracteristicsEnum.PvpNeutralElementReduction].Total : 0 ) + Stats[CaracteristicsEnum.PhysicalDamageReduction];
                    break;
                case EffectSchoolEnum.Earth:
                    percentResistance = Stats[CaracteristicsEnum.EarthResistPercent].Total + ( pvp ? Stats[CaracteristicsEnum.PvpEarthResistPercent].Total : 0 );
                    fixResistance = Stats[CaracteristicsEnum.EarthElementReduction].Total + ( pvp ? Stats[CaracteristicsEnum.PvpEarthElementReduction].Total : 0 ) + Stats[CaracteristicsEnum.PhysicalDamageReduction];
                    break;
                case EffectSchoolEnum.Air:
                    percentResistance = Stats[CaracteristicsEnum.AirResistPercent].Total + ( pvp ? Stats[CaracteristicsEnum.PvpAirResistPercent].Total : 0 );
                    fixResistance = Stats[CaracteristicsEnum.AirElementReduction].Total + ( pvp ? Stats[CaracteristicsEnum.PvpAirElementReduction].Total : 0 ) + Stats[CaracteristicsEnum.MagicDamageReduction];
                    break;
                case EffectSchoolEnum.Water:
                    percentResistance = Stats[CaracteristicsEnum.WaterResistPercent].Total + ( pvp ? Stats[CaracteristicsEnum.PvpWaterResistPercent].Total : 0 );
                    fixResistance = Stats[CaracteristicsEnum.WaterElementReduction].Total + ( pvp ? Stats[CaracteristicsEnum.PvpWaterElementReduction].Total : 0 ) + Stats[CaracteristicsEnum.MagicDamageReduction];
                    break;
                case EffectSchoolEnum.Fire:
                    percentResistance = Stats[CaracteristicsEnum.FireResistPercent].Total + ( pvp ? Stats[CaracteristicsEnum.PvpFireResistPercent].Total : 0 );
                    fixResistance = Stats[CaracteristicsEnum.FireElementReduction].Total + ( pvp ? Stats[CaracteristicsEnum.PvpFireElementReduction].Total : 0 ) + Stats[CaracteristicsEnum.MagicDamageReduction];
                    break;
                default:
                    return 0;
            }

            return (short) (( 1 - percentResistance / 100d ) * ( damage - fixResistance ));
        }

        public short CalculateHeal(int heal)
        {
            return (short) (heal * ( 100 + Stats[CaracteristicsEnum.Intelligence].Total ) / 100d + Stats[CaracteristicsEnum.HealBonus].Total);
        }

        public short CalculateArmorValue(int reduction, EffectSchoolEnum type)
        {
            CaracteristicsEnum schoolCaracteristic;
            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    schoolCaracteristic = CaracteristicsEnum.Strength;
                    break;
                case EffectSchoolEnum.Earth:
                    schoolCaracteristic = CaracteristicsEnum.Strength;
                    break;
                case EffectSchoolEnum.Air:
                    schoolCaracteristic = CaracteristicsEnum.Agility;
                    break;
                case EffectSchoolEnum.Water:
                    schoolCaracteristic = CaracteristicsEnum.Chance;
                    break;
                case EffectSchoolEnum.Fire:
                    schoolCaracteristic = CaracteristicsEnum.Intelligence;
                    break;
                default:
                    return 0;
            }

            return (short)( reduction *
                            Math.Max(1 + Stats[schoolCaracteristic].Total / 100d,
                                     1 + ( Stats[CaracteristicsEnum.Intelligence].Total / 200d ) + ( Stats[schoolCaracteristic].Total / 200d )));
        }

        public short CalculateWeaponDamage(short damage, EffectSchoolEnum school)
        {
            return damage;
        }

        public short CalculateWeaponHeal(short heal)
        {
            return heal;
        }

        public double CalculateCriticRate(double baseRate)
        {
            const double multipleOfE = Math.E * 1.1;

            return Math.Floor(baseRate * multipleOfE / Math.Log(Stats[CaracteristicsEnum.Agility].Total + 12, Math.E));
        }

        public short CalculateReflectedDamageBonus(short spellBonus)
        {
            return (short)( spellBonus * ( 1 + ( Stats[CaracteristicsEnum.Wisdom].Total / 100d ) ) + Stats[CaracteristicsEnum.DamageReflection].Total );
        }

        #endregion

        #endregion

        #region Sequences

        public bool StartSequence(SequenceTypeEnum sequenceType)
        {
            if (IsSequencing)
                return false;

            IsSequencing = true;
            Sequence = sequenceType;
            LastSequenceAction = FindSequenceEndAction(Sequence);

            NotifySequenceStarted(sequenceType);

            return true;
        }

        public bool EndSequence()
        {
            if (!IsSequencing)
                return false;

            IsSequencing = false;

            NotifySequenceEnded(Sequence, LastSequenceAction);

            return true;
        }

        public void EndSequenceAction()
        {
            switch (Sequence)
            {
                case SequenceTypeEnum.SEQUENCE_MOVE:
                    StopMove();
                    break;
            }

            NotifySequenceActionEnded(Sequence);
        }

        private static FightSequenceAction FindSequenceEndAction(SequenceTypeEnum sequenceTypeEnum)
        {
            switch (sequenceTypeEnum)
            {
                case SequenceTypeEnum.SEQUENCE_MOVE:
                    return FightSequenceAction.Move;
                case SequenceTypeEnum.SEQUENCE_SPELL:
                    return FightSequenceAction.Spell;
                case SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH:
                    return FightSequenceAction.Death;
                default:
                    return FightSequenceAction.None;
            }
        }


        #endregion

        #region Conditions

        public bool IsAlive()
        {
            return Stats[CaracteristicsEnum.Health].Total > 0;
        }

        public bool IsDead()
        {
            return !IsAlive();
        }

        public bool IsTeamLeader()
        {
            return Team.Leader == this;
        }

        public bool IsFighterTurn()
        {
            return Fight.TimeLine.Current == this;
        }

        public bool IsFriendlyWith(FightActor actor)
        {
            return actor.Team == Team;
        }

        public bool IsEnnemyWith(FightActor actor)
        {
            return !IsFriendlyWith(actor);
        }

        public override bool CanMove()
        {
            return base.CanMove() && IsFighterTurn();
        }

        public virtual bool CanPlay()
        {
            return IsAlive();
        }

        #endregion

        #endregion

        #region Network

        public override EntityDispositionInformations GetEntityDispositionInformations()
        {
            if (CarriedActor != null)
                return new FightEntityDispositionInformations(Position.Cell.Id, (sbyte) Position.Direction, CarriedActor.Id);

            return base.GetEntityDispositionInformations();
        }

        public virtual GameFightMinimalStats GetGameFightMinimalStats()
        {
            return new GameFightMinimalStats(
                Stats[CaracteristicsEnum.Health].Total,
                ((StatsHealth) Stats[CaracteristicsEnum.Health]).TotalMax,
                Stats[CaracteristicsEnum.Health].Base,
                Stats[CaracteristicsEnum.PermanentDamagePercent].Total,
                0, // shieldsPoints = ?
                (short) Stats[CaracteristicsEnum.AP].Total,
                (short) Stats[CaracteristicsEnum.MP].Total,
                Stats[CaracteristicsEnum.SummonLimit].Total,
                (short) Stats[CaracteristicsEnum.NeutralResistPercent].Total,
                (short) Stats[CaracteristicsEnum.EarthResistPercent].Total,
                (short) Stats[CaracteristicsEnum.WaterResistPercent].Total,
                (short) Stats[CaracteristicsEnum.AirResistPercent].Total,
                (short) Stats[CaracteristicsEnum.FireResistPercent].Total,
                (short) Stats[CaracteristicsEnum.DodgeAPProbability].Total,
                (short) Stats[CaracteristicsEnum.DodgeMPProbability].Total,
                (short) Stats[CaracteristicsEnum.TackleBlock].Total,
                (short) Stats[CaracteristicsEnum.TackleEvade].Total,
                (int) GameActionFightInvisibilityStateEnum.VISIBLE // invisibility state
                );
        }

        public virtual FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberInformations(Id);
        }

        public virtual GameFightFighterInformations GetGameFightFighterInformations()
        {
            return new GameFightFighterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(),
                Team.Id,
                IsAlive(),
                GetGameFightMinimalStats());
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return GetGameFightFighterInformations();
        }

        #endregion
    }
}
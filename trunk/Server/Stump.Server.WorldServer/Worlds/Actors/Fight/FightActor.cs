using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Effects;
using Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Fights.Buffs;
using Stump.Server.WorldServer.Worlds.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Worlds.Fights.Results;
using Stump.Server.WorldServer.Worlds.Items;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Spells;
using FightLoot = Stump.Server.WorldServer.Worlds.Fights.FightLoot;

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

        public event Action<FightActor, FightActor, int> DamageReducted;

        private void NotifyDamageReducted(FightActor source, int reduction)
        {
            Action<FightActor, FightActor, int> handler = DamageReducted;
            if (handler != null)
                handler(this, source, reduction);
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

        public delegate void SpellCastingHandler(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast);
        public event SpellCastingHandler SpellCasting;

        private void NotifySpellCasting(Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            SpellCastingHandler handler = SpellCasting;
            if (handler != null)
                handler(this, spell, target, critical, silentCast);
        }

        public event SpellCastingHandler SpellCasted;

        private void NotifySpellCasted(Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            SpellCastingHandler handler = SpellCasted;
            if (handler != null)
                handler(this, spell, target, critical, silentCast);
        }

        public event Action<FightActor, Buff> BuffAdded;

        private void NotifyBuffAdded(Buff buff)
        {
            Action<FightActor, Buff> handler = BuffAdded;
            if (handler != null) handler(this, buff);
        }

        public event Action<FightActor, Buff> BuffRemoved;

        private void NotifyBuffRemoved(Buff buff)
        {
            Action<FightActor, Buff> handler = BuffRemoved;
            if (handler != null) handler(this, buff);
        }

        public event Action<FightActor, FightActor> Dead;

        private void NotifyDead(FightActor killedBy)
        {
            OnDead(killedBy);

            Action<FightActor, FightActor> handler = Dead;
            if (handler != null)
                handler(this, killedBy);
        }

        protected virtual void OnDead(FightActor killedBy)
        {
            RemoveAndDispellAllBuffs();
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
                    NotifyApUsed((short)( -delta ));
                    break;
                case ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE:
                    NotifyMpUsed((short)( -delta ));
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
            OpposedTeam = Fight.BlueTeam == Team ? Fight.RedTeam : Fight.BlueTeam;
            Loot = new FightLoot();
        }

        #endregion

        #region Properties


        public Fights.Fight Fight
        {
            get
            {
                return Team.Fight;
            }
        }

        public FightTeam Team
        {
            get;
            private set;
        }

        public FightTeam OpposedTeam
        {
            get;
            private set;
        }

        public override IContext Context
        {
            get
            {
                return Fight;
            }
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

        public bool IsReady
        {
            get;
            protected set;
        }

        public bool IsTurnReady
        {
            get;
            internal set;
        }

        #region Stats

        public abstract byte Level
        {
            get;
        }

        public int LifePoints
        {
            get
            {
                return Stats.Health.Total;
            }
        }

        public int MaxLifePoints
        {
            get
            {
                return Stats.Health.TotalMax;
            }
        }

        public short DamageTaken
        {
            get
            {
                return Stats.Health.Context;
            }
            set
            {
                Stats.Health.Context = value;
            }
        }

        public int AP
        {
            get
            {
                return Stats[CaracteristicsEnum.AP].Total;
            }
        }

        public short UsedAP
        {
            get;
            private set;
        }

        public int MP
        {
            get
            {
                return Stats[CaracteristicsEnum.MP].Total;
            }
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

        public FightLoot Loot
        {
            get;
            private set;
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

        public virtual ObjectPosition GetLeaderBladePosition()
        {
            return MapPosition.Clone();
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

            NotifyFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, this, this, (short)( -amount ));

            return true;
        }

        public bool UseMP(short amount)
        {
            if (Stats[CaracteristicsEnum.MP].Total - amount < 0)
                return false;

            Stats[CaracteristicsEnum.MP].Context -= amount;
            UsedMP += amount;

            NotifyFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE, this, this, (short)( -amount ));

            return true;
        }

        public bool LostAP(short amount)
        {
            if (Stats[CaracteristicsEnum.AP].Total - amount < 0)
                return false;

            Stats[CaracteristicsEnum.AP].Context -= amount;
            UsedAP += amount;

            NotifyFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_LOST, this, this, (short)( -amount ));

            return true;
        }

        public bool LostMP(short amount)
        {
            if (Stats[CaracteristicsEnum.MP].Total - amount < 0)
                return false;

            Stats[CaracteristicsEnum.MP].Context -= amount;
            UsedMP += amount;

            NotifyFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST, this, this, (short)( -amount ));

            return true;
        }

        public bool RegainAP(short amount)
        {
            Stats[CaracteristicsEnum.AP].Context += amount;
            UsedAP -= amount;

            NotifyFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_WIN, this, this, (short)( amount ));

            return true;
        }

        public bool RegainMP(short amount)
        {
            Stats[CaracteristicsEnum.MP].Context += amount;
            UsedMP -= amount;

            NotifyFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_WIN, this, this, (short)( amount ));

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
            if (!IsFighterTurn())
                return;

            var spellLevel = spell.CurrentSpellLevel;

            if (!CanCastSpell(spell, cell))
                return;

            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            var random = new AsyncRandom();
            var critical = RollCriticalDice(spellLevel);

            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
            {
                NotifySpellCasting(spell, cell, critical, false);
                UseAP((short)spellLevel.ApCost);

                if (spellLevel.CriticalFailureEndsTurn)
                    PassTurn();

                return;
            }

            var effects = critical == FightSpellCastCriticalEnum.CRITICAL_HIT ? spellLevel.CritialEffects : spellLevel.Effects;
            var handlers = new List<SpellEffectHandler>();
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

                var handler = EffectManager.Instance.GetSpellEffectHandler(effect, this, spell, cell, critical == FightSpellCastCriticalEnum.CRITICAL_HIT);
                handlers.Add(handler);
            }

            var silentCast = handlers.Any(entry => entry.RequireSilentCast());

            NotifySpellCasting(spell, cell, critical, silentCast);
            UseAP((short)spellLevel.ApCost);

            foreach (var handler in handlers)
                handler.Apply();

            NotifySpellCasted(spell, cell, critical, silentCast);
        }

        public FightSpellCastCriticalEnum RollCriticalDice(SpellLevelTemplate spell)
        {
            var random = new AsyncRandom();

            var critical = FightSpellCastCriticalEnum.NORMAL;

            if (spell.CriticalHitProbability != 0 && random.Next((int)spell.CriticalFailureProbability) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;

            else if (spell.CriticalHitProbability != 0 && random.Next((int)spell.CriticalHitProbability) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            return critical;
        }

        public int GetReflectedSpellLevel()
        {
            if (m_buffList.OfType<SpellReflectionBuff>().Count() == 0)
                return 0;

            return m_buffList.OfType<SpellReflectionBuff>().Max(entry => entry.ReflectedLevel);
        }

        public void Die()
        {
            DamageTaken += (short)LifePoints;

            NotifyDead(null);
        }

        public short InflictDirectDamage(short damage, FightActor from)
        {
            if (LifePoints - damage < 0)
                damage = (short)LifePoints;

            DamageTaken += damage;

            TriggerBuffs(TriggerType.BEFORE_ATTACKED);

            NotifyLifePointsChanged(-damage, from);

            if (IsDead())
                NotifyDead(from);

            TriggerBuffs(TriggerType.AFTER_ATTACKED);

            return damage;
        }

        public short InflictDirectDamage(short damage)
        {
            if (LifePoints - damage < 0)
                damage = (short)LifePoints;

            DamageTaken += damage;

            TriggerBuffs(TriggerType.BEFORE_ATTACKED);

            NotifyLifePointsChanged(-damage, null);

            if (IsDead())
                NotifyDead(null);

            TriggerBuffs(TriggerType.AFTER_ATTACKED);


            return damage;
        }

        public short InflictDamage(short damage, EffectSchoolEnum school, bool pvp = false)
        {
            damage = CalculateDamage(damage, school);
            damage = CalculateDamageResistance(damage, school, pvp);

            short reduction = CalculateArmorReduction(school);

            if (reduction > 0)
                NotifyDamageReducted(this, reduction);

            damage -= reduction;

            if (damage <= 0)
                return 0;

            return InflictDirectDamage(damage);
        }

        public short InflictDamage(short damage, EffectSchoolEnum school, FightActor from, bool pvp = false)
        {
            damage = CalculateDamage(damage, school);
            damage = CalculateDamageResistance(damage, school, pvp);

            short reduction = CalculateArmorReduction(school);

            if (reduction > 0)
                NotifyDamageReducted(from, reduction);

            damage -= reduction;

            if (damage <= 0)
                return 0;

            return InflictDirectDamage(damage, from);
        }

        public short HealDirect(short healPoints)
        {
            if (LifePoints + healPoints > MaxLifePoints)
                healPoints = (short)( MaxLifePoints - LifePoints );

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
                    return (short)( damage *
                                    ( 100 + Stats[CaracteristicsEnum.Strength] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total * 100 ) / 100d +
                                    ( Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.PhysicalDamage].Total ) );
                case EffectSchoolEnum.Earth:
                    return (short)( damage *
                                    ( 100 + Stats[CaracteristicsEnum.Strength] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total * 100 ) / 100d +
                                    ( Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.PhysicalDamage].Total ) );
                case EffectSchoolEnum.Air:
                    return (short)( damage *
                                    ( 100 + Stats[CaracteristicsEnum.Agility] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total * 100 ) / 100d +
                                    ( Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.MagicDamage].Total ) );
                case EffectSchoolEnum.Water:
                    return (short)( damage *
                                    ( 100 + Stats[CaracteristicsEnum.Chance] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total * 100 ) / 100d +
                                    ( Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.MagicDamage].Total ) );
                case EffectSchoolEnum.Fire:
                    return (short)( damage *
                                    ( 100 + Stats[CaracteristicsEnum.Intelligence] + Stats[CaracteristicsEnum.DamageBonusPercent] + Stats[CaracteristicsEnum.DamageMultiplicator].Total * 100 ) / 100d +
                                    ( Stats[CaracteristicsEnum.DamageBonus].Total + Stats[CaracteristicsEnum.MagicDamage].Total ) );
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

            return (short)( ( 1 - percentResistance / 100d ) * ( damage - fixResistance ) );
        }

        public short CalculateHeal(int heal)
        {
            return (short)( heal * ( 100 + Stats[CaracteristicsEnum.Intelligence].Total ) / 100d + Stats[CaracteristicsEnum.HealBonus].Total );
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
                    return (short) (reduction * (1 + Stats[CaracteristicsEnum.Intelligence].Total / 200d));
            }

            return (short)( reduction *
                            Math.Max(1 + Stats[schoolCaracteristic].Total / 100d,
                                     1 + ( Stats[CaracteristicsEnum.Intelligence].Total / 200d ) + ( Stats[schoolCaracteristic].Total / 200d )) );
        }

        public short CalculateArmorReduction(EffectSchoolEnum damageType)
        {
            int specificArmor = 0;
            switch (damageType)
            {
                case EffectSchoolEnum.Neutral:
                    specificArmor = Stats[CaracteristicsEnum.NeutralDamageArmor].Total;
                    break;
                case EffectSchoolEnum.Earth:
                    specificArmor = Stats[CaracteristicsEnum.EarthDamageArmor].Total;
                    break;
                case EffectSchoolEnum.Air:
                    specificArmor = Stats[CaracteristicsEnum.AirDamageArmor].Total;
                    break;
                case EffectSchoolEnum.Water:
                    specificArmor = Stats[CaracteristicsEnum.WaterDamageArmor].Total;
                    break;
                case EffectSchoolEnum.Fire:
                    specificArmor = Stats[CaracteristicsEnum.FireDamageArmor].Total;
                    break;
            }

            return (short) (specificArmor + Stats[CaracteristicsEnum.GlobalDamageReduction].Total);
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

        #region Buffs
        private readonly UniqueIdProvider m_buffIdProvider = new UniqueIdProvider();
        private readonly List<Buff> m_buffList = new List<Buff>();

        public int PopNextBuffId()
        {
            return m_buffIdProvider.Pop();
        }

        public void FreeBuffId(int id)
        {
            m_buffIdProvider.Push(id);
        }

        public void AddAndApplyBuff(Buff buff)
        {
            AddBuff(buff);

            if (!( buff is TriggerBuff ) ||
                ( ( buff as TriggerBuff ).Trigger & TriggerType.BUFF_ADDED ) == TriggerType.BUFF_ADDED)
                buff.Apply();
        }

        public void AddBuff(Buff buff)
        {
            m_buffList.Add(buff);

            NotifyBuffAdded(buff);
        }

        public void RemoveAndDispellBuff(Buff buff)
        {
            RemoveBuff(buff);

            buff.Remove();

            FreeBuffId(buff.Id);
        }

        public void RemoveBuff(Buff buff)
        {
            m_buffList.Remove(buff);

            NotifyBuffRemoved(buff);

            FreeBuffId(buff.Id);
        }

        public void RemoveAndDispellAllBuffs()
        {
            var copyOfBuffs = m_buffList.ToArray();

            foreach (var buff in copyOfBuffs)
            {
                buff.Remove();
                m_buffList.Remove(buff);

                NotifyBuffRemoved(buff);
            }
        }

        public void TriggerBuffs(TriggerType trigger)
        {
            foreach (var buff in m_buffList)
            {
                var triggerBuff = buff as TriggerBuff;

                if (triggerBuff == null)
                    continue;

                if (( triggerBuff.Trigger & trigger ) == trigger)
                {
                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED);
                    triggerBuff.Apply(trigger);
                    Fight.EndSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED);
                }
            }
        }

        public void DecrementBuffsDuration(FightActor caster)
        {
            var buffsToRemove = new List<Buff>();

            foreach (var buff in m_buffList)
            {
                if (buff.Caster == caster)
                    if (buff.DecrementDuration())
                        buffsToRemove.Add(buff);
            }

            foreach (var buff in buffsToRemove)
            {
                RemoveAndDispellBuff(buff);
            }
        }

        /// <summary>
        /// Decrement the duration of all the buffs that the fighter casted.
        /// </summary>
        public void DecrementAllCastedBuffsDuration()
        {
            foreach (var fighter in Fight.GetAllFighters())
            {
                fighter.DecrementBuffsDuration(this);
            }
        }

        #endregion

        #region End Fight

        public virtual IEnumerable<DroppedItem> RollLoot(FightActor fighter)
        {
            return new DroppedItem[0];
        }

        public virtual uint GetDroppedKamas()
        {
            return 0;
        }

        public virtual IFightResult GetFightResult()
        {
            if (Fight.State != FightState.Ended)
                throw new Exception("Fight not ended, cannot generate the fight result");

            return new FightResult(this, GetFighterOutcome(), Loot);
        }

        protected FightOutcomeEnum GetFighterOutcome()
        {
            var teamDead = Team.AreAllDead();
            var opposedTeamDead = OpposedTeam.AreAllDead();

            if (!teamDead && opposedTeamDead)
                return FightOutcomeEnum.RESULT_VICTORY;

            if (teamDead && !opposedTeamDead)
                return FightOutcomeEnum.RESULT_LOST;

            return FightOutcomeEnum.RESULT_DRAW;
        }

        #endregion

        #region Conditions

        public bool IsAlive()
        {
            return Stats.Health.Total > 0;
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
            return IsFighterTurn();
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
                return new FightEntityDispositionInformations(Position.Cell.Id, (sbyte)Position.Direction, CarriedActor.Id);

            return base.GetEntityDispositionInformations();
        }

        public virtual GameFightMinimalStats GetGameFightMinimalStats()
        {
            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.Base,
                Stats[CaracteristicsEnum.PermanentDamagePercent].Total,
                0, // shieldsPoints = ?
                (short)Stats[CaracteristicsEnum.AP].Total,
                (short)Stats[CaracteristicsEnum.MP].Total,
                0,
                false,
                (short)Stats[CaracteristicsEnum.NeutralResistPercent].Total,
                (short)Stats[CaracteristicsEnum.EarthResistPercent].Total,
                (short)Stats[CaracteristicsEnum.WaterResistPercent].Total,
                (short)Stats[CaracteristicsEnum.AirResistPercent].Total,
                (short)Stats[CaracteristicsEnum.FireResistPercent].Total,
                (short)Stats[CaracteristicsEnum.DodgeAPProbability].Total,
                (short)Stats[CaracteristicsEnum.DodgeMPProbability].Total,
                (short)Stats[CaracteristicsEnum.TackleBlock].Total,
                (short)Stats[CaracteristicsEnum.TackleEvade].Total,
                (int)GameActionFightInvisibilityStateEnum.VISIBLE // invisibility state
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
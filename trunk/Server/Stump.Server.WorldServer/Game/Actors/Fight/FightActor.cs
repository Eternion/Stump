using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Stump.Core.Pool;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Fights.History;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Context;
using FightLoot = Stump.Server.WorldServer.Game.Fights.FightLoot;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using SpellState = Stump.Server.WorldServer.Database.Spells.SpellState;
using VisibleStateEnum = Stump.DofusProtocol.Enums.GameActionFightInvisibilityStateEnum;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public abstract class FightActor : ContextActor, IStatsOwner
    {
        #region Events

        public event Action<FightActor, bool> ReadyStateChanged;

        protected virtual void OnReadyStateChanged(bool isReady)
        {
            Action<FightActor, bool> handler = ReadyStateChanged;
            if (handler != null)
                handler(this, isReady);
        }

        public event Action<FightActor, Cell, bool> CellShown;

        protected virtual void OnCellShown(Cell cell, bool team)
        {
            Action<FightActor, Cell, bool> handler = CellShown;
            if (handler != null)
                CellShown(this, cell, team);
        }

        public event Action<FightActor, int, int, FightActor> LifePointsChanged;

        protected virtual void OnLifePointsChanged(int delta, int permanentDamages, FightActor from)
        {
            Action<FightActor, int, int, FightActor> handler = LifePointsChanged;

            if (handler != null)
                handler(this, delta, permanentDamages, from);
        }

        public event Action<FightActor, FightActor, int> DamageReducted;

        protected virtual void OnDamageReducted(FightActor source, int reduction)
        {
            Action<FightActor, FightActor, int> handler = DamageReducted;
            if (handler != null)
                handler(this, source, reduction);
        }

        public event Action<FightActor, FightActor, int> DamageReflected;

        protected internal virtual void OnDamageReflected(FightActor target, int reflected)
        {
            ActionsHandler.SendGameActionFightReflectDamagesMessage(Fight.Clients, this, target, reflected);

            Action<FightActor, FightActor, int> handler = DamageReflected;
            if (handler != null)
                handler(this, target, reflected);
        }

        public event Action<FightActor> FighterLeft;

        public event Action<FightActor, ObjectPosition> PrePlacementChanged;

        protected virtual void OnPrePlacementChanged(ObjectPosition position)
        {
            Action<FightActor, ObjectPosition> handler = PrePlacementChanged;
            if (handler != null)
                handler(this, position);
        }

        public event Action<FightActor> TurnPassed;

        protected virtual void OnTurnPassed()
        {
            Action<FightActor> handler = TurnPassed;
            if (handler != null)
                handler(this);
        }

        public delegate void SpellCastingHandler(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast);

        public event SpellCastingHandler SpellCasting;

        protected virtual void OnSpellCasting(Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            SpellCastingHandler handler = SpellCasting;
            if (handler != null)
                handler(this, spell, target, critical, silentCast);
        }

        public event SpellCastingHandler SpellCasted;

        protected virtual void OnSpellCasted(Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (!spell.CurrentSpellLevel.Effects.Any(effect => effect.EffectId == EffectsEnum.Effect_Invisibility) &&
                VisibleState == GameActionFightInvisibilityStateEnum.INVISIBLE)
            {
                ShowCell(Cell, false);

                if (!IsInvisibleSpellCast(spell))
                    if (!DispellInvisibilityBuff())
                        SetInvisibilityState(VisibleStateEnum.VISIBLE);
            }

            SpellHistory.RegisterCastedSpell(spell.CurrentSpellLevel, Fight.GetOneFighter(target));

            SpellCastingHandler handler = SpellCasted;
            if (handler != null)
                handler(this, spell, target, critical, silentCast);
        }

        public event Action<FightActor, WeaponTemplate, Cell, FightSpellCastCriticalEnum, bool > WeaponUsed;

        protected virtual void OnWeaponUsed(WeaponTemplate weapon, Cell cell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (VisibleState == GameActionFightInvisibilityStateEnum.INVISIBLE)
            {
                ShowCell(Cell, false);

                if (!DispellInvisibilityBuff())
                     SetInvisibilityState(VisibleStateEnum.VISIBLE);
            }

            Action<FightActor, WeaponTemplate, Cell, FightSpellCastCriticalEnum, bool> handler = WeaponUsed;
            if (handler != null) handler(this, weapon, cell, critical, silentCast);
        }

        public event Action<FightActor, Buff> BuffAdded;

        protected virtual void OnBuffAdded(Buff buff)
        {
            Action<FightActor, Buff> handler = BuffAdded;
            if (handler != null) handler(this, buff);
        }

        public event Action<FightActor, Buff> BuffRemoved;

        protected virtual void OnBuffRemoved(Buff buff)
        {
            Action<FightActor, Buff> handler = BuffRemoved;
            if (handler != null) handler(this, buff);
        }

        public event Action<FightActor, FightActor> Dead;

        protected virtual void OnDead(FightActor killedBy)
        {
            RemoveAndDispellAllBuffs();

            Action<FightActor, FightActor> handler = Dead;
            if (handler != null)
                handler(this, killedBy);
        }

        public delegate void FightPointsVariationHandler(FightActor actor, ActionsEnum action, FightActor source, FightActor target, short delta);

        public event FightPointsVariationHandler FightPointsVariation;

        protected virtual void OnFightPointsVariation(ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            switch (action)
            {
                case ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE:
                    OnApUsed((short)( -delta ));
                    break;
                case ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE:
                    OnMpUsed((short)( -delta ));
                    break;
            }

            FightPointsVariationHandler handler = FightPointsVariation;
            if (handler != null)
                handler(this, action, source, target, delta);
        }


        public event Action<FightActor, short> ApUsed;

        protected virtual void OnApUsed(short amount)
        {
            Action<FightActor, short> handler = ApUsed;
            if (handler != null)
                handler(this, amount);
        }

        public event Action<FightActor, short> MpUsed;

        protected virtual void OnMpUsed(short amount)
        {
            Action<FightActor, short> handler = MpUsed;
            if (handler != null)
                handler(this, amount);
        }

        #endregion

        #region Constructor

        protected FightActor(FightTeam team)
        {
            Team = team;
            OpposedTeam = Fight.BlueTeam == Team ? Fight.RedTeam : Fight.BlueTeam;
            VisibleState = VisibleStateEnum.VISIBLE;
            Loot = new FightLoot();
            SpellHistory = new SpellHistory(this);
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

        public FightTeam OpposedTeam
        {
            get;
            private set;
        }

        public override ICharacterContainer CharacterContainer
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

        public virtual bool IsReady
        {
            get;
            protected set;
        }

        public SpellHistory SpellHistory
        {
            get;
            private set;
        }

        public ObjectPosition TurnStartPosition
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
            get { return Stats.Health.Total; }
        }

        public int MaxLifePoints
        {
            get
            {
                return Stats.Health.TotalMax;
            }
        }

        public int PermanentDamages
        {
            get;
            set;
        }

        public int DamageTaken
        {
            get { return Stats.Health.DamageTaken; }
            set { Stats.Health.DamageTaken = value; }
        }

        public int AP
        {
            get { return Stats.AP.Total; }
        }

        public short UsedAP
        {
            get { return Stats.AP.Used; }
        }

        public int MP
        {
            get { return Stats.MP.Total; }
        }

        public short UsedMP
        {
            get { return Stats.MP.Used; }
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

        public abstract Spell GetSpell(int id);
        public abstract bool HasSpell(int id);

        #endregion

        #endregion

        #region Actions

        #region Pre-Fight

        public void ToggleReady(bool ready)
        {
            IsReady = ready;

            OnReadyStateChanged(ready);
        }

        public void ChangePrePlacement(Cell cell)
        {
            if (!Fight.CanChangePosition(this, cell))
                return;

            Position.Cell = cell;

            OnPrePlacementChanged(Position);
        }

        public virtual ObjectPosition GetLeaderBladePosition()
        {
            return MapPosition.Clone();
        }

        #endregion

        #region Turn

        public void PassTurn()
        {
            Fight.StopTurn();

            OnTurnPassed();
        }

        #endregion

        #region Leave

        public void LeaveFight()
        {
            if (HasLeft())
                return;

            m_left = true;

            OnLeft();
        }

        protected virtual void OnLeft()
        {
            var evnt = FighterLeft;
            if (evnt != null)
                evnt(this);
        }

        #endregion

        #region Fighting

        public override bool StartMove(Maps.Pathfinding.Path movementPath)
        {
            return base.StartMove(movementPath);
        }

        public void ShowCell(Cell cell, bool team = true)
        {
            if (team)
            {
                foreach (var fighter in Team.GetAllFighters<CharacterFighter>())
                {
                    ContextHandler.SendShowCellMessage(fighter.Character.Client, this, cell);
                }
            }
            else
            {
                ContextHandler.SendShowCellMessage(Fight.Clients, this, cell);
            }

            OnCellShown(cell, team);
        }

        public virtual bool UseAP(short amount)
        {
            if (Stats[PlayerFields.AP].Total - amount < 0)
                return false;

            Stats.AP.Used += amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, this, this, (short)( -amount ));

            return true;
        }

        public virtual bool UseMP(short amount)
        {
            if (Stats[PlayerFields.MP].Total - amount < 0)
                return false;

            Stats.MP.Used += amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE, this, this, (short) (-amount));

            return true;
        }

        public virtual bool LostAP(short amount)
        {
            if (Stats[PlayerFields.AP].Total - amount < 0)
                return false;

            Stats.AP.Used += amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_LOST, this, this, (short)( -amount ));

            return true;
        }

        public virtual bool LostMP(short amount)
        {
            if (Stats[PlayerFields.MP].Total - amount < 0)
                return false;

            Stats.MP.Used += amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST, this, this, (short)( -amount ));

            return true;
        }

        public virtual bool RegainAP(short amount)
        {
            /*if (amount > Stats.AP.Used)
                amount = Stats.AP.Used;*/

            Stats.AP.Used -= amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_WIN, this, this, (short)( amount ));

            return true;
        }

        public virtual bool RegainMP(short amount)
        {
            /*if (amount > Stats.MP.Used)
                amount = Stats.MP.Used;*/

            Stats.MP.Used -= amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_WIN, this, this, (short)( amount ));

            return true;
        }

        public virtual void ResetUsedPoints()
        {
            Stats.AP.Used = 0;
            Stats.MP.Used = 0;
        }

        public virtual SpellCastResult CanCastSpell(Spell spell, Cell cell)
        {
            if (!IsFighterTurn() || IsDead())
            {
                return SpellCastResult.CANNOT_PLAY;
            }

            if (!HasSpell(spell.Id))
            {
                return SpellCastResult.HAS_NOT_SPELL;
            }

            var spellLevel = spell.CurrentSpellLevel;

            if (!cell.Walkable || cell.NonWalkableDuringFight)
            {
                return SpellCastResult.UNWALKABLE_CELL;
            }

            if (AP < spellLevel.ApCost)
            {
                return SpellCastResult.NOT_ENOUGH_AP;
            }

            var cellfree = Fight.IsCellFree(cell);
            if (( spellLevel.NeedFreeCell && !cellfree ) ||
                ( spellLevel.NeedTakenCell && cellfree ))
            {
                return SpellCastResult.CELL_NOT_FREE;
            }

            if (spellLevel.StatesForbidden.Any(HasState))
            {
                return SpellCastResult.STATE_FORBIDDEN;
            }

            if (spellLevel.StatesRequired.Any(state => !HasState(state)))
            {
                return SpellCastResult.STATE_REQUIRED;
            }

            var castZone = GetCastZone(spellLevel);

            if (!castZone.Contains(cell))
            {
                Debug.WriteLine("!castZone.Contains(cell)");
                return SpellCastResult.NOT_IN_ZONE;
            }

            if (!SpellHistory.CanCastSpell(spellLevel, cell))
            {
                Debug.WriteLine("!SpellHistory.CanCastSpell(spellLevel, cell)");
                return SpellCastResult.HISTORY_ERROR;
            }

            if (spell.CurrentSpellLevel.CastTestLos && !Fight.CanBeSeen(Cell, cell))
            {
                Debug.WriteLine("spell.CurrentSpellLevel.CastTestLos && !Fight.CanBeSeen(Cell, cell)");
                return SpellCastResult.NO_LOS;
            }

            return SpellCastResult.OK;
        }


        public virtual Cell[] GetCastZone(SpellLevelTemplate spellLevel)
        {
            var range = spellLevel.Range;
            IShape shape;

            if (spellLevel.RangeCanBeBoosted)
            {
                range += (uint)Stats[PlayerFields.Range].Total;

                if (range < spellLevel.MinRange)
                    range = spellLevel.MinRange;

                range = Math.Min(range, MapPoint.MapHeight * MapPoint.MapWidth);
            }

            if (spellLevel.CastInDiagonal && spellLevel.CastInLine)
            {
                shape = new Cross((byte)spellLevel.MinRange, (byte)range)
                {
                    AllDirections = true
                };
            }
            else if (spellLevel.CastInLine)
            {
                shape = new Cross((byte) spellLevel.MinRange, (byte) range);
            }
            else if (spellLevel.CastInDiagonal)
            {
                shape = new Cross((byte)spellLevel.MinRange, (byte)range)
                {
                    Diagonal = true
                };
            }
            else
            {
                shape = new Lozenge((byte)spellLevel.MinRange, (byte)range);
            }

            return shape.GetCells(Cell, Map);
        }
        public int GetSpellRange(SpellLevelTemplate spell)
        {
            return (int) (spell.Range + ( spell.RangeCanBeBoosted ? Stats[PlayerFields.Range].Total : 0 ));
        }

        public virtual bool CastSpell(Spell spell, Cell cell)
        {
            if (!IsFighterTurn() || IsDead())
                return false;

            var spellLevel = spell.CurrentSpellLevel;

            if (CanCastSpell(spell, cell) != SpellCastResult.OK)
                return false;

            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            var critical = RollCriticalDice(spellLevel);

            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
            {
                OnSpellCasting(spell, cell, critical, false);
                UseAP((short) spellLevel.ApCost);
                Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

                if (spellLevel.CriticalFailureEndsTurn)
                    PassTurn();

                return false;
            }

            var handler = SpellManager.Instance.GetSpellCastHandler(this, spell, cell, critical == FightSpellCastCriticalEnum.CRITICAL_HIT);
            handler.Initialize();

            OnSpellCasting(spell, cell, critical, handler.SilentCast);
            UseAP((short)spellLevel.ApCost);

            handler.Execute();

            OnSpellCasted(spell, cell, critical, handler.SilentCast);

            return true;
        }

        public SpellReflectionBuff GetBestReflectionBuff()
        {
            return m_buffList.OfType<SpellReflectionBuff>().
                OrderByDescending(entry => entry.ReflectedLevel).
                FirstOrDefault();
        }

        public void Die()
        {
            DamageTaken += LifePoints;

            OnDead(this);
        }

        public virtual int InflictDirectDamage(int damage, FightActor from)
        {
            TriggerBuffs(BuffTriggerType.BEFORE_ATTACKED, damage);

            if (HasState((int)SpellStatesEnum.Invulnerable))
            {
                OnDamageReducted(from, damage);
                TriggerBuffs(BuffTriggerType.AFTER_ATTACKED, damage);
                return 0;
            }

            if (LifePoints - damage < 0)
                damage = (short)LifePoints;

            var permanentDamages = CalculateErosionDamage(damage);
            damage -= permanentDamages;
            Stats.Health.DamageTaken += damage;
            Stats.Health.PermanentDamages += permanentDamages;


            OnLifePointsChanged(-(damage + permanentDamages), permanentDamages, from);

            if (IsDead())
                OnDead(from);

            TriggerBuffs(BuffTriggerType.AFTER_ATTACKED, damage);

            return damage;
        }

        public int InflictDirectDamage(int damage)
        {
            return InflictDirectDamage(damage, this);
        }

        public int InflictDamage(int damage, EffectSchoolEnum school, FightActor from, bool pvp = false, Spell spell = null, bool withBoost = true)
        {
            var fractionGlyph = Fight.GetTriggers().FirstOrDefault(x => x is FractionGlyph && x.ContainsCell(Cell)) as FractionGlyph;
            if (fractionGlyph != null)
                return fractionGlyph.DispatchDamages(damage, school, from, pvp, spell, withBoost);

            if (spell != null)
                damage += from.GetSpellBoost(spell);

            if (withBoost)
                damage = from.CalculateDamage(damage, school);

            var minDamage = CalculateErosionDamage(damage);

            damage = CalculateDamageResistance(damage, school, pvp);

            int reduction = CalculateArmorReduction(school);

            if (damage - reduction < minDamage)
            {
                reduction = damage - minDamage;
            }

            if (reduction > 0)
                OnDamageReducted(from, reduction);

            if (from != this)
            {
                int reflected = CalculateDamageReflection(damage);

                if (reflected > 0)
                {
                    from.InflictDirectDamage(reflected, this);
                    OnDamageReflected(from, reflected);
                }
            }

            if (reduction > 0)
                damage -= reduction;

            if (damage <= 0)
                damage = 0;
            return InflictDirectDamage(damage, from);
        }

        public virtual int HealDirect(int healPoints, FightActor from)
        {
            if (HasState((int)SpellStatesEnum.Unhealable))
            {
                OnLifePointsChanged(0, 0, from);
                return 0;
            }

            if (LifePoints + healPoints > MaxLifePoints)
                healPoints = MaxLifePoints - LifePoints;

            DamageTaken -= healPoints;

            OnLifePointsChanged(healPoints, 0, from);

            return healPoints;
        }

        public int Heal(int healPoints, FightActor from , bool withBoost = true)
        {
            if (withBoost)
                healPoints = from.CalculateHeal(healPoints);

            return HealDirect(healPoints, from);
        }

        public void ExchangePositions(FightActor with)
        {
            var cell = Cell;
            
            Cell = with.Cell;
            with.Cell = cell;

            ActionsHandler.SendGameActionFightExchangePositionsMessage(Fight.Clients, this, with);
        }

        #region Formulas

        public virtual int CalculateDamage(int damage, EffectSchoolEnum type)
        {
            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    return (short) (damage*
                                    (100 + Stats[PlayerFields.Strength] + Stats[PlayerFields.DamageBonusPercent] + Stats[PlayerFields.DamageMultiplicator].Total*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].Total + Stats[PlayerFields.PhysicalDamage].Total + Stats[PlayerFields.NeutralDamageBonus]));
                case EffectSchoolEnum.Earth:
                    return (short) (damage*
                                    (100 + Stats[PlayerFields.Strength] + Stats[PlayerFields.DamageBonusPercent] + Stats[PlayerFields.DamageMultiplicator].Total*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].Total + Stats[PlayerFields.PhysicalDamage].Total + Stats[PlayerFields.EarthDamageBonus]));
                case EffectSchoolEnum.Air:
                    return (short) (damage*
                                    (100 + Stats[PlayerFields.Agility] + Stats[PlayerFields.DamageBonusPercent] + Stats[PlayerFields.DamageMultiplicator].Total*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].Total + Stats[PlayerFields.MagicDamage].Total + Stats[PlayerFields.AirDamageBonus]));
                case EffectSchoolEnum.Water:
                    return (short) (damage*
                                    (100 + Stats[PlayerFields.Chance] + Stats[PlayerFields.DamageBonusPercent] + Stats[PlayerFields.DamageMultiplicator].Total*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].Total + Stats[PlayerFields.MagicDamage].Total + Stats[PlayerFields.WaterDamageBonus]));
                case EffectSchoolEnum.Fire:
                    return (short) (damage*
                                    (100 + Stats[PlayerFields.Intelligence] + Stats[PlayerFields.DamageBonusPercent] + Stats[PlayerFields.DamageMultiplicator].Total*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].Total + Stats[PlayerFields.MagicDamage].Total + Stats[PlayerFields.FireDamageBonus]));
                default:
                    return damage;
            }
        }

        public virtual int CalculateDamageResistance(int damage, EffectSchoolEnum type, bool pvp)
        {
            double percentResistance = 0;
            double fixResistance = 0;

            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    percentResistance = Stats[PlayerFields.NeutralResistPercent].Total + (pvp ? Stats[PlayerFields.PvpNeutralResistPercent].Total : 0);
                    fixResistance = Stats[PlayerFields.NeutralElementReduction].Total + (pvp ? Stats[PlayerFields.PvpNeutralElementReduction].Total : 0) + Stats[PlayerFields.PhysicalDamageReduction];
                    break;
                case EffectSchoolEnum.Earth:
                    percentResistance = Stats[PlayerFields.EarthResistPercent].Total + (pvp ? Stats[PlayerFields.PvpEarthResistPercent].Total : 0);
                    fixResistance = Stats[PlayerFields.EarthElementReduction].Total + (pvp ? Stats[PlayerFields.PvpEarthElementReduction].Total : 0) + Stats[PlayerFields.PhysicalDamageReduction];
                    break;
                case EffectSchoolEnum.Air:
                    percentResistance = Stats[PlayerFields.AirResistPercent].Total + (pvp ? Stats[PlayerFields.PvpAirResistPercent].Total : 0);
                    fixResistance = Stats[PlayerFields.AirElementReduction].Total + (pvp ? Stats[PlayerFields.PvpAirElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                    break;
                case EffectSchoolEnum.Water:
                    percentResistance = Stats[PlayerFields.WaterResistPercent].Total + (pvp ? Stats[PlayerFields.PvpWaterResistPercent].Total : 0);
                    fixResistance = Stats[PlayerFields.WaterElementReduction].Total + (pvp ? Stats[PlayerFields.PvpWaterElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                    break;
                case EffectSchoolEnum.Fire:
                    percentResistance = Stats[PlayerFields.FireResistPercent].Total + (pvp ? Stats[PlayerFields.PvpFireResistPercent].Total : 0);
                    fixResistance = Stats[PlayerFields.FireElementReduction].Total + (pvp ? Stats[PlayerFields.PvpFireElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                    break;
                default:
                    return damage;
            }

            return (int)( ( 1 - percentResistance / 100d ) * ( damage - fixResistance ) );
        }

        public virtual int CalculateDamageReflection(int damage)
        {
            // only spell damage reflection are mutlplied by wisdom
            var reflectDamages = Stats[PlayerFields.DamageReflection].Context * ( 1 + ( Stats[PlayerFields.Wisdom].Total / 100 ) ) +
                ( Stats[PlayerFields.DamageReflection].Total - Stats[PlayerFields.DamageReflection].Context );

            if (reflectDamages > damage / 2d)
                return (int)( damage / 2d );

            return reflectDamages;
        }

        public virtual int CalculateHeal(int heal)
        {
            return
                (int) (heal*(100 + Stats[PlayerFields.Intelligence].Total)/100d + Stats[PlayerFields.HealBonus].Total);
        }

        public virtual int CalculateArmorValue(int reduction)
        {
            return (int) (reduction*(100 + 5*Level)/100d);
        }

        public virtual int CalculateErosionDamage(int damages)
        {
            var erosion = Stats[PlayerFields.Erosion].TotalSafe;

            if (erosion > 50)
                erosion = 50;

            return (int)( damages * ( erosion / 100d ) );

        }
        public virtual int CalculateArmorReduction(EffectSchoolEnum damageType)
        {
            int specificArmor = 0;
            switch (damageType)
            {
                case EffectSchoolEnum.Neutral:
                    specificArmor = Stats[PlayerFields.NeutralDamageArmor].Total;
                    break;
                case EffectSchoolEnum.Earth:
                    specificArmor = Stats[PlayerFields.EarthDamageArmor].Total;
                    break;
                case EffectSchoolEnum.Air:
                    specificArmor = Stats[PlayerFields.AirDamageArmor].Total;
                    break;
                case EffectSchoolEnum.Water:
                    specificArmor = Stats[PlayerFields.WaterDamageArmor].Total;
                    break;
                case EffectSchoolEnum.Fire:
                    specificArmor = Stats[PlayerFields.FireDamageArmor].Total;
                    break;
                default:
                    return 0;
            }

            return specificArmor + Stats[PlayerFields.GlobalDamageReduction].Total;
        }

        public virtual double CalculateCriticRate(double baseRate)
        {
            const double multipleOfE = Math.E*1.1;

            var prob = Math.Floor((baseRate - Stats[PlayerFields.CriticalHit].Total) * multipleOfE / Math.Log(Stats[PlayerFields.Agility].TotalSafe + 12, Math.E));

            return prob > 2 ? prob : 2;
        }

        public virtual FightSpellCastCriticalEnum RollCriticalDice(SpellLevelTemplate spell)
        {
            var random = new AsyncRandom();

            var critical = FightSpellCastCriticalEnum.NORMAL;

            if (spell.CriticalFailureProbability != 0 && random.Next((int)spell.CriticalFailureProbability) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;

            else if (spell.CriticalHitProbability != 0 && random.Next((int) CalculateCriticRate(spell.CriticalHitProbability)) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            return critical;
        }

        public virtual short CalculateReflectedDamageBonus(short spellBonus)
        {
            return (short) (spellBonus*(1 + (Stats[PlayerFields.Wisdom].Total/100d)) + Stats[PlayerFields.DamageReflection].Total);
        }

        public virtual bool RollAPLose(FightActor from)
        {
            var apAttack = from.Stats[PlayerFields.APAttack].Total > 1 ? from.Stats[PlayerFields.APAttack].TotalSafe : 1;
            var apDodge = Stats[PlayerFields.DodgeAPProbability].Total > 1 ? from.Stats[PlayerFields.DodgeAPProbability].TotalSafe : 1;

            var prob = (apAttack/(double) apDodge)*
                       ( ( Stats.AP.TotalMax / (double)( Stats.AP.TotalMax - Stats.AP.Used ) ) / 2d );

            if (prob < 0.10)
                prob = 0.10;
            else if (prob > 0.90)
                prob = 0.90;

            var rnd = new AsyncRandom().NextDouble();

            return rnd < prob;
        }

        public virtual bool RollMPLose(FightActor from)
        {
            var mpAttack = from.Stats[PlayerFields.MPAttack].Total > 1 ? from.Stats[PlayerFields.MPAttack].TotalSafe : 1;
            var mpDodge = Stats[PlayerFields.DodgeMPProbability].Total > 1 ? from.Stats[PlayerFields.DodgeMPProbability].TotalSafe : 1;

            var prob = (mpAttack/(double) mpDodge)*
                       ( ( Stats.AP.TotalMax / (double)( Stats.AP.TotalMax - Stats.AP.Used ) ) / 2d );

            if (prob < 0.10)
                prob = 0.10;
            else if (prob > 0.90)
                prob = 0.90;

            var rnd = new AsyncRandom().NextDouble();

            return rnd < prob;
        }

        public FightActor[] GetTacklers()
        {
            return OpposedTeam.GetAllFighters(entry => entry.IsAlive() && entry.Position.Point.IsAdjacentTo(Position.Point)).ToArray();
        }

        public virtual int GetTackledMP()
        {
            if (VisibleState != GameActionFightInvisibilityStateEnum.VISIBLE)
                return 0;

            var tacklers = GetTacklers();

            // no tacklers, then no tackle possible
            if (tacklers.Length <= 0)
                return 0;

            var percentLost = 0d;
            for (int i = 0; i < tacklers.Length; i++)
            {
                var fightActor = tacklers[i];

                if (i == 0)
                    percentLost = GetTacklePercent(fightActor);
                else
                {
                    percentLost *= GetTacklePercent(fightActor);
                }
            }

            percentLost = 1 - percentLost;

            if (percentLost < 0)
                percentLost = 0d;
            else if (percentLost > 1)
                percentLost = 1;

            return (int) (Math.Ceiling(MP*percentLost));
        }

        public virtual int GetTackledAP()
        {
            if (VisibleState != GameActionFightInvisibilityStateEnum.VISIBLE)
                return 0;

            var tacklers = GetTacklers();

            // no tacklers, then no tackle possible
            if (tacklers.Length <= 0)
                return 0;

            var percentLost = 0d;
            for (int i = 0; i < tacklers.Length; i++)
            {
                var fightActor = tacklers[i];

                if (i == 0)
                    percentLost = GetTacklePercent(fightActor);
                else
                {
                    percentLost *= GetTacklePercent(fightActor);
                }
            }

            percentLost = 1 - percentLost;

            if (percentLost < 0)
                percentLost = 0d;
            else if (percentLost > 1)
                percentLost = 1;

            return (int) (Math.Ceiling(AP*percentLost));
        }

        private double GetTacklePercent(FightActor tackler)
        {
            if (tackler.Stats[PlayerFields.TackleBlock].Total == -2)
                return 0;

            return (Stats[PlayerFields.TackleEvade].Total + 2)/((2d*(tackler.Stats[PlayerFields.TackleBlock].Total + 2)));
        }

        #endregion

        #endregion

        #region Buffs

        private readonly Dictionary<Spell, short> m_buffedSpells = new Dictionary<Spell, short>(); 
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

        public IEnumerable<Buff> GetBuffs()
        {
            return m_buffList;
        }

        public IEnumerable<Buff> GetBuffs(Predicate<Buff> predicate)
        {
            return m_buffList.Where(entry => predicate(entry));
        }

        public bool BuffMaxStackReached(Buff buff)
        {
            return buff.Spell.CurrentSpellLevel.MaxStack > 0 && buff.Spell.CurrentSpellLevel.MaxStack <= m_buffList.Count(entry => entry.Spell == buff.Spell && entry.Effect.EffectId == buff.Effect.EffectId);
        }

        public bool AddAndApplyBuff(Buff buff, bool freeIdIfFail = true)
        {
            if (BuffMaxStackReached(buff))
            {
                if (freeIdIfFail)
                    FreeBuffId(buff.Id);

                return false;
            }

            AddBuff(buff);

            if (!(buff is TriggerBuff) ||
                ((buff as TriggerBuff).Trigger & BuffTriggerType.BUFF_ADDED) == BuffTriggerType.BUFF_ADDED)
                buff.Apply();

            return true;
        }

        public bool AddBuff(Buff buff, bool freeIdIfFail = true)
        {
            if (BuffMaxStackReached(buff))
            {
                if (freeIdIfFail)
                    FreeBuffId(buff.Id);

                return false;
            }

            m_buffList.Add(buff);

            OnBuffAdded(buff);

            return true;
        }

        public void RemoveAndDispellBuff(Buff buff)
        {
            RemoveBuff(buff);

            buff.Dispell();
        }

        public void RemoveBuff(Buff buff)
        {
            m_buffList.Remove(buff);

            OnBuffRemoved(buff);

            FreeBuffId(buff.Id);
        }

        public void RemoveAndDispellAllBuffs()
        {
            var copyOfBuffs = m_buffList.ToArray();

            foreach (var buff in copyOfBuffs)
            {
                RemoveAndDispellBuff(buff);
            }
        }

        public void RemoveAndDispellAllBuffs(FightActor caster)
        {
            var copyOfBuffs = m_buffList.ToArray();

            foreach (var buff in copyOfBuffs)
            {
                if (buff.Caster == caster)
                    RemoveAndDispellBuff(buff);
            }
        }

        public void RemoveAllCastedBuffs()
        {
            foreach (var fighter in Fight.GetAllFighters())
            {
                fighter.RemoveAndDispellAllBuffs(this);
            }
        }

        public void TriggerBuffs(BuffTriggerType trigger, object token = null)
        {
            var copy = m_buffList.ToArray();
            foreach (var buff in copy)
            {
                var triggerBuff = buff as TriggerBuff;

                if (triggerBuff == null)
                    continue;

                if ((triggerBuff.Trigger & trigger) == trigger)
                {
                    Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED);
                    triggerBuff.Apply(trigger, token);
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
                if (buff is TriggerBuff && ( buff as TriggerBuff ).Trigger.HasFlag(BuffTriggerType.BUFF_ENDED))
                    ( buff as TriggerBuff ).Apply(BuffTriggerType.BUFF_ENDED);

                if (!(buff is TriggerBuff && ( buff as TriggerBuff ).Trigger.HasFlag(BuffTriggerType.BUFF_ENDED_TURNEND)))
                    RemoveAndDispellBuff(buff);
            }
        }

        public void TriggerBuffsRemovedOnTurnEnd()
        {
            foreach (var buff in m_buffList.Where(entry => entry.Duration <= 0 && entry is TriggerBuff &&
                ( entry as TriggerBuff ).Trigger.HasFlag(BuffTriggerType.BUFF_ENDED_TURNEND)).ToArray())
            {
                buff.Apply();
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

        public void BuffSpell(Spell spell, short boost)
        {
            if (!m_buffedSpells.ContainsKey(spell))
                m_buffedSpells.Add(spell, boost);
            else
                m_buffedSpells[spell] += boost;
        }

        public void UnBuffSpell(Spell spell, short boost)
        {
            if (!m_buffedSpells.ContainsKey(spell))
                return;

            m_buffedSpells[spell] -= boost;

            if (m_buffedSpells[spell] == 0)
                m_buffedSpells.Remove(spell);
        }

        public short GetSpellBoost(Spell spell)
        {
            if (!m_buffedSpells.ContainsKey(spell))
                return 0;

            return m_buffedSpells[spell];
        }


        #endregion

        #region Summons

        private int m_summonedCount;

        private readonly List<SummonedFighter> m_summons = new List<SummonedFighter>();

        public int SummonedCount
        {
            get { return m_summonedCount; }
        }

        public ReadOnlyCollection<SummonedFighter> GetSummons()
        {
            return m_summons.AsReadOnly();
        }

        public bool CanSummon()
        {
            return m_summonedCount < Stats[PlayerFields.SummonLimit].Total;
        }

        public void AddSummon(SummonedFighter summon)
        {
            if (summon is SummonedMonster && ( summon as SummonedMonster ).Monster.Template.UseSummonSlot)
                m_summonedCount++;
            // clone

            m_summons.Add(summon);
        }

        public void RemoveSummon(SummonedFighter summon)
        {
            if (summon is SummonedMonster && ( summon as SummonedMonster ).Monster.Template.UseSummonSlot)
                m_summonedCount--;

            m_summons.Remove(summon);
        }

        public void RemoveAllSummons()
        {
            m_summons.Clear();
        }

        public void KillAllSummons()
        {
            foreach (var summon in m_summons.ToArray())
            {
                summon.Die();
            }
        }

        #endregion

        #region States
        private readonly List<SpellState> m_states = new List<SpellState>();

        public void AddState(SpellState state)
        {
            m_states.Add(state);
        }

        public void RemoveState(SpellState state)
        {
            m_states.Remove(state);
        }

        public bool HasState(int stateId)
        {
            return m_states.Any(entry => entry.Id == stateId);
        }

        public bool HasState(SpellState state)
        {
            return HasState(state.Id);
        }

        public bool HasSpellBlockerState()
        {
            return m_states.Any(entry => entry.PreventsSpellCast);
        }

        public bool HasFightBlockerState()
        {
            return m_states.Any(entry => entry.PreventsFight);
        }


        #region Invisibility

        public GameActionFightInvisibilityStateEnum VisibleState
        {
            get;
            private set;
        }

        public void SetInvisibilityState(GameActionFightInvisibilityStateEnum state)
        {
            var lastState = VisibleState;
            VisibleState = state;

            OnVisibleStateChanged(this, lastState);
        }

        public void SetInvisibilityState(GameActionFightInvisibilityStateEnum state, FightActor source)
        {
            var lastState = VisibleState; 
            VisibleState = state;

            OnVisibleStateChanged(source, lastState);
        }

        public bool IsInvisibleSpellCast(Spell spell)
        {
            var spellLevel = spell.CurrentSpellLevel;

            if (!(this is CharacterFighter))
                return true;

            if (spellLevel.Effects.Any(entry => entry.EffectId == EffectsEnum.Effect_Trap) || // traps
                spellLevel.Effects.Any(entry => entry.EffectId == EffectsEnum.Effect_Summon) || // summons
                spell.Template.Id == 74 || // double
                spell.Template.Id == 62 || // chakra pulsion
                spell.Template.Id == 66 || // insidious poison
                spell.Template.Id == 67) // fear
                // todo : masteries
                return true;

            return false;
        }

        public bool DispellInvisibilityBuff()
        {
            var buffs = GetBuffs(entry => entry is InvisibilityBuff).ToArray();

            foreach (Buff buff in buffs)
            {
                RemoveAndDispellBuff(buff);
            }

            return buffs.Any();
        }

        public VisibleStateEnum GetVisibleStateFor(FightActor fighter)
        {
            return fighter.IsFriendlyWith(this) && VisibleState != VisibleStateEnum.VISIBLE ? VisibleStateEnum.DETECTED : VisibleState;
        }

        public VisibleStateEnum GetVisibleStateFor(Character character)
        {
            if (!character.IsFighting() || character.Fight != Fight)
                return VisibleState;

            return character.Fighter.IsFriendlyWith(this) && VisibleState != VisibleStateEnum.VISIBLE ? VisibleStateEnum.DETECTED : VisibleState;
        }

        public bool IsVisibleFor(FightActor fighter)
        {
            return GetVisibleStateFor(fighter) != GameActionFightInvisibilityStateEnum.INVISIBLE;
        }

        public bool IsVisibleFor(Character character)
        {
            return GetVisibleStateFor(character) != GameActionFightInvisibilityStateEnum.INVISIBLE;
        }

        protected virtual void OnVisibleStateChanged(FightActor source, VisibleStateEnum lastState)
        {
            Fight.ForEach(entry => ActionsHandler.SendGameActionFightInvisibilityMessage(entry.Client, source, this, GetVisibleStateFor(entry)), true);
        
            if (lastState == GameActionFightInvisibilityStateEnum.INVISIBLE)
                Fight.ForEach(entry => ContextHandler.SendGameFightRefreshFighterMessage(entry.Client, this));
        }

        #endregion

        #endregion

        #region End Fight

        public virtual void ResetFightProperties()
        {
            ResetUsedPoints();
            RemoveAndDispellAllBuffs();

            foreach (var field in Stats.Fields)
            {
                field.Value.Context = 0;
            }

            Stats.Health.PermanentDamages = 0;
        }

        public virtual IEnumerable<DroppedItem> RollLoot(CharacterFighter looter)
        {
            return new DroppedItem[0];
        }

        public virtual uint GetDroppedKamas()
        {
            return 0;
        }

        public virtual IFightResult GetFightResult()
        {
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
            return Stats.Health.Total > 0 && !HasLeft();
        }

        public bool IsDead()
        {
            return !IsAlive();
        }

        private bool m_left;
        public bool HasLeft()
        {
            return m_left;
        }

        public bool HasLost()
        {
            return Fight.Losers == Team;
        }

        public bool HasWin()
        {
            return Fight.Winners == Team;
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
            return IsFighterTurn() && IsAlive() && MP > 0;
        }

        public virtual bool CanPlay()
        {
            return IsAlive() && !HasLeft();
        }

        public override bool CanSee(WorldObject obj)
        {
            return base.CanSee(obj);
        }

        public override bool CanBeSee(WorldObject obj)
        {
            var fighter = obj as FightActor;
            var character = obj as Character;

            if (character != null && character.IsFighting())
                fighter = character.Fighter;

            if (fighter == null || fighter.Fight != Fight)
                return base.CanBeSee(obj) && VisibleState != GameActionFightInvisibilityStateEnum.INVISIBLE;

            return GetVisibleStateFor(fighter) != GameActionFightInvisibilityStateEnum.INVISIBLE && IsAlive();
        }

        #endregion

        #endregion

        #region Network

        public override EntityDispositionInformations GetEntityDispositionInformations()
        {
            return GetEntityDispositionInformations(null);
        }

        public virtual EntityDispositionInformations GetEntityDispositionInformations(WorldClient client = null)
        {
            return new FightEntityDispositionInformations(client != null ? ( IsVisibleFor(client.Character) ? Cell.Id : Cell.Null.Id ) : Cell.Id, (sbyte)Direction, CarriedActor != null ? CarriedActor.Id : 0);
        }

        public virtual GameFightMinimalStats GetGameFightMinimalStats()
        {
            return GetGameFightMinimalStats(null);
        }

        public virtual GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null)
        {
            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.Base,
                Stats[PlayerFields.PermanentDamagePercent].Total,
                0, // shieldsPoints = ?
                (short)Stats.AP.Total,
                (short)Stats.AP.TotalMax,
                (short)Stats.MP.Total,
                (short)Stats.MP.TotalMax,
                0,
                false,
                (short)Stats[PlayerFields.NeutralResistPercent].Total,
                (short)Stats[PlayerFields.EarthResistPercent].Total,
                (short)Stats[PlayerFields.WaterResistPercent].Total,
                (short)Stats[PlayerFields.AirResistPercent].Total,
                (short)Stats[PlayerFields.FireResistPercent].Total,
                (short)Stats[PlayerFields.NeutralElementReduction].Total,
                (short)Stats[PlayerFields.EarthElementReduction].Total,
                (short)Stats[PlayerFields.WaterElementReduction].Total,
                (short)Stats[PlayerFields.AirElementReduction].Total,
                (short)Stats[PlayerFields.FireElementReduction].Total,
                (short)Stats[PlayerFields.PushDamageReduction].Total,
                (short)Stats[PlayerFields.CriticalDamageReduction].Total,
                (short)Stats[PlayerFields.DodgeAPProbability].Total,
                (short)Stats[PlayerFields.DodgeMPProbability].Total,
                (short)Stats[PlayerFields.TackleBlock].Total,
                (short)Stats[PlayerFields.TackleEvade].Total,
                (sbyte)( client == null ? VisibleState : GetVisibleStateFor(client.Character) ) // invisibility state
                );
        }

        public virtual FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberInformations(Id);
        }

        public virtual GameFightFighterInformations GetGameFightFighterInformations()
        {
            return GetGameFightFighterInformations(null);
        }

        public virtual GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightFighterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(client),
                Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(client));
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return GetGameFightFighterInformations();
        }

        public abstract string GetMapRunningFighterName();

        #endregion
    }
}
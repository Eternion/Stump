using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Mathematics;
using Stump.Core.Pool;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Fights.History;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Context;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Results.FightLoot;
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
            var handler = ReadyStateChanged;
            if (handler != null)
                handler(this, isReady);
        }

        public event Action<FightActor, Cell, bool> CellShown;

        protected virtual void OnCellShown(Cell cell, bool team)
        {
            var handler = CellShown;
            if (handler != null)
                CellShown(this, cell, team);
        }

        public event Action<FightActor, int, int, int, FightActor> LifePointsChanged;

        protected virtual void OnLifePointsChanged(int delta, int shieldDamages, int permanentDamages, FightActor from)
        {
            var handler = LifePointsChanged;

            if (handler != null)
                handler(this, delta, shieldDamages, permanentDamages, from);
        }

        public event Action<FightActor, Damage> BeforeDamageInflicted;

        protected virtual void OnBeforeDamageInflicted(Damage damage)
        {
            var handler = BeforeDamageInflicted;

            if (handler != null)
                handler(this, damage);
        }

        public event Action<FightActor, Damage> DamageInflicted;

        protected virtual void OnDamageInflicted(Damage damage)
        {
            var handler = DamageInflicted;

            if (handler != null)
                handler(this, damage);
        }

        public event Action<FightActor, FightActor, int> DamageReducted;

        protected virtual void OnDamageReducted(FightActor source, int reduction)
        {
            var handler = DamageReducted;
            if (handler != null)
                handler(this, source, reduction);
        }

        public event Action<FightActor, FightActor, int> DamageReflected;

        protected internal virtual void OnDamageReflected(FightActor target, int reflected)
        {
            ActionsHandler.SendGameActionFightReflectDamagesMessage(Fight.Clients, this, target, (int)reflected);

            var handler = DamageReflected;
            if (handler != null)
                handler(this, target, reflected);
        }

        public event Action<FightActor> FighterLeft;

        public event Action<FightActor, ObjectPosition> PrePlacementChanged;

        protected virtual void OnPrePlacementChanged(ObjectPosition position)
        {
            var handler = PrePlacementChanged;
            if (handler != null)
                handler(this, position);
        }

        public event Action<FightActor> TurnPassed;

        protected virtual void OnTurnPassed()
        {
            var handler = TurnPassed;
            if (handler != null)
                handler(this);
        }

        public delegate void SpellCastingHandler(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast);

        public event SpellCastingHandler SpellCasting;

        protected virtual void OnSpellCasting(Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            var handler = SpellCasting;
            if (handler != null)
                handler(this, spell, target, critical, silentCast);
        }

        public event SpellCastingHandler SpellCasted;

        protected virtual void OnSpellCasted(Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (spell.CurrentSpellLevel.Effects.All(effect => effect.EffectId != EffectsEnum.Effect_Invisibility) &&
                VisibleState == GameActionFightInvisibilityStateEnum.INVISIBLE)
            {
                ShowCell(Cell, false);

                if (!IsInvisibleSpellCast(spell))
                    if (!DispellInvisibilityBuff())
                        SetInvisibilityState(VisibleStateEnum.VISIBLE);
            }

            SpellHistory.RegisterCastedSpell(spell.CurrentSpellLevel, Fight.GetOneFighter(target));

            var handler = SpellCasted;
            if (handler != null)
                handler(this, spell, target, critical, silentCast);
        }

        protected virtual void OnSpellCasted(Spell spell, FightActor target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (spell.CurrentSpellLevel.Effects.All(effect => effect.EffectId != EffectsEnum.Effect_Invisibility) &&
                VisibleState == GameActionFightInvisibilityStateEnum.INVISIBLE)
            {
                ShowCell(Cell, false);

                if (!IsInvisibleSpellCast(spell))
                    if (!DispellInvisibilityBuff())
                        SetInvisibilityState(VisibleStateEnum.VISIBLE);
            }

            SpellHistory.RegisterCastedSpell(spell.CurrentSpellLevel, target);

            var handler = SpellCasted;
            if (handler != null)
                handler(this, spell, target.Cell, critical, silentCast);
        }

        public event Action<FightActor, Spell, Cell> SpellCastFailed;

        protected virtual void OnSpellCastFailed(Spell spell, Cell cell)
        {
            var handler = SpellCastFailed;
            if (handler != null) handler(this, spell, cell);
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

            var handler = WeaponUsed;
            if (handler != null) handler(this, weapon, cell, critical, silentCast);
        }

        public event Action<FightActor, Buff> BuffAdded;

        protected virtual void OnBuffAdded(Buff buff)
        {
            var handler = BuffAdded;
            if (handler != null) handler(this, buff);
        }

        public event Action<FightActor, Buff> BuffRemoved;

        protected virtual void OnBuffRemoved(Buff buff)
        {
            var handler = BuffRemoved;
            if (handler != null) handler(this, buff);
        }

        public event Action<FightActor, FightActor> Dead;

        protected virtual void OnDead(FightActor killedBy, bool passTurn = true)
        {
            if (passTurn)
                PassTurn();

            KillAllSummons();
            RemoveAndDispellAllBuffs();

            var handler = Dead;
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

            var handler = FightPointsVariation;
            if (handler != null)
                handler(this, action, source, target, delta);
        }


        public event Action<FightActor, short> ApUsed;

        protected virtual void OnApUsed(short amount)
        {
            var handler = ApUsed;
            if (handler != null)
                handler(this, amount);
        }

        public event Action<FightActor, short> MpUsed;

        protected virtual void OnMpUsed(short amount)
        {
            var handler = MpUsed;
            if (handler != null)
                handler(this, amount);
        }

        public event Action<FightActor, bool> ActorPushed;

        public virtual void OnActorPushed(FightActor fighter, bool takeDamage)
        {
            var handler = ActorPushed;
            if (handler != null)
                handler(fighter, takeDamage);
        }

        #endregion

        #region Constructor

        protected FightActor(FightTeam team)
        {
            Team = team;
            VisibleState = VisibleStateEnum.VISIBLE;
            Loot = new FightLoot();
            SpellHistory = new SpellHistory(this);
        }

        #endregion

        #region Properties

        public IFight Fight
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
            get { return Team.OpposedTeam; }
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

        public ObjectPosition FightStartPosition
        {
            get;
            internal set;
        }

        public override bool BlockSight
        {
            get { return IsAlive() && VisibleState != GameActionFightInvisibilityStateEnum.INVISIBLE; }
        }

        public bool IsSacrificeProtected
        {
            get;
            set;
        }

        public virtual bool IsVisibleInTimeline
        {
            get { return true; }
        }

        #region Stats

        public abstract byte Level
        {
            get;
        }

        public int LifePoints
        {
            get { return Stats.Health.TotalSafe; }
        }

        public int MaxLifePoints
        {
            get
            {
                return Stats.Health.TotalMax;
            }
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
            if (!IsFighterTurn() || Fight.Freezed)
                return;

            Fight.StopTurn();

            OnTurnPassed();
        }

        #endregion

        #region Leave

        public void LeaveFight(bool force = false)
        {
            if (HasLeft())
                return;

            m_left = !force;

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
            Stats.MP.Used += amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST, this, this, (short)( -amount ));

            return true;
        }

        public virtual bool RegainAP(short amount)
        {
            /*if (amount > Stats.AP.Used)
                amount = Stats.AP.Used;*/

            Stats.AP.Used -= amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_WIN, this, this, amount);

            return true;
        }

        public virtual bool RegainMP(short amount)
        {
            /*if (amount > Stats.MP.Used)
                amount = Stats.MP.Used;*/

            Stats.MP.Used -= amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_WIN, this, this, amount);

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
                return SpellCastResult.NOT_IN_ZONE;
            }

            if (!SpellHistory.CanCastSpell(spellLevel, cell))
            {
                return SpellCastResult.HISTORY_ERROR;
            }

            if (spell.CurrentSpellLevel.CastTestLos && !Fight.CanBeSeen(Cell, cell))
            {
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

        public virtual bool CastSpell(Spell spell, Cell cell, bool force = false, bool ApFree = false)
        {
            if (!force && (!IsFighterTurn() || IsDead()))
                return false;

            var spellLevel = spell.CurrentSpellLevel;

            if (!force && CanCastSpell(spell, cell) != SpellCastResult.OK)
            {
                OnSpellCastFailed(spell, cell);
                return false;
            }

            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            var critical = RollCriticalDice(spellLevel);

            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
            {
                OnSpellCasting(spell, cell, critical, false);

                if (!ApFree)
                    UseAP((short) spellLevel.ApCost);

                Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

                if (spellLevel.CriticalFailureEndsTurn)
                    PassTurn();

                return false;
            }

            var handler = SpellManager.Instance.GetSpellCastHandler(this, spell, cell, critical == FightSpellCastCriticalEnum.CRITICAL_HIT);

            handler.Initialize();

            OnSpellCasting(spell, handler.TargetedCell, critical, handler.SilentCast);
            if (!ApFree)
                UseAP((short)spellLevel.ApCost);

            var fighter = Fight.GetOneFighter(handler.TargetedCell);

            handler.Execute();

            if (fighter == null)
                OnSpellCasted(spell, handler.TargetedCell, critical, handler.SilentCast);
            else
                OnSpellCasted(spell, fighter, critical, handler.SilentCast);

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

        public int InflictDirectDamage(int damage, FightActor from)
        {
            return InflictDamage(new Damage(damage) {Source = from, School = EffectSchoolEnum.Unknown, IgnoreDamageBoost = true, IgnoreDamageReduction = true});
        }

        public int InflictDirectDamage(int damage)
        {
            return InflictDirectDamage(damage, this);
        }

        public virtual int InflictDamage(Damage damage)
        {
            OnBeforeDamageInflicted(damage);
            damage.Source.TriggerBuffs(BuffTriggerType.BEFORE_ATTACK, damage);
            TriggerBuffs(BuffTriggerType.BEFORE_ATTACKED, damage);

            damage.GenerateDamages();

            if (HasState((int)SpellStatesEnum.Invulnerable))
            {
                OnDamageReducted(damage.Source, damage.Amount);
                damage.Source.TriggerBuffs(BuffTriggerType.AFTER_ATTACK, damage);
                TriggerBuffs(BuffTriggerType.AFTER_ATTACKED, damage);
                return 0;
            }

            if (damage.Source != null && !damage.IgnoreDamageBoost)
            {
                if (damage.Spell != null)
                    damage.Amount += damage.Source.GetSpellBoost(damage.Spell);

                damage.Amount = damage.Source.CalculateDamage(damage.Amount, damage.School, damage.IsCritical);
            }

            var permanentDamages = CalculateErosionDamage(damage.Amount);
            
            //Fraction
            var fractionBuff = GetBuffs(x => x is FractionBuff).FirstOrDefault() as FractionBuff;
            if (fractionBuff != null && !(damage is FractionDamage))
                return fractionBuff.DispatchDamages(damage);

            if (!damage.IgnoreDamageReduction)
            {
                var isPoisonSpell = damage.Spell != null && IsPoisonSpellCast(damage.Spell);
                var damageWithoutArmor = CalculateDamageResistance(damage.Amount, damage.School, damage.IsCritical, false, isPoisonSpell);
                damage.Amount = CalculateDamageResistance(damage.Amount, damage.School, damage.IsCritical, true, isPoisonSpell);

                var reduction = CalculateArmorReduction(damage.School);

                if (isPoisonSpell)
                    reduction = 0;

                if (reduction > 0)
                    OnDamageReducted(damage.Source, reduction);

                if (damage.Source != null && !damage.ReflectedDamages && !isPoisonSpell)
                {
                    var reflected = CalculateDamageReflection(damage.Amount);

                    if (reflected > 0)
                    {
                        damage.Source.InflictDirectDamage(reflected, this);
                        OnDamageReflected(damage.Source, reflected);
                    }
                }

                permanentDamages = CalculateErosionDamage(damageWithoutArmor);
            }

            var shieldDamages = 0;
            if (Stats.Shield.TotalSafe > 0)
            {
                if (Stats.Shield.TotalSafe > damage.Amount)
                {
                    shieldDamages += damage.Amount;
                    damage.Amount = 0;
                }
                else
                {
                    shieldDamages += Stats.Shield.TotalSafe;
                    damage.Amount -= Stats.Shield.TotalSafe;
                }
            }

            //Heal Or Multiply
            var healOrMultiplyBuff = GetBuffs(x => x is HealOrMultiplyBuff).FirstOrDefault() as HealOrMultiplyBuff;
            if (healOrMultiplyBuff != null)
            {
                var newDamage = healOrMultiplyBuff.GetDamages(damage.Amount);

                if (newDamage > 0)
                    damage.Amount = newDamage;
                else
                {
                    Heal(-newDamage, damage.Source, false);
                    return 0;
                }

                permanentDamages = 0;
            }

            if (damage.Amount <= 0)
                damage.Amount = 0;

            if (shieldDamages <= 0)
                shieldDamages = 0;

            if (damage.Amount > LifePoints)
            {
                damage.Amount = LifePoints;
                permanentDamages = 0;
            }

            Stats.Health.DamageTaken += damage.Amount;
            Stats.Health.PermanentDamages += permanentDamages;
            Stats.Shield.Context -= shieldDamages;

            OnLifePointsChanged(-damage.Amount, shieldDamages, permanentDamages, damage.Source);

            if (IsDead())
                OnDead(damage.Source);

            OnDamageInflicted(damage);

            if (damage.Source != null)
                damage.Source.TriggerBuffs(BuffTriggerType.AFTER_ATTACK, damage);

            TriggerBuffs(BuffTriggerType.AFTER_ATTACKED, damage);

            return damage.Amount;
        }

        public virtual int HealDirect(int healPoints, FightActor from)
        {
            TriggerBuffs(BuffTriggerType.BEFORE_HEALED);
            from.TriggerBuffs(BuffTriggerType.BEFORE_HEAL);

            if (HasState((int)SpellStatesEnum.Unhealable))
            {
                OnLifePointsChanged(0, 0, 0, from);
                return 0;
            }

            if (LifePoints + healPoints > MaxLifePoints)
                healPoints = MaxLifePoints - LifePoints;

            DamageTaken -= healPoints;

            OnLifePointsChanged(healPoints, 0, 0, from);

            TriggerBuffs(BuffTriggerType.AFTER_HEALED);
            from.TriggerBuffs(BuffTriggerType.AFTER_HEAL);

            return healPoints;
        }

        public int Heal(int healPoints, FightActor from , bool withBoost = true)
        {
            if (healPoints < 0)
                healPoints = 0;

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

        public virtual int CalculateDamage(int damage, EffectSchoolEnum type, bool critical)
        {
            // formulas :
            // DAMAGE * [(100 + STATS + %BONUS + MULT*100)/100 + (BONUS + PHS/MGKBONUS + ELTBONUS)]

            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    damage = (int) (damage*
                                    (100 + Stats[PlayerFields.Strength].TotalSafe +
                                     Stats[PlayerFields.DamageBonusPercent].TotalSafe +
                                     Stats[PlayerFields.DamageMultiplicator].TotalSafe*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].TotalSafe +
                                     Stats[PlayerFields.PhysicalDamage].TotalSafe +
                                     Stats[PlayerFields.NeutralDamageBonus].TotalSafe));
                    break;
                case EffectSchoolEnum.Earth:
                    damage = (int) (damage*
                                    (100 + Stats[PlayerFields.Strength].TotalSafe +
                                     Stats[PlayerFields.DamageBonusPercent].TotalSafe +
                                     Stats[PlayerFields.DamageMultiplicator].TotalSafe*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].TotalSafe +
                                     Stats[PlayerFields.PhysicalDamage].TotalSafe +
                                     Stats[PlayerFields.EarthDamageBonus].TotalSafe));
                    break;
                case EffectSchoolEnum.Air:
                    damage = (int) (damage*
                                    (100 + Stats[PlayerFields.Agility].TotalSafe +
                                     Stats[PlayerFields.DamageBonusPercent].TotalSafe +
                                     Stats[PlayerFields.DamageMultiplicator].TotalSafe*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].TotalSafe +
                                     Stats[PlayerFields.MagicDamage].TotalSafe +
                                     Stats[PlayerFields.AirDamageBonus].TotalSafe));
                    break;
                case EffectSchoolEnum.Water:
                    damage = (int) (damage*
                                    (100 + Stats[PlayerFields.Chance].TotalSafe +
                                     Stats[PlayerFields.DamageBonusPercent].TotalSafe +
                                     Stats[PlayerFields.DamageMultiplicator].TotalSafe*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].TotalSafe +
                                     Stats[PlayerFields.MagicDamage].TotalSafe +
                                     Stats[PlayerFields.WaterDamageBonus].TotalSafe));
                    break;
                case EffectSchoolEnum.Fire:
                    damage = (int) (damage*
                                    (100 + Stats[PlayerFields.Intelligence].TotalSafe +
                                     Stats[PlayerFields.DamageBonusPercent].TotalSafe +
                                     Stats[PlayerFields.DamageMultiplicator].TotalSafe*100)/100d +
                                    (Stats[PlayerFields.DamageBonus].TotalSafe +
                                     Stats[PlayerFields.MagicDamage].TotalSafe +
                                     Stats[PlayerFields.FireDamageBonus].TotalSafe));
                    break;
            }

            if (critical)
                return damage + Stats[PlayerFields.CriticalDamageBonus].Total;

            return damage;
        }

        public virtual int CalculateDamageResistance(int damage, EffectSchoolEnum type, bool critical, bool withArmor, bool poison)
        {           
            var percentResistance = CalculateTotalResistances(type, true, poison);
            var fixResistance = CalculateTotalResistances(type, false, poison);
            var armorResistance = withArmor && !poison ? CalculateArmorReduction(type) : 0;

            percentResistance = percentResistance > StatsFields.ResistanceLimit ? StatsFields.ResistanceLimit : percentResistance;
            fixResistance = fixResistance > StatsFields.ResistanceLimit ? StatsFields.ResistanceLimit : fixResistance;

            var result = (int)((1 - percentResistance / 100d) * (damage - armorResistance - fixResistance)) -
                         (critical ? Stats[PlayerFields.CriticalDamageReduction].Total : 0);

            return result;
        }

        public virtual int CalculateTotalResistances(EffectSchoolEnum type, bool percent, bool poison)
        {
            var pvp = Fight.IsPvP;

            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    if (percent)
                        return Stats[PlayerFields.NeutralResistPercent].Base + Stats[PlayerFields.NeutralResistPercent].Equiped + Stats[PlayerFields.NeutralResistPercent].Given + (poison ? 0 : Stats[PlayerFields.NeutralResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpNeutralResistPercent].Total : 0);

                    return Stats[PlayerFields.NeutralElementReduction].Base + Stats[PlayerFields.NeutralElementReduction].Equiped + Stats[PlayerFields.NeutralElementReduction].Given + (poison ? 0 : Stats[PlayerFields.NeutralElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpNeutralElementReduction].Total : 0) + Stats[PlayerFields.PhysicalDamageReduction];
                case EffectSchoolEnum.Earth:
                    if (percent)
                        return Stats[PlayerFields.EarthResistPercent].Base + Stats[PlayerFields.EarthResistPercent].Equiped + Stats[PlayerFields.EarthResistPercent].Given + (poison ? 0 : Stats[PlayerFields.EarthResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpEarthResistPercent].Total : 0);

                    return Stats[PlayerFields.EarthElementReduction].Base + Stats[PlayerFields.EarthElementReduction].Equiped + Stats[PlayerFields.EarthElementReduction].Given + (poison ? 0 : Stats[PlayerFields.EarthElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpEarthElementReduction].Total : 0) + Stats[PlayerFields.PhysicalDamageReduction];
                case EffectSchoolEnum.Air:
                    if (percent)
                        return Stats[PlayerFields.AirResistPercent].Base + Stats[PlayerFields.AirResistPercent].Equiped + Stats[PlayerFields.AirResistPercent].Given + (poison ? 0 : Stats[PlayerFields.AirResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpAirElementReduction].Total : 0);

                    return Stats[PlayerFields.AirElementReduction].Base + Stats[PlayerFields.AirElementReduction].Equiped + Stats[PlayerFields.AirElementReduction].Given + (poison ? 0 : Stats[PlayerFields.AirElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpAirElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                case EffectSchoolEnum.Water:
                    if (percent)
                        return Stats[PlayerFields.WaterResistPercent].Base + Stats[PlayerFields.WaterResistPercent].Equiped + Stats[PlayerFields.WaterResistPercent].Given + (poison ? 0 : Stats[PlayerFields.WaterResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpWaterElementReduction].Total : 0);

                    return Stats[PlayerFields.WaterElementReduction].Base + Stats[PlayerFields.WaterElementReduction].Equiped + Stats[PlayerFields.WaterElementReduction].Given + (poison ? 0 : Stats[PlayerFields.WaterElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpWaterElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                case EffectSchoolEnum.Fire:
                    if (percent)
                        return Stats[PlayerFields.FireResistPercent].Base + Stats[PlayerFields.FireResistPercent].Equiped + Stats[PlayerFields.FireResistPercent].Given + (poison ? 0 : Stats[PlayerFields.FireResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpFireResistPercent].Total : 0);

                    return Stats[PlayerFields.FireElementReduction].Base + Stats[PlayerFields.FireElementReduction].Equiped + Stats[PlayerFields.FireElementReduction].Given + (poison ? 0 : Stats[PlayerFields.FireElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpFireElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                default:
                    return 0;
            }
        }

        public virtual int CalculateDamageReflection(int damage)
        {
            // only spell damage reflection are mutlplied by wisdom
            var reflectDamages = Stats[PlayerFields.DamageReflection].Context * ( 1 + ( Stats[PlayerFields.Wisdom].TotalSafe / 100 ) ) +
                (Stats[PlayerFields.DamageReflection].TotalSafe - Stats[PlayerFields.DamageReflection].Context);

            if (reflectDamages > damage / 2d)
                return (int)( damage / 2d );

            return reflectDamages;
        }

        public virtual int CalculateHeal(int heal)
        {
            return (int)(heal * (100 + Stats[PlayerFields.Intelligence].TotalSafe) / 100d + Stats[PlayerFields.HealBonus].TotalSafe);
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

            if (GetBuffs(x => x.Spell.Id == (int) SpellIdEnum.TRÊVE).Any())
                erosion -= 10;

            return (int)( damages * ( erosion / 100d ) );

        }
        public virtual int CalculateArmorReduction(EffectSchoolEnum damageType)
        {
            int specificArmor;
            switch (damageType)
            {
                case EffectSchoolEnum.Neutral:
                    specificArmor = Stats[PlayerFields.NeutralDamageArmor].TotalSafe;
                    break;
                case EffectSchoolEnum.Earth:
                    specificArmor = Stats[PlayerFields.EarthDamageArmor].TotalSafe;
                    break;
                case EffectSchoolEnum.Air:
                    specificArmor = Stats[PlayerFields.AirDamageArmor].TotalSafe;
                    break;
                case EffectSchoolEnum.Water:
                    specificArmor = Stats[PlayerFields.WaterDamageArmor].TotalSafe;
                    break;
                case EffectSchoolEnum.Fire:
                    specificArmor = Stats[PlayerFields.FireDamageArmor].TotalSafe;
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

        public virtual int CalculateReflectedDamageBonus(int spellBonus)
        {
            return (int)(spellBonus * (1 + (Stats[PlayerFields.Wisdom].TotalSafe / 100d)) + Stats[PlayerFields.DamageReflection].TotalSafe);
        }

        public virtual bool RollAPLose(FightActor from, int value)
        {
            var apAttack = from.Stats[PlayerFields.APAttack].Total > 1 ? from.Stats[PlayerFields.APAttack].TotalSafe : 1;
            var apDodge = Stats[PlayerFields.DodgeAPProbability].Total > 1 ? Stats[PlayerFields.DodgeAPProbability].TotalSafe : 1;
            var prob = ((Stats.AP.Total-value)/(double) (Stats.AP.TotalMax))*(apAttack/(double) apDodge) /2d;

            if (prob < 0.10)
                prob = 0.10;
            else if (prob > 0.90)
                prob = 0.90;

            var rnd = new CryptoRandom().NextDouble();

            return rnd < prob;
        }

        public virtual bool RollMPLose(FightActor from, int value)
        {
            var mpAttack = from.Stats[PlayerFields.MPAttack].Total > 1 ? from.Stats[PlayerFields.MPAttack].TotalSafe : 1;
            var mpDodge = Stats[PlayerFields.DodgeMPProbability].Total > 1 ? Stats[PlayerFields.DodgeMPProbability].TotalSafe : 1;
            var prob = ((Stats.MP.Total - value) / (double)(Stats.MP.TotalMax)) * (mpAttack / (double)mpDodge) / 2d;

            if (prob < 0.10)
                prob = 0.10;
            else if (prob > 0.90)
                prob = 0.90 - (0.10 * value);

            var rnd = new CryptoRandom().NextDouble();

            return rnd < prob;
        }

        public FightActor[] GetTacklers()
        {
            return OpposedTeam.GetAllFighters(entry => entry.CanTackle(this) && entry.Position.Point.IsAdjacentTo(Position.Point)).ToArray();
        }

        public virtual int GetTackledMP()
        {
            if (VisibleState != GameActionFightInvisibilityStateEnum.VISIBLE)
                return 0;

            if (HasState((int)SpellStatesEnum.Unlockable))
                return 0;

            var tacklers = GetTacklers();

            // no tacklers, then no tackle possible
            if (tacklers.Length <= 0)
                return 0;

            var percentLost = 0d;
            for (var i = 0; i < tacklers.Length; i++)
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

            if (HasState((int)SpellStatesEnum.Unlockable))
                return 0;

            var tacklers = GetTacklers();

            // no tacklers, then no tackle possible
            if (tacklers.Length <= 0)
                return 0;

            var percentLost = 0d;
            for (var i = 0; i < tacklers.Length; i++)
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

        private double GetTacklePercent(IStatsOwner tackler)
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
            return buff.Spell.CurrentSpellLevel.MaxStack > 0 && buff.Spell.CurrentSpellLevel.MaxStack <= m_buffList.Count(entry => entry.Spell == buff.Spell && entry.Effect.EffectId == buff.Effect.EffectId && !(buff is DelayBuff));
        }

        public bool AddAndApplyBuff(Buff buff, bool freeIdIfFail = true, bool bypassMaxStack = false)
        {
            if (BuffMaxStackReached(buff) && !bypassMaxStack)
            {
                if (freeIdIfFail)
                    FreeBuffId(buff.Id);

                return false;
            }

            AddBuff(buff, freeIdIfFail, bypassMaxStack);

            if (!(buff is TriggerBuff) && !(buff is DelayBuff))
                buff.Apply();

            if (buff is TriggerBuff && ((buff as TriggerBuff).Trigger & BuffTriggerType.BUFF_ADDED) == BuffTriggerType.BUFF_ADDED)
                buff.Apply();

            return true;
        }

        public bool AddBuff(Buff buff, bool freeIdIfFail = true, bool bypassMaxStack = false)
        {
            if (BuffMaxStackReached(buff) && !bypassMaxStack)
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

        public void RemoveSpellBuffs(int spellId)
        {
            foreach (var buff in m_buffList.Where(x => x.Spell.Id == spellId).ToArray())
            {
                RemoveAndDispellBuff(buff);
            }
        }

        public void RemoveAndDispellAllBuffs()
        {
            foreach (var buff in m_buffList.ToArray())
            {
                RemoveAndDispellBuff(buff);
            }
        }

        public void RemoveAndDispellAllBuffs(FightActor caster)
        {
            var copyOfBuffs = m_buffList.ToArray();

            foreach (var buff in copyOfBuffs.Where(buff => buff.Caster == caster))
            {
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
            foreach (var triggerBuff in copy.OfType<TriggerBuff>().Where(triggerBuff => (triggerBuff.Trigger & trigger) == trigger))
            {
                Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED);
                triggerBuff.Apply(trigger, token);
                Fight.EndSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED);
            }
        }

        public void DecrementBuffsDuration(FightActor caster)
        {
            var buffsToRemove = m_buffList.Where(buff => buff.Caster == caster).Where(buff => buff.DecrementDuration()).ToList();

            foreach (var buff in buffsToRemove)
            {
                if (buff is TriggerBuff && ( buff as TriggerBuff ).Trigger.HasFlag(BuffTriggerType.BUFF_ENDED))
                    (buff as TriggerBuff).Apply(BuffTriggerType.BUFF_ENDED);

                if (!(buff is TriggerBuff && ( buff as TriggerBuff ).Trigger.HasFlag(BuffTriggerType.BUFF_ENDED_TURNEND)))
                    RemoveAndDispellBuff(buff);
            }
        }

        public void TriggerBuffsRemovedOnTurnEnd()
        {
            foreach (var buff in m_buffList.Where(entry => entry.Duration <= 0 && entry is TriggerBuff &&
                ((TriggerBuff) entry).Trigger.HasFlag(BuffTriggerType.BUFF_ENDED_TURNEND)).ToArray())
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
            return !m_buffedSpells.ContainsKey(spell) ? (short) 0 : m_buffedSpells[spell];
        }

        public bool MustSkipTurn()
        {
            return GetBuffs(x => x is SkipTurnBuff).Any();
        }


        #endregion

        #region Summons

        private readonly List<SummonedFighter> m_summons = new List<SummonedFighter>();
        private readonly List<SlaveFighter> m_slaves = new List<SlaveFighter>();
        private readonly List<SummonedBomb> m_bombs = new List<SummonedBomb>();

        public int SummonedCount
        {
            get;
            private set;
        }

        public int BombsCount
        {
            get;
            set;
        }

        public ReadOnlyCollection<SummonedFighter> Summons
        {
            get { return m_summons.AsReadOnly(); }
        }

        public ReadOnlyCollection<SlaveFighter> Slaves
        {
            get { return m_slaves.AsReadOnly(); }
        }

        public ReadOnlyCollection<SummonedBomb> Bombs
        {
            get { return m_bombs.AsReadOnly(); }
        }


        public bool CanSummon()
        {
            return SummonedCount < Stats[PlayerFields.SummonLimit].Total;
        }

        public bool CanSummonBomb()
        {
            return BombsCount < SummonedBomb.BombLimit;
        }

        public void AddSummon(SummonedFighter summon)
        {
            if (summon is SummonedMonster && ( summon as SummonedMonster ).Monster.Template.UseSummonSlot)
                SummonedCount++;
            // clone

            m_summons.Add(summon);
        }

        public void RemoveSummon(SummonedFighter summon)
        {
            if (summon is SummonedMonster && ( summon as SummonedMonster ).Monster.Template.UseSummonSlot)
                SummonedCount--;

            m_summons.Remove(summon);
        }

        public void AddSlave(SlaveFighter slave)
        {
            if (slave.Monster.Template.UseSummonSlot)
                SummonedCount++;

            m_slaves.Add(slave);
        }

        public void RemoveSlave(SlaveFighter slave)
        {
            if (slave.Monster.Template.UseSummonSlot)
                SummonedCount--;

            m_slaves.Remove(slave);
        }

        public void AddBomb(SummonedBomb bomb)
        {
            if (bomb.MonsterBombTemplate.Template.UseBombSlot)
                BombsCount++;

            m_bombs.Add(bomb);
        }

        public void RemoveBomb(SummonedBomb bomb)
        {
            if (bomb.MonsterBombTemplate.Template.UseBombSlot)
                BombsCount--;

            m_bombs.Remove(bomb);
        }

        public void RemoveAllSummons()
        {
            m_summons.Clear();
            m_bombs.Clear();
            m_slaves.Clear();
        }

        public void KillAllSummons()
        {
            foreach (var summon in m_summons.ToArray())
            {
                summon.Die();
            }

            foreach (var bomb in m_bombs.ToArray())
            {
                bomb.Die();
            }

            foreach (var slave in m_slaves.ToArray())
            {
                slave.Die();
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

        public bool IsPoisonSpellCast(Spell spell)
        {
            return spell.Template.Id == (int) SpellIdEnum.POISON_INSIDIEUX ||
                   spell.Template.Id == (int)SpellIdEnum.POISON_INSIDIEUX_DU_DOPEUL ||
                   spell.Template.Id == (int)SpellIdEnum.POISON_PARALYSANT ||
                   spell.Template.Id == (int)SpellIdEnum.POISON_PARALYSANT_DU_DOPEUL ||
                   spell.Template.Id == (int)SpellIdEnum.FLECHETTE_EMPOISONNÉE ||
                   spell.Template.Id == (int)SpellIdEnum.FLÈCHE_EMPOISONNÉE ||
                   spell.Template.Id == (int)SpellIdEnum.BROUILLARD_EMPOISONNÉ ||
                   spell.Template.Id == (int)SpellIdEnum.TOURBE_EMPOISONNÉE ||
                   spell.Template.Id == (int)SpellIdEnum.GRAINE_EMPOISONNÉE ||
                   spell.Template.Id == (int)SpellIdEnum.RONCE_EMPOISONNÉE ||
                   spell.Template.Id == (int)SpellIdEnum.PIÈGE_EMPOISONNÉ ||
                   spell.Template.Id == (int)SpellIdEnum.PIÈGE_EMPOISONNÉ_DU_DOPEUL ||
                   spell.Template.Id == (int)SpellIdEnum.VENT_EMPOISONNÉ ||
                   spell.Template.Id == (int)SpellIdEnum.VENT_EMPOISONNÉ_DU_DOPEUL ||
                   spell.Template.Id == (int)SpellIdEnum.TREMBLEMENT ||
                   spell.Template.Id == (int)SpellIdEnum.RONCE_INSOLENTE;
        }

        public bool IsInvisibleSpellCast(Spell spell)
        {
            var spellLevel = spell.CurrentSpellLevel;

            if (!(this is CharacterFighter))
                return true;

            return spellLevel.Effects.Any(entry => entry.EffectId == EffectsEnum.Effect_Trap) || // traps
                   spellLevel.Effects.Any(entry => entry.EffectId == EffectsEnum.Effect_Summon) || // summons
                   spell.Template.Id == (int)SpellIdEnum.DOUBLE || // double
                   spell.Template.Id == (int)SpellIdEnum.PULSION_DE_CHAKRA || // chakra pulsion
                   spell.Template.Id == (int)SpellIdEnum.CONCENTRATION_DE_CHAKRA || // chakra concentration
                   spell.Template.Id == (int)SpellIdEnum.POISON_INSIDIEUX || // insidious poison
                   spell.Template.Id == (int)SpellIdEnum.PEUR || //Fear
                   spell.Template.Id == (int)SpellIdEnum.POISSE; //Jinx
        }

        public bool DispellInvisibilityBuff()
        {
            var buffs = GetBuffs(entry => entry is InvisibilityBuff).ToArray();

            foreach (var buff in buffs)
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

        #region Carry/Throw

        private FightActor m_carriedActor;

        public bool IsCarrying()
        {
            return m_carriedActor != null;
        }

        public bool IsCarried()
        {
            return GetCarryingActor() != null;
        }

        public FightActor GetCarriedActor()
        {
            return m_carriedActor;
        }

        public FightActor GetCarryingActor()
        {
            return Fight.GetFirstFighter<FightActor>(x => x.GetCarriedActor() == this);
        }

        public void CarryActor(FightActor target, EffectBase effect, Spell spell)
        {
            var stateCarried = SpellManager.Instance.GetSpellState((uint)SpellStatesEnum.Carried);
            var stateCarrying = SpellManager.Instance.GetSpellState((uint)SpellStatesEnum.Carrying);

            if (HasState(stateCarrying) || HasState(stateCarried) || target.HasState(stateCarrying) || target.HasState(stateCarried))
                return;

            var actorBuffId = PopNextBuffId();
            var targetBuffId = target.PopNextBuffId();

            var actorBuff = new StateBuff(actorBuffId, this, this, effect, spell, false, stateCarrying)
            {
                Duration = -1
            };

            var targetBuff = new StateBuff(targetBuffId, target, this, effect, spell, false, stateCarried)
            {
                Duration = -1
            };

            AddAndApplyBuff(actorBuff);
            target.AddAndApplyBuff(targetBuff);

            ActionsHandler.SendGameActionFightCarryCharacterMessage(Fight.Clients, this, target);        

            target.Position.Cell = Position.Cell;
            m_carriedActor = target;

            m_carriedActor.Dead += OnCarryingActorDead;
            //m_carriedActor.FighterLeft += OnCarryingActorLeft;
            Dead += OnCarryingActorDead;
            //FighterLeft += OnCarryingActorLeft;
        }

        public void ThrowActor(Cell cell, bool drop = false)
        {
            var actor = Fight.GetOneFighter(cell);
            if (actor != null && !drop)
                return;

            var actorState = GetBuffs(x => x is StateBuff && (x as StateBuff).State.Id == (int)SpellStatesEnum.Carrying).FirstOrDefault();
            var targetState = m_carriedActor.GetBuffs(x => x is StateBuff && (x as StateBuff).State.Id == (int)SpellStatesEnum.Carried).FirstOrDefault();

            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);

            if (drop)
                ActionsHandler.SendGameActionFightDropCharacterMessage(Fight.Clients, this, m_carriedActor, cell);   
            else
                ActionsHandler.SendGameActionFightThrowCharacterMessage(Fight.Clients, this, m_carriedActor, cell);

            if (actorState != null)
                RemoveAndDispellBuff(actorState);

            if (targetState != null)
                m_carriedActor.RemoveAndDispellBuff(targetState);

            RemoveSpellBuffs((int)SpellIdEnum.KARCHAM);
            m_carriedActor.RemoveSpellBuffs((int)SpellIdEnum.KARCHAM);

            m_carriedActor.Position.Cell = cell;

            Fight.ForEach(entry => ContextHandler.SendGameFightRefreshFighterMessage(entry.Client, m_carriedActor));

            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);

            m_carriedActor.Dead -= OnCarryingActorDead;
            Dead -= OnCarryingActorDead;

            m_carriedActor = null;
        }

        public override bool StartMove(Path movementPath)
        {
            if (!IsCarried())
                return base.StartMove(movementPath);

            var carryingActor = GetCarryingActor();

            if (carryingActor == null)
                return base.StartMove(movementPath);

            //movementPath.CutPath(1, true);

            carryingActor.ThrowActor(movementPath.StartCell, true);

            return base.StartMove(movementPath);
        }

        public override bool StopMove()
        {
            if (!base.StopMove())
                return false;

            if (IsCarrying())
            {
                m_carriedActor.Position.Cell = Position.Cell;
            }

            return true;
        }

        private void OnCarryingActorDead(FightActor actor, FightActor killer)
        {
            ThrowActor(Cell, true);
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

        public virtual IEnumerable<DroppedItem> RollLoot(IFightResult looter)
        {
            return new DroppedItem[0];
        }

        public virtual uint GetDroppedKamas()
        {
            return 0;
        }

        public virtual int GetGivenExperience()
        {
            return 0;
        }
        
        public virtual IFightResult GetFightResult(FightOutcomeEnum outcome)
        {
            return new FightResult(this, outcome, Loot);
        }

        public IFightResult GetFightResult()
        {
            return GetFightResult(GetFighterOutcome());
        }

        public FightOutcomeEnum GetFighterOutcome()
        {
            /*if (HasLeft())
                return FightOutcomeEnum.RESULT_LOST;*/

            var teamDead = Team.AreAllDead();
            var opposedTeamDead = OpposedTeam.AreAllDead();

            if (!teamDead && opposedTeamDead)
                return FightOutcomeEnum.RESULT_VICTORY;

            if (teamDead && !opposedTeamDead)
                return FightOutcomeEnum.RESULT_LOST;

            return FightOutcomeEnum.RESULT_VICTORY;
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

        public virtual bool CanTackle(FightActor fighter)
        {
            return IsEnnemyWith(fighter) && IsAlive() && IsVisibleFor(fighter) && !HasState((int)SpellStatesEnum.Rooted) && !fighter.HasState((int)SpellStatesEnum.Rooted) && fighter.Position.Cell != Position.Cell;
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
            return GetEntityDispositionInformations();
        }

        public virtual EntityDispositionInformations GetEntityDispositionInformations(WorldClient client = null)
        {
            return new FightEntityDispositionInformations(client != null ? ( IsVisibleFor(client.Character) ? Cell.Id : (short)-1 ) : Cell.Id, (sbyte)Direction, CarriedActor != null ? CarriedActor.Id : 0);
        }

        public virtual GameFightMinimalStats GetGameFightMinimalStats()
        {
            return GetGameFightMinimalStats(null);
        }

        public virtual GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null)
        {
            var pvp = Fight.IsPvP;

            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.Base,
                Stats[PlayerFields.PermanentDamagePercent].Total,
                Stats.Shield.TotalSafe,
                (short)Stats.AP.Total,
                (short)Stats.AP.TotalMax,
                (short)Stats.MP.Total,
                (short)Stats.MP.TotalMax,
                0,
                false,
                (short)(Stats[PlayerFields.NeutralResistPercent].Total + (pvp ? Stats[PlayerFields.PvpNeutralResistPercent].Total : 0)),
                (short)(Stats[PlayerFields.EarthResistPercent].Total + (pvp ? Stats[PlayerFields.PvpEarthResistPercent].Total : 0)),
                (short)(Stats[PlayerFields.WaterResistPercent].Total + (pvp ? Stats[PlayerFields.PvpWaterResistPercent].Total : 0)),
                (short)(Stats[PlayerFields.AirResistPercent].Total + (pvp ? Stats[PlayerFields.PvpAirResistPercent].Total : 0)),
                (short)(Stats[PlayerFields.FireResistPercent].Total + (pvp ? Stats[PlayerFields.PvpFireResistPercent].Total : 0)),
                (short)(Stats[PlayerFields.NeutralElementReduction].Total + (pvp ? Stats[PlayerFields.PvpNeutralElementReduction].Total : 0)),
                (short)(Stats[PlayerFields.EarthElementReduction].Total + (pvp ? Stats[PlayerFields.PvpEarthElementReduction].Total : 0)),
                (short)(Stats[PlayerFields.WaterElementReduction].Total + (pvp ? Stats[PlayerFields.PvpWaterElementReduction].Total : 0)),
                (short)(Stats[PlayerFields.AirElementReduction].Total + (pvp ? Stats[PlayerFields.PvpAirElementReduction].Total : 0)),
                (short)(Stats[PlayerFields.FireElementReduction].Total + (pvp ? Stats[PlayerFields.PvpFireElementReduction].Total : 0)),
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
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte)Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(client));
        }

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
        {
            return GetGameFightFighterInformations();
        }

        public abstract string GetMapRunningFighterName();

        #endregion
    }
}
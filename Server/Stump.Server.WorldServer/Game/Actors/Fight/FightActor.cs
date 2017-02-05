using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Mathematics;
using Stump.Core.Memory;
using Stump.Core.Pool;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States;
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
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Context;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Results.FightLoot;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using SpellState = Stump.Server.WorldServer.Database.Spells.SpellState;
using VisibleStateEnum = Stump.DofusProtocol.Enums.GameActionFightInvisibilityStateEnum;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells.Casts;
using Stump.Core.Extensions;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public abstract class FightActor : ContextActor, IStatsOwner
    {
        public const int UNLIMITED_ZONE_SIZE = 50;

        #region Events

        public event Action<FightActor> GetAlive;

        public virtual void OnGetAlive()
        {
            foreach (var idol in Fight.ActiveIdols)
            {
                if (GetType().Name != idol.Template.TargetString)
                    continue;

                CastAutoSpell(new Spell(idol.Template.IdolSpellId.Value, idol.Template.IdolSpellLevel), Cell);
            }

            GetAlive?.Invoke(this);
        }

        public event Action<FightActor, bool> ReadyStateChanged;

        protected virtual void OnReadyStateChanged(bool isReady)
        {
            ReadyStateChanged?.Invoke(this, isReady);
        }

        public event Action<FightActor, Cell, bool> CellShown;

        protected virtual void OnCellShown(Cell cell, bool team)
        {
            CellShown?.Invoke(this, cell, team);
        }

        public event Action<FightActor, int, int, int, FightActor, EffectSchoolEnum> LifePointsChanged;

        public event Action<FightActor> FighterLeft;

        protected virtual void OnLeft()
        {
            FighterLeft?.Invoke(this);
        }

        protected virtual void OnLifePointsChanged(int delta, int shieldDamages, int permanentDamages, FightActor from, EffectSchoolEnum school)
        {
            LifePointsChanged?.Invoke(this, delta, shieldDamages, permanentDamages, from, school);
        }

        public event Action<FightActor, Damage> BeforeDamageInflicted;

        protected virtual void OnBeforeDamageInflicted(Damage damage)
        {
            BeforeDamageInflicted?.Invoke(this, damage);
        }

        public event Action<FightActor, Damage> DamageInflicted;

        protected virtual void OnDamageInflicted(Damage damage)
        {
            DamageInflicted?.Invoke(this, damage);
        }

        public event Action<FightActor, FightActor, int> DamageReducted;

        protected virtual void OnDamageReducted(FightActor source, int reduction)
        {
            DamageReducted?.Invoke(this, source, reduction);
        }

        public event Action<FightActor, FightActor> DamageReflected;

        protected internal virtual void OnDamageReflected(FightActor target)
        {
            ActionsHandler.SendGameActionFightReflectDamagesMessage(Fight.Clients, this, target);
            DamageReflected?.Invoke(this, target);
        }


        public event Action<FightActor, ObjectPosition> PrePlacementChanged;

        protected virtual void OnPrePlacementChanged(ObjectPosition position)
        {
            PrePlacementChanged?.Invoke(this, position);
        }

        public event Action<FightActor, FightActor> PrePlacementSwapped;

        protected virtual void OnPrePlacementSwapped(FightActor actor)
        {
            PrePlacementSwapped?.Invoke(this, actor);
        }

        public event Action<FightActor> TurnPassed;

        protected virtual void OnTurnPassed()
        {
            TurnPassed?.Invoke(this);
        }

        public delegate void SpellCastingHandler(FightActor caster, SpellCastHandler castHandler);

        public event SpellCastingHandler SpellCasting;

        protected virtual void OnSpellCasting(SpellCastHandler castHandler)
        {
            SpellCasting?.Invoke(this, castHandler);
            SpellHistory.RegisterCastedSpell(castHandler.SpellLevel, Fight.GetOneFighter(castHandler.TargetedCell));
        }

        public event SpellCastingHandler SpellCasted;

        protected virtual void OnSpellCasted(SpellCastHandler castHandler)
        {
            if (castHandler.SpellLevel.Effects.All(effect => effect.EffectId != EffectsEnum.Effect_Invisibility) && VisibleState == VisibleStateEnum.INVISIBLE)
            {
                if (!IsInvisibleSpellCast(castHandler.Spell))
                {
                    if (!DispellInvisibilityBuff())
                        SetInvisibilityState(VisibleStateEnum.VISIBLE);
                }
                else
                {
                    Fight.ForEach(x => ActionsHandler.SendGameActionFightInvisibleDetectedMessage(x.Client, this, this), true);
                }
            }

            SpellCasted?.Invoke(this, castHandler);
        }

        public event Action<FightActor, SpellCastInformations> SpellCastFailed;

        protected virtual void OnSpellCastFailed(SpellCastInformations cast)
        {
            SpellCastFailed?.Invoke(this, cast);
        }

        public event Action<FightActor, WeaponTemplate, Cell, FightSpellCastCriticalEnum, bool> WeaponUsed;

        protected virtual void OnWeaponUsed(WeaponTemplate weapon, Cell cell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (VisibleState == VisibleStateEnum.INVISIBLE)
            {
                if (!DispellInvisibilityBuff())
                    SetInvisibilityState(VisibleStateEnum.VISIBLE);
            }

            WeaponUsed?.Invoke(this, weapon, cell, critical, silentCast);
        }

        public event Action<FightActor, Buff> BuffAdded;

        protected virtual void OnBuffAdded(Buff buff)
        {
            BuffAdded?.Invoke(this, buff);
        }

        public event Action<FightActor, Buff> BuffRemoved;

        protected virtual void OnBuffRemoved(Buff buff)
        {
            BuffRemoved?.Invoke(this, buff);
        }

        public event Action<FightActor, FightActor> Dead;

        protected virtual void OnDead(FightActor killedBy, bool passTurn = true)
        {
            TriggerBuffs(this, BuffTriggerType.OnDeath);

            if (passTurn)
                PassTurn();

            KillAllSummons();
            RemoveAndDispellAllBuffs();

            if (Fight.CheckFightEnd(false) && killedBy is CharacterFighter && (killedBy as CharacterFighter).Character.FatalBlows.Any())
            {
                var durationChance = Fight.GetFightDuration().TotalMinutes * 10;
                var rand = new AsyncRandom().Next(0, 100);

                if (durationChance > rand)
                {
                    Fight.ForEach(entry => ContextHandler.SendGameActionFightSpellCastMessage(entry.Client, (ActionsEnum) 2029, killedBy,
                        this, Cell, FightSpellCastCriticalEnum.NORMAL, false, new Spell((int) (killedBy as CharacterFighter).Character.FatalBlows.RandomElementOrDefault(), 1)));
                }
            }

            Dead?.Invoke(this, killedBy);
        }

        public delegate void FightPointsVariationHandler(FightActor actor, ActionsEnum action, FightActor source, FightActor target, short delta);

        public event FightPointsVariationHandler FightPointsVariation;

        public virtual void OnFightPointsVariation(ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            switch (action)
            {
                case ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE:
                    OnApUsed((short) (-delta));
                    break;
                case ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE:
                    OnMpUsed((short) (-delta));
                    break;
            }

            FightPointsVariation?.Invoke(this, action, source, target, delta);
        }


        public event Action<FightActor, short> ApUsed;

        protected virtual void OnApUsed(short amount)
        {
            ApUsed?.Invoke(this, amount);
        }

        public event Action<FightActor, short> MpUsed;

        protected virtual void OnMpUsed(short amount)
        {
            MpUsed?.Invoke(this, amount);
        }

        #endregion

        #region Constructor

        protected bool m_isUsingWeapon;

        protected FightActor(FightTeam team)
        {
            Team = team;
            VisibleState = VisibleStateEnum.VISIBLE;
            Loot = new FightLoot();
            SpellHistory = new SpellHistory(this);
            MovementHistory = new MovementHistory(this);

            Fight.FightStarted += OnFightStarted;
        }

        #endregion

        #region Properties

        public IFight Fight => Team.Fight;

        public FightTeam Team
        {
            get;
        }

        public FightTeam OpposedTeam => Team.OpposedTeam;

        public override ICharacterContainer CharacterContainer => Fight;

        public abstract ObjectPosition MapPosition
        {
            get;
        }

        public virtual bool IsReady
        {
            get;
            protected set;
        }

        public SpellHistory SpellHistory
        {
            get;
            protected set;
        }

        public MovementHistory MovementHistory
        {
            get;
            protected set;
        }

        public ObjectPosition TurnStartPosition
        {
            get;
            internal set;
        }

        public int TurnTime
        {
            get
            {
                var duration = FightConfiguration.TurnTime + ((Stats.AP.Base + Stats.AP.Equiped + Stats.MP.Base + Stats.MP.Equiped + TurnTimeReport) * 1000);
                return duration > FightConfiguration.MaxTurnTime ? FightConfiguration.MaxTurnTime : duration;
            }
        }

        public int TurnTimeReport
        {
            get;
            internal set;
        }

        public ObjectPosition FightStartPosition
        {
            get;
            internal set;
        }

        public bool NeedTelefragState
        {
            get;
            set;
        }

        public override bool BlockSight => IsAlive() && VisibleState != VisibleStateEnum.INVISIBLE;

        public bool IsSacrificeProtected
        {
            get;
            set;
        }

        public virtual bool IsVisibleInTimeline => CanPlay();

        public bool IsRevived
        {
            get;
            private set;
        }

        public FightActor RevivedBy
        {
            get;
            private set;
        }

        #region Stats

            public abstract 

        byte Level
        {
            get;
        }

        public int LifePoints
        {
            get { return Stats.Health.TotalSafe; }
        }

        public int MaxLifePoints
        {
            get { return Stats.Health.TotalMax; }
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

        public void SwapPrePlacement(FightActor actor)
        {
            var oldCell = Position.Cell;

            Position.Cell = actor.Cell;
            actor.Position.Cell = oldCell;

            OnPrePlacementSwapped(actor);
        }

        public virtual ObjectPosition GetLeaderBladePosition() => MapPosition.Clone();

        #endregion

        #region Turn

        private void OnFightStarted(IFight obj)
        {
            OnGetAlive();
        }

        public void PassTurn()
        {
            if (!IsFighterTurn() || Fight.Freezed)
                return;

            Fight.StopTurn();

            OnTurnPassed();
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

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, this, this, (short) (-amount));

            return true;
        }

        public virtual bool UseMP(short amount)
        {
            if (Stats[PlayerFields.MP].Total - amount < 0)
                amount = (short) Stats[PlayerFields.MP].Total;

            Stats.MP.Used += amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE, this, this, (short) (-amount));

            return true;
        }

        public virtual bool LostAP(short amount, FightActor source)
        {
            if (Stats[PlayerFields.AP].Total - amount < 0)
                amount = (short) Stats[PlayerFields.AP].Total;

            Stats.AP.Used += amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_LOST, source, this, (short) (-amount));

            return true;
        }

        public virtual bool LostMP(short amount, FightActor source)
        {
            Stats.MP.Used += amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST, source, this, (short) (-amount));

            return true;
        }

        public virtual bool RegainAP(short amount)
        {
            Stats.AP.Used -= amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_WIN, this, this, amount);

            return true;
        }

        public virtual bool RegainMP(short amount)
        {
            Stats.MP.Used -= amount;

            OnFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_WIN, this, this, amount);

            return true;
        }

        public virtual void ResetUsedPoints()
        {
            Stats.AP.Used = 0;
            Stats.MP.Used = 0;
        }

        public SpellCastResult CanCastSpell(Spell spell, Cell cell, SpellCastResult[] ignored = null) => CanCastSpell(new SpellCastInformations(this, spell, cell));

        public virtual SpellCastResult CanCastSpell(SpellCastInformations cast)
        {
            if (cast.Force)
                return SpellCastResult.OK;

            if (!cast.IsConditionBypassed(SpellCastResult.CANNOT_PLAY) && (!IsFighterTurn() || IsDead()))
            {
                return SpellCastResult.CANNOT_PLAY;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.HAS_NOT_SPELL) && !HasSpell(cast.Spell.Id))
            {
                return SpellCastResult.HAS_NOT_SPELL;
            }

            var spellLevel = cast.SpellLevel;

            if (!cast.IsConditionBypassed(SpellCastResult.UNWALKABLE_CELL) && (!cast.TargetedCell.Walkable || cast.TargetedCell.NonWalkableDuringFight))
            {
                return SpellCastResult.UNWALKABLE_CELL;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.NOT_ENOUGH_AP) && (AP < spellLevel.ApCost))
            {
                return SpellCastResult.NOT_ENOUGH_AP;
            }

            var cellfree = Fight.IsCellFree(cast.TargetedCell);
            if (!cast.IsConditionBypassed(SpellCastResult.CELL_NOT_FREE) &&
                ((spellLevel.NeedFreeCell && !cellfree) || (spellLevel.NeedTakenCell && cellfree)))
            {
                return SpellCastResult.CELL_NOT_FREE;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.STATE_FORBIDDEN) && spellLevel.StatesForbidden.Any(HasState))
            {
                return SpellCastResult.STATE_FORBIDDEN;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.STATE_REQUIRED) &&
                spellLevel.StatesRequired.Any(state => !HasState(state)))
            {
                return SpellCastResult.STATE_REQUIRED;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.NOT_IN_ZONE) &&
                !IsInCastZone(spellLevel, cast.CastCell, cast.TargetedCell))
            {
                return SpellCastResult.NOT_IN_ZONE;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.HISTORY_ERROR) &&
                !SpellHistory.CanCastSpell(spellLevel, cast.TargetedCell))
            {
                return SpellCastResult.HISTORY_ERROR;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.NO_LOS) &&
                (cast.SpellLevel.CastTestLos && !Fight.CanBeSeen(cast.Caster.Cell, cast.TargetedCell)))
            {
                return SpellCastResult.NO_LOS;
            }

            return SpellCastResult.OK;
        }

        public bool IsInCastZone(SpellLevelTemplate spellLevel, MapPoint castCell, MapPoint cell)
        {
            var range = (int) spellLevel.Range;
            Set set;

            if (spellLevel.RangeCanBeBoosted)
            {
                range += Stats[PlayerFields.Range].Total;

                if (range < spellLevel.MinRange)
                    range = (int) spellLevel.MinRange;

                range = Math.Min(range, 63);
            }

            if (spellLevel.CastInDiagonal || spellLevel.CastInLine)
            {
                set = new CrossSet(castCell, range, (int) spellLevel.MinRange)
                {
                    AllDirections = spellLevel.CastInDiagonal && spellLevel.CastInLine,
                    Diagonal = spellLevel.CastInDiagonal
                };
            }
            else
            {
                set = new LozengeSet(castCell, range, (int) spellLevel.MinRange);
            }

            return set.BelongToSet(cell);
        }

        public virtual Set GetCastZoneSet(SpellLevelTemplate spellLevel, MapPoint castCell)
        {
            var range = spellLevel.Range;
            Set shape;

            if (spellLevel.RangeCanBeBoosted)
            {
                range += (uint) Stats[PlayerFields.Range].Total;

                if (range < spellLevel.MinRange)
                    range = spellLevel.MinRange;

                range = Math.Min(range, 63);
            }

            if (spellLevel.CastInDiagonal && spellLevel.CastInLine)
            {
                shape = new CrossSet(castCell, (int) range, (int) spellLevel.MinRange)
                {
                    AllDirections = true
                };
            }
            else if (spellLevel.CastInLine)
            {
                shape = new CrossSet(castCell, (int) range, (int) spellLevel.MinRange);
            }
            else if (spellLevel.CastInDiagonal)
            {
                shape = new CrossSet(castCell, (int) range, (int) spellLevel.MinRange)
                {
                    Diagonal = true
                };
            }
            else
            {
                shape = new LozengeSet(castCell, (int) range, (int) spellLevel.MinRange);
            }

            return shape;
        }

        public int GetSpellRange(SpellLevelTemplate spell) => (int) (spell.Range + (spell.RangeCanBeBoosted ? Stats[PlayerFields.Range].Total : 0));

        public bool CastAutoSpell(Spell spell, Cell cell)
        {
            return CastSpell(new SpellCastInformations(this, spell, cell) {Force = true, Silent = true, ApFree = true});
        }

        public bool CastSpell(Spell spell, Cell cell, params SpellCastResult[] ignored)
        {
            return CastSpell(new SpellCastInformations(this, spell, cell) {BypassedConditions = ignored});
        }

        public bool CastSpell(Spell spell, Cell cell)
        {
            return CastSpell(new SpellCastInformations(this, spell, cell));
        }

        public virtual bool CastSpell(SpellCastInformations cast)
        {
            if (!cast.Force && (!IsFighterTurn() || IsDead()))
                return false;

            var spellLevel = cast.SpellLevel;

            if (CanCastSpell(cast) != SpellCastResult.OK)
            {
                OnSpellCastFailed(cast);
                return false;
            }

            using (Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL))
            {
                cast.Critical = RollCriticalDice(spellLevel);

                if (cast.Critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                {
                    OnSpellCasting(new DefaultSpellCastHandler(cast));

                    if (!cast.ApFree)
                        UseAP((short) spellLevel.ApCost);


                    if (spellLevel.CriticalFailureEndsTurn)
                        PassTurn();

                    return false;
                }

                var handler = SpellManager.Instance.GetSpellCastHandler(cast);

                if (!handler.Initialize())
                {
                    OnSpellCastFailed(cast);

                    return false;
                }

                OnSpellCasting(handler);
                if (!cast.ApFree)
                    UseAP((short) spellLevel.ApCost);


                handler.Execute();

                OnSpellCasted(handler);
            }

            return true;
        }

        public SpellReflectionBuff GetBestReflectionBuff()
            => m_buffList.OfType<SpellReflectionBuff>().
                OrderByDescending(entry => entry.ReflectedLevel).
                FirstOrDefault();

        public void Die()
        {
            DamageTaken += LifePoints;
            OnDead(this);
        }

        public void Die(FightActor killedBy)
        {
            DamageTaken += LifePoints;
            OnDead(killedBy);
        }

        public int InflictDirectDamage(int damage, FightActor from)
            => InflictDamage(new Damage(damage)
            {
                Source = from,
                School = EffectSchoolEnum.Unknown,
                IgnoreDamageBoost = true,
                IgnoreDamageReduction = true
            });

        public int InflictDirectDamage(int damage) => InflictDirectDamage(damage, this);

        void TriggerDamageBuffs(Damage damage)
        {
            if (damage.School != EffectSchoolEnum.Pushback && !damage.ReflectedDamages && !IsIndirectSpellCast(damage.Spell))
                TriggerBuffs(damage.Source, BuffTriggerType.OnDamaged, damage);

            TriggerBuffs(damage.Source, damage.Source.IsEnnemyWith(this) ? BuffTriggerType.OnDamagedByEnemy : BuffTriggerType.OnDamagedByAlly, damage);

            if (damage.Source.IsSummoned())
                TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedBySummon, damage);

            switch (damage.School)
            {
                case EffectSchoolEnum.Neutral:
                    TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedNeutral, damage);
                    break;
                case EffectSchoolEnum.Earth:
                    TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedEarth, damage);
                    break;
                case EffectSchoolEnum.Water:
                    TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedWater, damage);
                    break;
                case EffectSchoolEnum.Air:
                    TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedAir, damage);
                    break;
                case EffectSchoolEnum.Fire:
                    TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedFire, damage);
                    break;
                case EffectSchoolEnum.Pushback:
                    {
                        TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedByPush, damage);

                        if (damage.Source.IsEnnemyWith(this))
                        {
                            TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedByEnemyPush, damage);

                            if (this is ICreature)
                                damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.OnDamageEnemyByPush, damage);
                        }
                        break;
                    }
            }

            TriggerBuffs(damage.Source, damage.Source.Position.Point.ManhattanDistanceTo(Position.Point) <= 1 ? BuffTriggerType.OnDamagedInCloseRange : BuffTriggerType.OnDamagedInLongRange, damage);
            TriggerBuffs(damage.Source, damage.IsWeaponAttack ? BuffTriggerType.OnDamagedByWeapon : BuffTriggerType.OnDamagedBySpell, damage);

            if (damage.MarkTrigger is Trap)
                TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedByTrap, damage);

            if (damage.MarkTrigger is Glyph)
                TriggerBuffs(damage.Source, BuffTriggerType.OnDamagedByGlyph, damage);
        }

        public virtual int InflictDamage(Damage damage)
        {
            var isCloseRangeAttack = damage.Source.Position.Point.ManhattanDistanceTo(Position.Point) <= 1;

            OnBeforeDamageInflicted(damage);
            damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.BeforeAttack, damage);
            TriggerBuffs(damage.Source, BuffTriggerType.BeforeDamaged, damage);

            if (damage.IsCritical)
                damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.OnCriticalHit, damage);

            damage.GenerateDamages();

            if (damage.Source != null && !damage.Source.CanDealDamage())
            {
                damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.AfterAttack, damage);
                TriggerBuffs(damage.Source, BuffTriggerType.AfterDamaged, damage);
                return 0;
            }

            if (IsInvulnerable(isCloseRangeAttack))
            {
                OnDamageReducted(damage.Source, damage.Amount);
                TriggerDamageBuffs(damage);
                damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.AfterAttack, damage);
                TriggerBuffs(damage.Source, BuffTriggerType.AfterDamaged, damage);
                return 0;
            }

            if (damage.Source != null && !damage.IgnoreDamageBoost)
            {
                if (damage.Spell != null)
                    damage.Amount += damage.Source.GetSpellBoost(damage.Spell);

                damage.Source.CalculateDamageBonuses(damage);
            }

            // zone damage
            if (damage.TargetCell != null && damage.Zone != null)
            {
                var efficiency = GetShapeEfficiency(damage.TargetCell, Position.Point, damage.Zone);

                damage.Amount = (int) (damage.Amount * efficiency);
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

                // removed
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
                        var reflectedDamage = new Damage(reflected)
                        {
                            ReflectedDamages = true,
                            Source = this,
                            School = EffectSchoolEnum.Unknown,
                            IgnoreDamageBoost = true,
                            IgnoreDamageReduction = true
                        };

                        damage.Source.InflictDamage(reflectedDamage);
                        OnDamageReflected(damage.Source);
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

            TriggerDamageBuffs(damage);

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

            OnLifePointsChanged(-damage.Amount, shieldDamages, permanentDamages, damage.Source, damage.School);

            CheckDead(damage.Source);

            OnDamageInflicted(damage);

            if (damage.Source != null)
                damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.AfterAttack, damage);

            TriggerBuffs(damage.Source, BuffTriggerType.AfterDamaged, damage);

            return damage.Amount;
        }


        public virtual int Heal(int amount, FightActor source, bool ignoreBonus = false)
        {
            return Heal(new Damage(amount) {Source = source, School = EffectSchoolEnum.Healing, IgnoreDamageBoost = ignoreBonus});
        }

        public virtual int Heal(Damage damage)
        {
            if (!damage.Generated)
                damage.GenerateDamages();

            if (damage.Source != null && !damage.IgnoreDamageBoost)
                damage.Amount = damage.Source.CalculateHeal(damage.Amount);

            if (damage.Source != null)
            {
                TriggerBuffs(damage.Source, BuffTriggerType.OnHealed, damage);
                damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.OnHeal, damage);
            }

            if (!CanBeHeal())
            {
                OnLifePointsChanged(0, 0, 0, damage.Source, EffectSchoolEnum.Unknown);
                return 0;
            }

            if (damage.Amount < 0)
                return 0;

            if (damage.TargetCell != null && damage.Zone != null)
            {
                var efficiency = GetShapeEfficiency(damage.TargetCell, Position.Point, damage.Zone);
                damage.Amount = (int) (damage.Amount * efficiency);
            }

            if (LifePoints + damage.Amount > MaxLifePoints)
                damage.Amount = MaxLifePoints - LifePoints;

            DamageTaken -= damage.Amount;

            OnLifePointsChanged(damage.Amount, 0, 0, damage.Source, EffectSchoolEnum.Unknown);

            if (damage.Source != null)
            {
                TriggerBuffs(damage.Source, BuffTriggerType.AfterHealed);
                damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.AfterHeal);
            }

            return damage.Amount;
        }

        public void Revive(int percentHp, FightActor caster)
        {
            foreach (var stat in Stats.Fields)
                stat.Value.Context = 0;

            var healAmount = (int) (MaxLifePoints * (percentHp / 100.0));

            if (healAmount <= 0)
                healAmount = 1;

            DamageTaken = Stats.Health.TotalMax - healAmount;

            IsRevived = true;
            RevivedBy = caster;

            OnGetAlive();
        }

        public void ExchangePositions(FightActor with)
        {
            var cell = Cell;

            Cell = with.Cell;
            with.Cell = cell;

            ActionsHandler.SendGameActionFightExchangePositionsMessage(Fight.Clients, this, with);
        }

        #region Formulas

        public virtual Damage CalculateDamageBonuses(Damage damage)
        {
            // formulas :
            // DAMAGE * [(100 + STATS + %BONUS + MULT)/100 + (BONUS + PHS/MGKBONUS + ELTBONUS)]

            var bonusPercent = Stats[PlayerFields.DamageBonusPercent].TotalSafe;
            var mult = Stats[PlayerFields.DamageMultiplicator].Total;
            var bonus = Stats[PlayerFields.DamageBonus].Total;
            var criticalBonus = 0;
            var phyMgkBonus = 0;
            var stats = 0;
            var eltBonus = 0;
            var weaponBonus = 0;
            var spellBonus = 0;

            if (m_isUsingWeapon)
                weaponBonus = Stats[PlayerFields.WeaponDamageBonus].TotalSafe;
            else
                spellBonus = Stats[PlayerFields.SpellDamageBonus].TotalSafe;

            switch (damage.School)
            {
                case EffectSchoolEnum.Neutral:
                    stats = Stats[PlayerFields.Strength].TotalSafe;
                    phyMgkBonus = Stats[PlayerFields.PhysicalDamage].TotalSafe;
                    eltBonus = Stats[PlayerFields.NeutralDamageBonus].Total;
                    break;
                case EffectSchoolEnum.Earth:
                    stats = Stats[PlayerFields.Strength].TotalSafe;
                    phyMgkBonus = Stats[PlayerFields.PhysicalDamage].TotalSafe;
                    eltBonus = Stats[PlayerFields.EarthDamageBonus].Total;
                    break;
                case EffectSchoolEnum.Air:
                    stats = Stats[PlayerFields.Agility].TotalSafe;
                    phyMgkBonus = Stats[PlayerFields.MagicDamage].TotalSafe;
                    eltBonus = Stats[PlayerFields.AirDamageBonus].Total;
                    break;
                case EffectSchoolEnum.Water:
                    stats = Stats[PlayerFields.Chance].TotalSafe;
                    phyMgkBonus = Stats[PlayerFields.MagicDamage].TotalSafe;
                    eltBonus = Stats[PlayerFields.WaterDamageBonus].Total;
                    break;
                case EffectSchoolEnum.Fire:
                    stats = Stats[PlayerFields.Intelligence].TotalSafe;
                    phyMgkBonus = Stats[PlayerFields.MagicDamage].TotalSafe;
                    eltBonus = Stats[PlayerFields.FireDamageBonus].Total;
                    break;
            }

            if (damage.IsCritical)
                criticalBonus += Stats[PlayerFields.CriticalDamageBonus].Total;

            if (damage.MarkTrigger is Glyph)
                bonusPercent += Stats[PlayerFields.GlyphBonusPercent].TotalSafe;

            if (damage.MarkTrigger is Trap)
                bonusPercent += Stats[PlayerFields.TrapBonusPercent].TotalSafe;

            damage.Amount = (int) Math.Max(0, (damage.Amount * (100 + stats + bonusPercent + weaponBonus + spellBonus) / 100d
                                               + (bonus + criticalBonus + phyMgkBonus + eltBonus)) * ((100 + mult) / 100.0));

            return damage;
        }

        public virtual double GetShapeEfficiency(MapPoint targetCell, MapPoint impactCell, Zone zone)
        {
            if (zone.Radius >= UNLIMITED_ZONE_SIZE)
                return 1.0;

            uint distance = 0;
            switch (zone.ShapeType)
            {
                case SpellShapeEnum.A:
                case SpellShapeEnum.a:
                case SpellShapeEnum.Z:
                case SpellShapeEnum.I:
                case SpellShapeEnum.O:
                case SpellShapeEnum.semicolon:
                case SpellShapeEnum.empty:
                case SpellShapeEnum.P:
                    return 1.0;
                case SpellShapeEnum.B:
                case SpellShapeEnum.V:
                case SpellShapeEnum.G:
                case SpellShapeEnum.W:
                    distance = targetCell.SquareDistanceTo(impactCell);
                    break;
                case SpellShapeEnum.minus:
                case SpellShapeEnum.plus:
                case SpellShapeEnum.U:
                    distance = targetCell.ManhattanDistanceTo(impactCell) / 2;
                    break;
                default:
                    distance = targetCell.ManhattanDistanceTo(impactCell);
                    break;
            }

            if (distance > zone.Radius)
                return 1.0;

            if (zone.MinRadius > 0)
            {
                if (distance <= zone.MinRadius)
                    return 1.0;

                return Math.Max(0d, 1 - 0.01 * Math.Min(distance - zone.MinRadius, zone.MaxEfficiency) * zone.EfficiencyMalus);
            }

            return Math.Max(0d, 1 - 0.01 * Math.Min(distance, zone.MaxEfficiency) * zone.EfficiencyMalus);
        }

        public virtual int CalculateDamageResistance(int damage, EffectSchoolEnum type, bool critical, bool withArmor, bool poison)
        {
            var percentResistance = CalculateTotalResistances(type, true, poison);
            var fixResistance = CalculateTotalResistances(type, false, poison);

            percentResistance = percentResistance > StatsFields.ResistanceLimit ? StatsFields.ResistanceLimit : percentResistance;
            fixResistance = fixResistance > StatsFields.ResistanceLimit ? StatsFields.ResistanceLimit : fixResistance;

            var result = (int) ((1 - percentResistance / 100d) * (damage - fixResistance)) -
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
                        return Stats[PlayerFields.NeutralResistPercent].Base + Stats[PlayerFields.NeutralResistPercent].Equiped + Stats[PlayerFields.NeutralResistPercent].Given +
                               (poison ? 0 : Stats[PlayerFields.NeutralResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpNeutralResistPercent].Total : 0);

                    return Stats[PlayerFields.NeutralElementReduction].Base + Stats[PlayerFields.NeutralElementReduction].Equiped + Stats[PlayerFields.NeutralElementReduction].Given +
                           (poison ? 0 : Stats[PlayerFields.NeutralElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpNeutralElementReduction].Total : 0) +
                           Stats[PlayerFields.PhysicalDamageReduction];
                case EffectSchoolEnum.Earth:
                    if (percent)
                        return Stats[PlayerFields.EarthResistPercent].Base + Stats[PlayerFields.EarthResistPercent].Equiped + Stats[PlayerFields.EarthResistPercent].Given +
                               (poison ? 0 : Stats[PlayerFields.EarthResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpEarthResistPercent].Total : 0);

                    return Stats[PlayerFields.EarthElementReduction].Base + Stats[PlayerFields.EarthElementReduction].Equiped + Stats[PlayerFields.EarthElementReduction].Given +
                           (poison ? 0 : Stats[PlayerFields.EarthElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpEarthElementReduction].Total : 0) +
                           Stats[PlayerFields.PhysicalDamageReduction];
                case EffectSchoolEnum.Air:
                    if (percent)
                        return Stats[PlayerFields.AirResistPercent].Base + Stats[PlayerFields.AirResistPercent].Equiped + Stats[PlayerFields.AirResistPercent].Given +
                               (poison ? 0 : Stats[PlayerFields.AirResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpAirResistPercent].Total : 0);

                    return Stats[PlayerFields.AirElementReduction].Base + Stats[PlayerFields.AirElementReduction].Equiped + Stats[PlayerFields.AirElementReduction].Given +
                           (poison ? 0 : Stats[PlayerFields.AirElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpAirElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                case EffectSchoolEnum.Water:
                    if (percent)
                        return Stats[PlayerFields.WaterResistPercent].Base + Stats[PlayerFields.WaterResistPercent].Equiped + Stats[PlayerFields.WaterResistPercent].Given +
                               (poison ? 0 : Stats[PlayerFields.WaterResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpWaterResistPercent].Total : 0);

                    return Stats[PlayerFields.WaterElementReduction].Base + Stats[PlayerFields.WaterElementReduction].Equiped + Stats[PlayerFields.WaterElementReduction].Given +
                           (poison ? 0 : Stats[PlayerFields.WaterElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpWaterElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                case EffectSchoolEnum.Fire:
                    if (percent)
                        return Stats[PlayerFields.FireResistPercent].Base + Stats[PlayerFields.FireResistPercent].Equiped + Stats[PlayerFields.FireResistPercent].Given +
                               (poison ? 0 : Stats[PlayerFields.FireResistPercent].Context) + (pvp ? Stats[PlayerFields.PvpFireResistPercent].Total : 0);

                    return Stats[PlayerFields.FireElementReduction].Base + Stats[PlayerFields.FireElementReduction].Equiped + Stats[PlayerFields.FireElementReduction].Given +
                           (poison ? 0 : Stats[PlayerFields.FireElementReduction].Context) + (pvp ? Stats[PlayerFields.PvpFireElementReduction].Total : 0) + Stats[PlayerFields.MagicDamageReduction];
                default:
                    return 0;
            }
        }

        public virtual int CalculateDamageReflection(int damage)
        {
            // only spell damage reflection are mutlplied by wisdom
            var reflectDamages = Stats[PlayerFields.DamageReflection].Context * (1 + (Stats[PlayerFields.Wisdom].TotalSafe / 100)) +
                                 (Stats[PlayerFields.DamageReflection].TotalSafe - Stats[PlayerFields.DamageReflection].Context);

            if (reflectDamages > damage / 2d)
                return (int) (damage / 2d);

            return reflectDamages;
        }

        public virtual int CalculateHeal(int heal)
        {
            return (int) (heal * (100 + Stats[PlayerFields.Intelligence].TotalSafe) / 100d + Stats[PlayerFields.HealBonus].TotalSafe);
        }

        public virtual int CalculateArmorValue(int reduction)
        {
            return (int) (reduction * (100 + 5 * Level) / 100d);
        }

        public virtual int CalculateErosionDamage(int damages)
        {
            var erosion = Stats[PlayerFields.Erosion].TotalSafe;

            if (erosion > 50)
                erosion = 50;

            if (GetBuffs(x => x.Spell.Id == (int) SpellIdEnum.TRÊVE).Any())
                erosion -= 10;

            return (int) (damages * (erosion / 100d));
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

        public virtual FightSpellCastCriticalEnum RollCriticalDice(SpellLevelTemplate spell)
        {
            var random = new AsyncRandom();

            var critical = FightSpellCastCriticalEnum.NORMAL;

            if (spell.CriticalFailureProbability != 0 && random.NextDouble() * 100 < spell.CriticalFailureProbability + Stats[PlayerFields.CriticalMiss].Total)
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;

            else if (spell.CriticalHitProbability != 0 && random.NextDouble() * 100 < spell.CriticalHitProbability + Stats[PlayerFields.CriticalHit].Total)
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            var token = new Ref<FightSpellCastCriticalEnum>(critical);
            TriggerBuffs(this, BuffTriggerType.AfterRollCritical, token);

            critical = token.Target;

            return critical;
        }

        public virtual FightSpellCastCriticalEnum RollCriticalDice(WeaponTemplate weapon)
        {
            var random = new AsyncRandom();

            var critical = FightSpellCastCriticalEnum.NORMAL;

            if (weapon.CriticalHitProbability != 0 && random.NextDouble() * 100 < weapon.CriticalFailureProbability + Stats[PlayerFields.CriticalMiss])
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;
            else if (weapon.CriticalHitProbability != 0 &&
                     random.NextDouble() * 100 < weapon.CriticalHitProbability + Stats[PlayerFields.CriticalHit])
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            var token = new Ref<FightSpellCastCriticalEnum>(critical);
            TriggerBuffs(this, BuffTriggerType.AfterRollCritical, token);

            critical = token.Target;

            return critical;
        }

        public virtual int CalculateReflectedDamageBonus(int spellBonus)
        {
            return (int) (spellBonus * (1 + (Stats[PlayerFields.Wisdom].TotalSafe / 100d)) + Stats[PlayerFields.DamageReflection].TotalSafe);
        }

        public virtual bool RollAPLose(FightActor from, int value)
        {
            var apAttack = from.Stats[PlayerFields.APAttack].Total > 1 ? from.Stats[PlayerFields.APAttack].TotalSafe : 1;
            var apDodge = Stats[PlayerFields.DodgeAPProbability].Total > 1 ? Stats[PlayerFields.DodgeAPProbability].TotalSafe : 1;
            var prob = ((Stats.AP.Total - value) / (double) (Stats.AP.TotalMax)) * (apAttack / (double) apDodge) / 2d;

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
            var prob = ((Stats.MP.Total - value) / (double) (Stats.MP.TotalMax)) * (mpAttack / (double) mpDodge) / 2d;

            if (prob < 0.10)
                prob = 0.10;
            else if (prob > 0.90)
                prob = 0.90 - (0.10 * value);

            var rnd = new CryptoRandom().NextDouble();

            return rnd < prob;
        }

        public FightActor[] GetTacklers()
        {
            return GetTacklers(Position.Cell);
        }

        public FightActor[] GetTacklers(Cell cell)
        {
            return OpposedTeam.GetAllFighters(entry => entry.CanTackle(this) && entry.Position.Point.IsAdjacentTo(cell)).ToArray();
        }


        public int GetTackledMP()
        {
            return GetTackledMP(MP, Cell);
        }

        public virtual int GetTackledMP(int mp, Cell cell)
        {
            if (VisibleState != GameActionFightInvisibilityStateEnum.VISIBLE)
                return 0;

            if (HasState((int) SpellStatesEnum.INTACLABLE_96))
                return 0;

            return (int) Math.Round(mp * (1 - GetTacklePercent(cell)));
        }

        public int GetTackledAP()
        {
            return GetTackledAP(AP, Cell);
        }

        public virtual int GetTackledAP(int ap, Cell cell)
        {
            if (VisibleState != GameActionFightInvisibilityStateEnum.VISIBLE)
                return 0;

            if (HasState((int) SpellStatesEnum.INTACLABLE_96))
                return 0;

            return (int) Math.Round(ap * (1 - GetTacklePercent(cell)));
        }

        private double GetTacklePercent()
        {
            return GetTacklePercent(Cell);
        }

        private double GetTacklePercent(Cell cell)
        {
            var tacklers = GetTacklers(cell);

            // no tacklers, then no tackle possible
            if (tacklers.Length <= 0)
                return 1d;

            var percentRemaining = tacklers.Aggregate(1d, (current, fightActor) => current * GetSingleTacklerPercent(fightActor));

            if (percentRemaining < 0)
                percentRemaining = 0d;
            else if (percentRemaining > 1)
                percentRemaining = 1;

            return percentRemaining;
        }

        private double GetSingleTacklerPercent(IStatsOwner tackler)
        {
            var tackleBlock = tackler.Stats[PlayerFields.TackleBlock].TotalSafe;
            var tackleEvade = Stats[PlayerFields.TackleEvade].TotalSafe;

            return Math.Max(0, Math.Min(1, (tackleEvade + 2) / ((2d * (tackleBlock + 2)))));
        }

        #endregion

        #endregion

        #region Buffs

        readonly Dictionary<Spell, short> m_buffedSpells = new Dictionary<Spell, short>();
        readonly UniqueIdProvider m_buffIdProvider = new UniqueIdProvider();
        readonly List<Buff> m_buffList = new List<Buff>();
        public ReadOnlyCollection<Buff> Buffs => m_buffList.AsReadOnly();

        public int PopNextBuffId() => m_buffIdProvider.Pop();

        public void FreeBuffId(int id)
        {
            m_buffIdProvider.Push(id);
        }

        public IEnumerable<Buff> GetBuffs() => m_buffList;

        public IEnumerable<Buff> GetBuffs(Predicate<Buff> predicate) => m_buffList.Where(entry => predicate(entry));

        public virtual bool CanAddBuff(Buff buff) => true;

        public bool BuffMaxStackReached(Buff buff)
            => buff.Spell.CurrentSpellLevel.MaxStack > 0
               && buff.Spell.CurrentSpellLevel.MaxStack <= m_buffList.Count(entry => entry.Spell.Id == buff.Spell.Id
                                                                                     && entry.Effect.Id == buff.Effect.Id
                                                                                     && (buff.Critical || entry.Critical || Enumerable.SequenceEqual(entry.Effect.GetValues(), buff.Effect.GetValues()))
                                                                                     && entry.GetType().Name == buff.GetType().Name
                                                                                     && buff.Delay == 0);

        public bool AddBuff(Buff buff, bool bypassMaxStack = false)
        {
            if (!CanAddBuff(buff))
            {
                FreeBuffId(buff.Id);
                return false;
            }

            if (!bypassMaxStack && BuffMaxStackReached(buff))
            {
                var oldBuff = m_buffList.Where(x => x.Spell.Id == buff.Spell.Id
                                                    && x.Effect.EffectId == buff.Effect.EffectId
                                                    && (buff.Critical || x.Critical || Enumerable.SequenceEqual(x.Effect.GetValues(), buff.Effect.GetValues()))
                                                    && x.GetType() == buff.GetType()).OrderBy(x => x.Duration).FirstOrDefault();

                if (oldBuff == null)
                {
                    FreeBuffId(buff.Id);
                    return false;
                }

                RemoveBuff(oldBuff);
            }

            m_buffList.Add(buff);

            if (buff.Delay == 0)
                buff.Apply();

            OnBuffAdded(buff);

            return true;
        }

        public void RemoveBuff(Buff buff)
        {
            if (buff.Applied)
                buff.Dispell();

            m_buffList.Remove(buff);

            OnBuffRemoved(buff);

            FreeBuffId(buff.Id);
        }

        public void RemoveSpellBuffs(int spellId)
        {
            foreach (var buff in m_buffList.Where(x => x.Spell.Id == spellId || (x as TriggerBuff)?.ParentSpell.Id == spellId).ToArray())
            {
                RemoveBuff(buff);
                ActionsHandler.SendGameActionFightDispellSpellMessage(Fight.Clients, this, this, spellId);
            }
        }

        public void RemoveAndDispellAllBuffs(FightDispellableEnum dispellable = FightDispellableEnum.REALLY_NOT_DISPELLABLE)
        {
            foreach (var buff in m_buffList.Where(x => x.Dispellable <= dispellable).ToArray())
            {
                RemoveBuff(buff);
            }

            TriggerBuffs(this, BuffTriggerType.OnDispelled);
        }

        public void RemoveAndDispellAllBuffs(FightActor caster, FightDispellableEnum dispellable = FightDispellableEnum.REALLY_NOT_DISPELLABLE)
        {
            var copyOfBuffs = m_buffList.ToArray();

            foreach (var buff in copyOfBuffs.Where(buff => buff.Caster == caster && buff.Dispellable <= dispellable))
            {
                RemoveBuff(buff);
            }
        }

        public void RemoveAllCastedBuffs(FightDispellableEnum dispellable = FightDispellableEnum.REALLY_NOT_DISPELLABLE)
        {
            foreach (var fighter in Fight.GetAllFighters())
            {
                fighter.RemoveAndDispellAllBuffs(this, dispellable);
            }
        }

        public void TriggerBuffs(FightActor triggerer, BuffTriggerType trigger, object token = null)
        {
            foreach (var triggerBuff in m_buffList.OfType<TriggerBuff>().Where(buff => buff.ShouldTrigger(trigger, token)).OrderBy(x => x.Priority).ToArray())
            {
                using (Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED))
                    triggerBuff.Apply(triggerer, trigger, token);
            }
        }

        public void DecrementBuffsDuration(FightActor reference)
        {
            var buffsToRemove = m_buffList.ToArray().Where(buff => buff.DecrementReference == reference && buff.DecrementDuration()).OrderBy(x => x.Priority);

            foreach (var buff in buffsToRemove)
            {
                if (buff is TriggerBuff && (buff as TriggerBuff).ShouldTrigger(BuffTriggerType.OnBuffEnded))
                    (buff as TriggerBuff).Apply(this, BuffTriggerType.OnBuffEnded);

                if (!(buff is TriggerBuff && (buff as TriggerBuff).ShouldTrigger(BuffTriggerType.OnBuffEndedTurnEnd)))
                    RemoveBuff(buff);
            }
        }

        public void TriggerBuffsRemovedOnTurnEnd()
        {
            foreach (var buff in m_buffList.OfType<TriggerBuff>().Where(entry => entry.Duration <= 0 &&
                                                                                 entry.ShouldTrigger(BuffTriggerType.OnBuffEndedTurnEnd)).ToArray())
            {
                buff.Apply(this, BuffTriggerType.OnBuffEndedTurnEnd);
                RemoveBuff(buff);
            }
        }

        /// <summary>
        /// Decrement the duration of all the buffs that the fighter casted.
        /// </summary>
        public void DecrementAllCastedBuffsDuration()
        {
            foreach (var fighter in Fight.GetAllFighters())
                fighter.DecrementBuffsDuration(this);
        }

        public void DecrementSummonsCastedBuffsDuration()
        {
            foreach (var summon in Summons.ToArray())
                summon.DecrementAllCastedBuffsDuration();

            foreach (var bomb in Bombs.ToArray())
                bomb.DecrementAllCastedBuffsDuration();
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

        public short GetSpellBoost(Spell spell) => !m_buffedSpells.ContainsKey(spell) ? (short) 0 : m_buffedSpells[spell];

        public virtual bool MustSkipTurn() => GetBuffs(x => x is SkipTurnBuff).Any();

        public bool IsImmuneToSpell(int id) => GetBuffs(x => x is SpellImmunityBuff && id == ((SpellImmunityBuff) x).SpellImmune).Any();

        #endregion

        #region Look

        public ActorLook OriginalLook
        {
            get;
            protected set;
        }

        public override ActorLook Look
        {
            get { return m_look; }
            set
            {
                if (OriginalLook == null)
                    OriginalLook = value;

                m_look = value;
            }
        }

        public void UpdateLook(FightActor source = null)
        {
            var skinBuff = Buffs.OfType<SkinBuff>().LastOrDefault(x => x.Applied);
            var rescaleBuff = Buffs.OfType<RescaleSkinBuff>().LastOrDefault(x => x.Applied);

            Look = skinBuff == null ? OriginalLook : skinBuff.Look;

            if (rescaleBuff != null)
                Look.Rescale(rescaleBuff.RescaleFactor);
            else
                Look.ResetScales();

            ActionsHandler.SendGameActionFightChangeLookMessage(Fight.Clients, source ?? this, this, Look);
        }

        #endregion

        #region Summons

        readonly List<SummonedFighter> m_summons = new List<SummonedFighter>();
        readonly List<SummonedBomb> m_bombs = new List<SummonedBomb>();

        public int SummonedCount
            => m_summons.Count(x => x is SummonedMonster && (x as SummonedMonster).Monster.Template.UseSummonSlot);

        public int BombsCount => m_bombs.Count(x => x.MonsterBombTemplate.Template.UseBombSlot);

        public ReadOnlyCollection<SummonedFighter> Summons => m_summons.AsReadOnly();

        public ReadOnlyCollection<SummonedBomb> Bombs => m_bombs.AsReadOnly();

        public FightActor Summoner
        {
            get;
            protected set;
        }

        public SpellEffectHandler SummoningEffect
        {
            get;
            set;
        }

        public virtual bool IsSummoned() => Summoner != null;

        public bool CanSummon() => SummonedCount < Stats[PlayerFields.SummonLimit].Total;

        public bool CanSummonBomb() => BombsCount < SummonedBomb.BombLimit;

        public void AddSummon(SummonedFighter summon)
        {
            m_summons.Add(summon);
        }

        public void RemoveSummon(SummonedFighter summon)
        {
            m_summons.Remove(summon);
        }

        public void AddBomb(SummonedBomb bomb)
        {
            m_bombs.Add(bomb);
        }

        public void RemoveBomb(SummonedBomb bomb)
        {
            m_bombs.Remove(bomb);
        }

        public void RemoveAllSummons()
        {
            m_summons.Clear();
            m_bombs.Clear();
        }

        public void KillAllSummons()
        {
            foreach (var summon in Fight.GetAllFighters().Where(x => x.Summoner == this || x.RevivedBy == this).ToArray())
            {
                summon.Die();
            }

            foreach (var bomb in m_bombs.ToArray())
            {
                bomb.Die();
            }
        }

        #endregion

        #region States

        public IEnumerable<StateBuff> GetStates() => GetBuffs(x => x is StateBuff && x.Delay == 0).Select(x => x as StateBuff);

        public bool HasState(int stateId)
            => GetStates().Any(x => x.State.Id == stateId && !x.IsDisabled);

        public bool HasState(SpellState state) => HasState(state.Id);

        public bool HasSpellBlockerState() => GetStates().Any(x => x.State.PreventsSpellCast);

        public bool HasFightBlockerState() => GetStates().Any(x => x.State.PreventsFight);

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

        public bool IsIndirectSpellCast(Spell spell)
        {
            if (spell == null)
                return false;

            return spell.Template.Id == (int) SpellIdEnum.EXPLOSION_SOURNOISE
                   || spell.Template.Id == (int) SpellIdEnum.EXPLOSION_DE_MASSE
                   || spell.Template.Id == (int) SpellIdEnum.PIÈGE_MORTEL_SRAM
                   || spell.Template.Id == (int) SpellIdEnum.CONCENTRATION_DE_CHAKRA
                   || spell.Template.Id == (int) SpellIdEnum.VERTIGE
                   || spell.Template.Id == (int) SpellIdEnum.SORT_ENFLAMMÉ
                   || spell.Template.Id == (int) SpellIdEnum.GLYPHE_AGRESSIF_1503
                   || spell.Template.Id == (int) SpellIdEnum.GLYPHE_RALENTISSANT
                   || spell.Template.Id == (int) SpellIdEnum.PULSE
                   || spell.Template.Id == (int) SpellIdEnum.CONTRE_94
                   || spell.Template.Id == (int) SpellIdEnum.MOT_TOURNOYANT
                   || spell.Template.Id == (int) SpellIdEnum.MOT_TOURNOYANT_DU_DOPEUL
                   || spell.Template.Id == (int) SpellIdEnum.MUR_DE_FEU
                   || spell.Template.Id == (int) SpellIdEnum.MUR_D_AIR
                   || spell.Template.Id == (int) SpellIdEnum.MUR_D_EAU
                   || spell.Template.Id == (int) SpellIdEnum.EXPLOSION_ROUBLARDE
                   || spell.Template.Id == (int) SpellIdEnum.AVERSE_ROUBLARDE
                   || spell.Template.Id == (int) SpellIdEnum.TORNADE_ROUBLARDE;
        }

        public bool IsPoisonSpellCast(Spell spell)
        {
            return spell.Template.Id == (int) SpellIdEnum.POISON_INSIDIEUX ||
                   spell.Template.Id == (int) SpellIdEnum.POISON_INSIDIEUX_DU_DOPEUL ||
                   spell.Template.Id == (int) SpellIdEnum.POISON_PARALYSANT ||
                   spell.Template.Id == (int) SpellIdEnum.POISON_PARALYSANT_DU_DOPEUL ||
                   spell.Template.Id == (int) SpellIdEnum.FLECHETTE_EMPOISONNÉE ||
                   spell.Template.Id == (int) SpellIdEnum.FLÈCHE_EMPOISONNÉE ||
                   spell.Template.Id == (int) SpellIdEnum.BROUILLARD_EMPOISONNÉ ||
                   spell.Template.Id == (int) SpellIdEnum.TOURBE_EMPOISONNÉE ||
                   spell.Template.Id == (int) SpellIdEnum.GRAINE_EMPOISONNÉE ||
                   spell.Template.Id == (int) SpellIdEnum.RONCE_EMPOISONNÉE ||
                   spell.Template.Id == (int) SpellIdEnum.PIÈGE_EMPOISONNÉ ||
                   spell.Template.Id == (int) SpellIdEnum.PIÈGE_EMPOISONNÉ_DU_DOPEUL ||
                   spell.Template.Id == (int) SpellIdEnum.VENT_EMPOISONNÉ ||
                   spell.Template.Id == (int) SpellIdEnum.VENT_EMPOISONNÉ_DU_DOPEUL ||
                   spell.Template.Id == (int) SpellIdEnum.TREMBLEMENT_181 ||
                   spell.Template.Id == (int) SpellIdEnum.RONCE_INSOLENTE_188 ||
                   spell.Template.Id == (int) SpellIdEnum.VERTIGE ||
                   spell.Template.Id == (int) SpellIdEnum.SILENCE_DU_SRAM;
        }

        public bool IsInvisibleSpellCast(Spell spell)
        {
            var spellLevel = spell.CurrentSpellLevel;

            if (!(this is CharacterFighter))
                return true;

            return spellLevel.Effects.Any(entry => entry.EffectId == EffectsEnum.Effect_Trap) || // traps
                   spellLevel.Effects.Any(entry => entry.EffectId == EffectsEnum.Effect_Summon) || // summons
                   spell.Template.Id == (int) SpellIdEnum.DOUBLE || // double
                   spell.Template.Id == (int) SpellIdEnum.PULSION_DE_CHAKRA || // chakra pulsion
                   spell.Template.Id == (int) SpellIdEnum.CONCENTRATION_DE_CHAKRA || // chakra concentration
                   spell.Template.Id == (int) SpellIdEnum.POISON_INSIDIEUX || // insidious poison
                   spell.Template.Id == (int) SpellIdEnum.PEUR || //Fear
                   spell.Template.Id == (int) SpellIdEnum.POISSE ||
                   spell.Template.Id == (int) SpellIdEnum.ROUBLARDISE ||
                   spell.Template.Id == (int) SpellIdEnum.DOFUS_NÉBULEUX ||
                   spell.Template.Id == (int) SpellIdEnum.DOFUS_DES_VEILLEURS ||
                   spell.Template.Id == (int) SpellIdEnum.DOFUS_TURQUOISE ||
                   spell.Template.Id == (int) SpellIdEnum.MAÎTRISE_D_ARME ||
                   spell.Template.Id == 6149; //DOKOKO
        }

        public bool DispellInvisibilityBuff()
        {
            var buffs = GetBuffs(entry => entry is InvisibilityBuff).ToArray();

            foreach (var buff in buffs)
            {
                RemoveBuff(buff);
            }

            return buffs.Any();
        }

        public VisibleStateEnum GetVisibleStateFor(FightActor fighter) => fighter.IsFriendlyWith(this) && VisibleState != VisibleStateEnum.VISIBLE
            ? VisibleStateEnum.DETECTED
            : VisibleState;

        public VisibleStateEnum GetVisibleStateFor(Character character)
        {
            if (!character.IsFighting() || character.Fight != Fight)
                return VisibleState;

            return character.Fighter.IsFriendlyWith(this) && VisibleState != VisibleStateEnum.VISIBLE ? VisibleStateEnum.DETECTED : VisibleState;
        }

        public bool IsVisibleFor(FightActor fighter) => GetVisibleStateFor(fighter) != VisibleStateEnum.INVISIBLE;

        public bool IsVisibleFor(Character character) => GetVisibleStateFor(character) != VisibleStateEnum.INVISIBLE;

        protected virtual void OnVisibleStateChanged(FightActor source, VisibleStateEnum lastState)
        {
            Fight.ForEach(entry => ActionsHandler.SendGameActionFightInvisibilityMessage(entry.Client, source, this, GetVisibleStateFor(entry)), true);

            if (lastState == VisibleStateEnum.INVISIBLE)
                Fight.ForEach(entry => ContextHandler.SendGameFightRefreshFighterMessage(entry.Client, this), true);
        }

        #endregion

        #region Carry/Throw

        FightActor m_carriedActor;
        private ActorLook m_look;

        public bool IsCarrying() => m_carriedActor != null;

        public bool IsCarried() => GetCarryingActor() != null;

        public FightActor GetCarriedActor() => m_carriedActor;

        public FightActor GetCarryingActor() => Fight.GetFirstFighter<FightActor>(x => x.GetCarriedActor() == this);

        public bool CarryActor(FightActor target, EffectBase effect, Spell spell, SpellCastHandler castHandler)
        {
            var stateCarried = SpellManager.Instance.GetSpellState((uint) SpellStatesEnum.PORTE_8);
            var stateCarrying = SpellManager.Instance.GetSpellState((uint) SpellStatesEnum.PORTEUR_3);

            if (HasState(stateCarrying) || HasState(stateCarried) || target.HasState(stateCarrying) || target.HasState(stateCarried))
                return false;

            if (target.HasState((int)SpellStatesEnum.LOURD_63))
                return false;

            if (target.GetStates().Where(x => !x.IsDisabled).Any(x => x.State.CantBeMoved))
                return false;

            var actorBuffId = PopNextBuffId();
            var targetBuffId = target.PopNextBuffId();

            var addStateHandler = new AddState(new EffectDice((short) EffectsEnum.Effect_AddState, (short) stateCarrying.Id, 0, 0, new EffectBase()), this, castHandler, Cell, false);
            var actorBuff = new StateBuff(actorBuffId, this, this, addStateHandler,
                spell, FightDispellableEnum.DISPELLABLE_BY_DEATH, stateCarrying)
            {
                Duration = -1000
            };

            addStateHandler = new AddState(new EffectDice((short) EffectsEnum.Effect_AddState, (short) stateCarrying.Id, 0, 0, new EffectBase()), target, castHandler, target.Cell, false);
            var targetBuff = new StateBuff(targetBuffId, target, this, addStateHandler,
                spell, FightDispellableEnum.DISPELLABLE_BY_DEATH, stateCarried)
            {
                Duration = -1000
            };

            AddBuff(actorBuff);
            target.AddBuff(targetBuff);

            ActionsHandler.SendGameActionFightCarryCharacterMessage(Fight.Clients, this, target);

            m_carriedActor = target;
            target.Position.Cell = Position.Cell;

            m_carriedActor.Dead += OnCarryingActorDead;
            Dead += OnCarryingActorDead;

            return true;
        }

        public void ThrowActor(Cell cell, bool drop = false)
        {
            var actor = Fight.GetOneFighter(cell);
            if (actor != null && !drop)
                return;

            if (m_carriedActor == null)
                return;

            var actorState = GetBuffs(x => x is StateBuff && (x as StateBuff).State.Id == (int) SpellStatesEnum.PORTEUR_3).FirstOrDefault();
            var targetState = m_carriedActor.GetBuffs(x => x is StateBuff && (x as StateBuff).State.Id == (int) SpellStatesEnum.PORTE_8).FirstOrDefault();

            using (Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE))
            {
                if (drop)
                    ActionsHandler.SendGameActionFightDropCharacterMessage(Fight.Clients, this, m_carriedActor, cell);
                else
                    ActionsHandler.SendGameActionFightThrowCharacterMessage(Fight.Clients, this, m_carriedActor, cell);

                if (actorState != null)
                    RemoveBuff(actorState);

                if (targetState != null)
                    m_carriedActor.RemoveBuff(targetState);

                var carriedActor = m_carriedActor;
                m_carriedActor = null;

                if (carriedActor.IsAlive())
                {
                    carriedActor.Position.Cell = cell;
                    Fight.ForEach(entry => ContextHandler.SendGameFightRefreshFighterMessage(entry.Client, carriedActor));
                }

                carriedActor.Dead -= OnCarryingActorDead;
                Dead -= OnCarryingActorDead;
            }
        }

        public override bool StartMove(Path movementPath)
        {
            var fightPath = new FightPath(this, Fight, movementPath);

            if (!IsCarried())
                return base.StartMove(fightPath);

            var carryingActor = GetCarryingActor();

            if (carryingActor == null)
                return base.StartMove(fightPath);

            carryingActor.ThrowActor(fightPath.StartCell, true);

            return base.StartMove(fightPath);
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

        #region Telefrag

        public bool Telefrag(FightActor caster, FightActor target)
        {
            if (!target.CanSwitchPos())
                return false;

            if (HasState((int) SpellStatesEnum.DEPLACE_120))
                return false;

            if (target.IsCarrying())
                return false;

            if (target == this)
                return false;

            if (caster != this)
            {
                NeedTelefragState = true;
            }

            target.NeedTelefragState = true;

            ExchangePositions(target);

            //TriggerBuffs(this, BuffTriggerType.OnMoved);
            target.TriggerBuffs(this, BuffTriggerType.OnMoved);

            return true;
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

        public virtual IEnumerable<DroppedItem> RollLoot(IFightResult looter) => new DroppedItem[0];

        public virtual bool CanDrop() => false;

        public virtual uint GetDroppedKamas() => 0;

        public virtual int GetGivenExperience() => 0;

        public virtual IFightResult GetFightResult(FightOutcomeEnum outcome)
            => new FightResult(this, outcome, Loot);

        public IFightResult GetFightResult() => GetFightResult(GetFighterOutcome());

        public virtual bool HasResult => true;

        public FightOutcomeEnum GetFighterOutcome() => Team.GetOutcome();

        #endregion

        #region Conditions

        public virtual bool IsAlive() => Stats.Health.Total > 0;

        public bool IsDead() => !IsAlive();

        public void CheckDead(FightActor source)
        {
            if (IsDead())
                OnDead(source);
        }

        public bool HasLost() => Fight.Losers == Team;

        public bool HasWin() => Fight.Winners == Team;

        public bool IsTeamLeader() => Team.Leader == this;

        public bool IsFighterTurn() => Fight.TimeLine.Current == this;

        public bool IsFriendlyWith(FightActor actor) => actor.Team == Team;

        public bool IsEnnemyWith(FightActor actor) => !IsFriendlyWith(actor);

        public override bool CanMove() => IsFighterTurn() && IsAlive() && MP > 0;

        public virtual bool CanTackle(FightActor fighter)
            => IsEnnemyWith(fighter) && IsAlive() && IsVisibleFor(fighter) && CanBePushed() && fighter.CanBePushed() && fighter.Position.Cell != Position.Cell;

        public virtual bool CanBePushed() => GetStates().Where(x => !x.IsDisabled).All(x => !x.State.CantBeMoved && !x.State.CantBePushed);

        public virtual bool CanSwitchPos() => GetStates().Where(x => !x.IsDisabled).All(x => !x.State.CantBeMoved && !x.State.CantSwitchPosition);

        public virtual bool CanBeHeal() => GetStates().Where(x => !x.IsDisabled).All(x => !x.State.Incurable);

        public virtual bool CanDealDamage() => GetStates().Where(x => !x.IsDisabled).All(x => !x.State.CantDealDamage);

        public virtual bool CanPlay() => GetStates().Where(x => !x.IsDisabled).All(x => !x.State.PreventsFight);

        public virtual bool IsInvulnerable(bool closeRange) => GetStates().Where(x => !x.IsDisabled).Any(x => x.State.Invulnerable || (closeRange && x.State.InvulnerableMelee) || (!closeRange && x.State.InvulnerableRange));

        public virtual bool HasLeft() => false;

        public override bool CanBeSee(WorldObject obj)
        {
            var fighter = obj as FightActor;
            var character = obj as Character;

            if (character != null && character.IsFighting())
                fighter = character.Fighter;

            if (fighter == null || fighter.Fight != Fight)
                return (base.CanBeSee(obj) || character.IsSpectator()) && VisibleState != VisibleStateEnum.INVISIBLE;

            return GetVisibleStateFor(fighter) != VisibleStateEnum.INVISIBLE && IsAlive();
        }

        #endregion

        #endregion

        #region Network

        public virtual EntityDispositionInformations GetEntityDispositionInformations(WorldClient client = null)
            =>
                new FightEntityDispositionInformations(client != null ? (IsVisibleFor(client.Character) ? Cell.Id : (short) -1) : Cell.Id, (sbyte) Direction,
                    GetCarryingActor() != null ? GetCarryingActor().Id : 0);

        public virtual GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null)
        {
            var pvp = Fight.IsPvP;

            if (Fight.State == FightState.Placement || Fight.State == FightState.NotStarted)
                return new GameFightMinimalStatsPreparation(
                    Stats.Health.Total,
                    Stats.Health.TotalMax,
                    Stats.Health.TotalMaxWithoutPermanentDamages,
                    Stats[PlayerFields.PermanentDamagePercent].Total,
                    Stats.Shield.TotalSafe,
                    (short) Stats.AP.Total,
                    (short) Stats.AP.TotalMax,
                    (short) Stats.MP.Total,
                    (short) Stats.MP.TotalMax,
                    0,
                    false,
                    (short) Stats[PlayerFields.NeutralResistPercent].Total,
                    (short) Stats[PlayerFields.EarthResistPercent].Total,
                    (short) Stats[PlayerFields.WaterResistPercent].Total,
                    (short) Stats[PlayerFields.AirResistPercent].Total,
                    (short) Stats[PlayerFields.FireResistPercent].Total,
                    (short) Stats[PlayerFields.NeutralElementReduction].Total,
                    (short) Stats[PlayerFields.EarthElementReduction].Total,
                    (short) Stats[PlayerFields.WaterElementReduction].Total,
                    (short) Stats[PlayerFields.AirElementReduction].Total,
                    (short) Stats[PlayerFields.FireElementReduction].Total,
                    (short) Stats[PlayerFields.CriticalDamageReduction].Total,
                    (short) Stats[PlayerFields.PushDamageReduction].Total,
                    (short) Stats[PlayerFields.PvpNeutralResistPercent].Total,
                    (short) Stats[PlayerFields.PvpEarthResistPercent].Total,
                    (short) Stats[PlayerFields.PvpWaterResistPercent].Total,
                    (short) Stats[PlayerFields.PvpAirResistPercent].Total,
                    (short) Stats[PlayerFields.PvpFireResistPercent].Total,
                    (short) Stats[PlayerFields.PvpNeutralElementReduction].Total,
                    (short) Stats[PlayerFields.PvpEarthElementReduction].Total,
                    (short) Stats[PlayerFields.PvpWaterElementReduction].Total,
                    (short) Stats[PlayerFields.PvpAirElementReduction].Total,
                    (short) Stats[PlayerFields.PvpFireElementReduction].Total,
                    (short) Stats[PlayerFields.DodgeAPProbability].Total,
                    (short) Stats[PlayerFields.DodgeMPProbability].Total,
                    (short) Stats[PlayerFields.TackleBlock].Total,
                    (short) Stats[PlayerFields.TackleEvade].Total,
                    0,
                    (sbyte) (client == null ? VisibleState : GetVisibleStateFor(client.Character)),
                    Stats[PlayerFields.Initiative].Total // invisibility state
                );

            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.TotalMaxWithoutPermanentDamages,
                Stats[PlayerFields.PermanentDamagePercent].Total,
                Stats.Shield.TotalSafe,
                (short) Stats.AP.Total,
                (short) Stats.AP.TotalMax,
                (short) Stats.MP.Total,
                (short) Stats.MP.TotalMax,
                0,
                false,
                (short) Stats[PlayerFields.NeutralResistPercent].Total,
                (short) Stats[PlayerFields.EarthResistPercent].Total,
                (short) Stats[PlayerFields.WaterResistPercent].Total,
                (short) Stats[PlayerFields.AirResistPercent].Total,
                (short) Stats[PlayerFields.FireResistPercent].Total,
                (short) Stats[PlayerFields.NeutralElementReduction].Total,
                (short) Stats[PlayerFields.EarthElementReduction].Total,
                (short) Stats[PlayerFields.WaterElementReduction].Total,
                (short) Stats[PlayerFields.AirElementReduction].Total,
                (short) Stats[PlayerFields.FireElementReduction].Total,
                (short) Stats[PlayerFields.CriticalDamageReduction].Total,
                (short) Stats[PlayerFields.PushDamageReduction].Total,
                (short) Stats[PlayerFields.PvpNeutralResistPercent].Total,
                (short) Stats[PlayerFields.PvpEarthResistPercent].Total,
                (short) Stats[PlayerFields.PvpWaterResistPercent].Total,
                (short) Stats[PlayerFields.PvpAirResistPercent].Total,
                (short) Stats[PlayerFields.PvpFireResistPercent].Total,
                (short) Stats[PlayerFields.PvpNeutralElementReduction].Total,
                (short) Stats[PlayerFields.PvpEarthElementReduction].Total,
                (short) Stats[PlayerFields.PvpWaterElementReduction].Total,
                (short) Stats[PlayerFields.PvpAirElementReduction].Total,
                (short) Stats[PlayerFields.PvpFireElementReduction].Total,
                (short) Stats[PlayerFields.DodgeAPProbability].Total,
                (short) Stats[PlayerFields.DodgeMPProbability].Total,
                (short) Stats[PlayerFields.TackleBlock].Total,
                (short) Stats[PlayerFields.TackleEvade].Total,
                0,
                (sbyte) (client == null ? VisibleState : GetVisibleStateFor(client.Character)) // invisibility state
            );
        }

        public virtual FightTeamMemberInformations GetFightTeamMemberInformations() => new FightTeamMemberInformations(Id);

        public virtual GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
            => new GameFightFighterInformations(
                Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte) Team.Id,
                0,
                IsAlive(),
                GetGameFightMinimalStats(client),
                MovementHistory.GetEntries(2).Select(x => x.Cell.Id).ToArray());

        public virtual GameFightFighterLightInformations GetGameFightFighterLightInformations(WorldClient client = null)
            => new GameFightFighterLightInformations(
                true,
                IsAlive(),
                Id,
                0,
                Level,
                (sbyte) BreedEnum.UNDEFINED);

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
            => GetGameFightFighterInformations();

        public abstract string GetMapRunningFighterName();

        #endregion
    }
}
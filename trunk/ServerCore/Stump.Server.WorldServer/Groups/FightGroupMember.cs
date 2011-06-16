
using System;
using System.Collections.Generic;
using Stump.Core.Threading;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Effects.Executor;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Groups
{
    public sealed class FightGroupMember : IGroupMember, IMovable
    {
        // todo : change this. This is the only way i found to know the actionId to send in SequenceEndMessage. We need more investigation
        private static readonly Dictionary<SequenceTypeEnum, int> ActionIds = new Dictionary<SequenceTypeEnum, int>
                                                                              {
                                                                                  {SequenceTypeEnum.SEQUENCE_MOVE, 3},
                                                                                  {SequenceTypeEnum.SEQUENCE_SPELL, 4},
                                                                              };

        private readonly Dictionary<int, SequenceTypeEnum> m_sequenceActions = new Dictionary<int, SequenceTypeEnum>();


        public FightGroupMember(LivingEntity entity, FightGroup groupOwner)
        {
            Entity = entity;
            GroupOwner = groupOwner;

            Position = new ObjectPosition(entity.Map, entity.Position);
        }

        public FightGroup GroupOwner
        {
            get;
            private set;
        }

        public Fight Fight
        {
            get { return GroupOwner.Fight; }
        }

        public int TotalAp
        {
            get { return Entity.Stats["AP"].Total - UsedAp; }
        }

        public int TotalMp
        {
            get { return Entity.Stats["MP"].Total - UsedMp; }
        }

        public int CurrentHealth
        {
            get { return Entity.Stats["Health"].Total - DamageTaken; }
        }

        public int UsedAp
        {
            get;
            set;
        }

        public int UsedMp
        {
            get;
            set;
        }

        public int DamageTaken
        {
            get;
            private set;
        }

        public bool IsReady
        {
            get;
            private set;
        }

        public bool IsDead
        {
            get { return CurrentHealth <= 0; }
        }

        public bool IsTurnPassed
        {
            get;
            private set;
        }

        public bool HasLeft
        {
            get;
            private set;
        }

        public bool IsPlaying
        {
            get { return Equals(Fight.FighterPlaying); }
        }

        public SequenceTypeEnum? CurrentSequence
        {
            get;
            private set;
        }

        #region IGroupMember Members

        public LivingEntity Entity
        {
            get;
            private set;
        }

        IGroup IGroupMember.GroupOwner
        {
            get { return GroupOwner; }
        }

        #endregion

        #region IMovable Members

        public ObjectPosition Position
        {
            get;
            set;
        }

        public bool IsMoving
        {
            get;
            private set;
        }

        public bool CanMove()
        {
            return IsPlaying && !IsMoving;
        }

        public void Move(MovementPath movementPath)
        {
            int requiredMp = movementPath.MpCost;
            if (!CanMove() && (TotalMp - requiredMp) < 0)
            {
                // todo : send a message error
            }
            else
            {
                UsedMp += requiredMp;
                Position.ChangeLocation(movementPath.End);

                NotifyMoved(movementPath);
            }
        }

        public void MoveInstant(ObjectPosition to)
        {
            throw new NotImplementedException();
        }

        public void MovementEnded()
        {
            IsMoving = false;
        }

        public void StopMove(ObjectPosition currentObjectPosition)
        {
            IsMoving = false;
        }

        public long Id
        {
            get { return Entity.Id; }
            set { Entity.Id = value; }
        }

        public IdentifiedEntityDispositionInformations GetIdentifiedEntityDisposition()
        {
            return new IdentifiedEntityDispositionInformations(Position.CellId, (uint) Position.Direction, (int) Id);
        }

        public EntityDispositionInformations GetEntityDisposition()
        {
            return new EntityDispositionInformations(Position.CellId, (uint) Position.Direction);
        }

        #endregion

        public event Action<FightGroupMember, bool> ReadyStateChanged;

        private void NotifyReadyStateChanged(bool isReady)
        {
            Action<FightGroupMember, bool> handler = ReadyStateChanged;
            if (handler != null)
                handler(this, isReady);
        }

        public event Action<FightGroupMember> FightLeft;

        private void NotifyFightLeft()
        {
            Action<FightGroupMember> handler = FightLeft;
            if (handler != null)
                handler(this);
        }

        public event Action<FightGroupMember, MovementPath> Moved;

        private void NotifyMoved(MovementPath movementPath)
        {
            Action<FightGroupMember, MovementPath> handler = Moved;
            if (handler != null)
                handler(this, movementPath);
        }

        public event Action<FightGroupMember, ObjectPosition> PrePlacementChanged;

        private void NotifyPrePlacementChanged(ObjectPosition position)
        {
            Action<FightGroupMember, ObjectPosition> handler = PrePlacementChanged;
            if (handler != null)
                handler(this, position);
        }

        public event Action<FightGroupMember> TurnPassed;

        private void NotifyTurnPassed()
        {
            Action<FightGroupMember> handler = TurnPassed;
            if (handler != null)
                handler(this);
        }

        public event Action<FightGroupMember, FightGroupMember> Dead;

        public void NotifyDead(FightGroupMember killedBy)
        {
            Action<FightGroupMember, FightGroupMember> handler = Dead;
            if (handler != null)
                handler(this, killedBy);
        }

        public void CallIfCharacter(Action<Character> action)
        {
            if (Entity is Character)
                action(Entity as Character);
        }

        public void SetReady(bool isReady)
        {
            IsReady = isReady;

            NotifyReadyStateChanged(isReady);
        }

        public void ChangePrePlacementPosition(ushort cellId)
        {
            if (Fight.CanChangePosition(this, cellId))
            {
                Position.ChangeLocation(cellId);

                NotifyPrePlacementChanged(Position);
            }
        }

        public void PassTurn()
        {
            IsTurnPassed = true;

            NotifyTurnPassed();
        }

        public void CastSpell(uint spellId, int cellId)
        {
            if (!Entity.Spells.Contains((SpellIdEnum) spellId))
            {
                // Error : Character doesn't have this spell.
                return;
            }

            Spell spell = Entity.Spells[(SpellIdEnum) spellId];
            SpellLevel spellLevel = spell.CurrentSpellLevel;
            CellLinked cell = Entity.Map.Cells[cellId];

            if (CanCastSpell(cell, spellLevel))
            {
                var asyncRandom = new AsyncRandom();
                bool critical = false;

                if (IsCriticalFail(asyncRandom, spellLevel)) // if the cast fail
                {
                    // TODO: Manage Fail
                }
                else if (IsCriticalHit(asyncRandom, spellLevel)) // if the cast is critical
                {
                    critical = true;
                }

                StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);
                {
                    UseApPoints((ushort) spellLevel.ApCost);

                    Fight.Do(
                        entry =>
                        ContextHandler.SendGameActionFightSpellCastMessage(entry.Client,
                                                                           (uint) ActionsEnum.ACTION_FIGHT_CAST_SPELL,
                                                                           Entity, (ushort) cellId, critical, false,
                                                                           spellLevel));

                    FightEffectExecutor.ExecuteSpellEffects(Fight, spellLevel, this, cell, critical);
                }
                EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

                // TODO : Check range.
                // TODO: Get effect of spell then use it.
            }
        }

        public bool CanCastSpell(CellLinked cell, SpellLevel spellLevel)
        {
            // check range
            return TotalAp >= spellLevel.ApCost;
        }

        public bool IsCriticalHit(AsyncRandom random, SpellLevel spell)
        {
            return random.NextInt(1, 101) <= spell.CriticalHitProbability;
        }

        public bool IsCriticalFail(AsyncRandom random, SpellLevel spell)
        {
            return random.NextInt(1, 101) <= spell.CriticalFailureProbability;
        }

        public void UseApPoints(ushort points)
        {
            UsedAp += points;

            NotifyLosePoints(points, ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE);
        }

        public void UseMpPoints(ushort points)
        {
            UsedAp += points;

            NotifyLosePoints(points, ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE);
        }

        public void LeaveFight()
        {
            if (!HasLeft)
            {
                HasLeft = true;

                if (Fight.Started)
                {
                    Die();
                }

                Entity.LeaveFight();

                NotifyFightLeft();

                SynchroniseCharacter(FighterEndStatus.Leaver);
            }
        }

        internal void ResetUsedProperties()
        {
            UsedMp = 0;
            UsedAp = 0;
            IsTurnPassed = false;
        }

        internal void NotifyAssignedToFight()
        {
            Fight.FightEnded += OnFightEnded;
        }

        private void OnFightEnded(Fight fight, FightGroup winners, FightGroup losers, bool draw)
        {
            if (Entity is Character)
                SynchroniseCharacter(GroupOwner.IsWinner ? FighterEndStatus.Winner : FighterEndStatus.Loser);
        }

        private void SynchroniseCharacter(FighterEndStatus endStatus)
        {
            switch (endStatus)
            {
                case FighterEndStatus.Winner:
                    break;
                case FighterEndStatus.Loser:
                    break;
                case FighterEndStatus.Leaver:
                    break;
            }
        }

        public void Die()
        {
            ReceiveDamage((ushort) CurrentHealth);
        }

        public ushort ReceiveDamage(ushort damage, FightGroupMember from)
        {
            if (CurrentHealth - damage < 0)
                damage = (ushort) CurrentHealth;

            DamageTaken += damage;

            NotifyLoseLifePoints(damage, from);

            if (IsDead)
                NotifyDead(from);

            return damage;
        }

        public ushort ReceiveDamage(ushort damage)
        {
            if (CurrentHealth - damage < 0)
                damage = (ushort) CurrentHealth;

            DamageTaken += damage;

            NotifyLoseLifePoints(damage);

            if (IsDead)
                NotifyDead(null);

            return damage;
        }

        private void NotifyLosePoints(ushort losedPoints, ActionsEnum action)
        {
            Fight.Do(
                entry =>
                ActionsHandler.
                    SendGameActionFightPointsVariationMessage(
                        entry.Client,
                        action,
                        Entity,
                        Entity, (short)( -losedPoints )));
        }

        private void NotifyLosePoints(ushort losedPoints, ActionsEnum action, FightGroupMember from)
        {
            Fight.Do(
                entry =>
                ActionsHandler.
                    SendGameActionFightPointsVariationMessage(
                        entry.Client,
                        action,
                        from.Entity,
                        Entity, (short)( -losedPoints )));
        }

        private void NotifyLoseLifePoints(ushort losedPoints)
        {
            Fight.Do(
                entry =>
                ActionsHandler.
                    SendGameActionFightLifePointsVariationMessage(
                        entry.Client,
                        Entity,
                        Entity, (short) (-losedPoints)));
        }

        private void NotifyLoseLifePoints(ushort losedPoints, FightGroupMember from)
        {
            Fight.Do(
                entry =>
                ActionsHandler.
                    SendGameActionFightLifePointsVariationMessage(
                        entry.Client,
                        from.Entity,
                        Entity, (short) (-losedPoints)));
        }

        public void ExecuteInstantSequence(SequenceTypeEnum sequenceType, Action<Character> sequence)
        {
            int actionId = ActionIds.ContainsKey(sequenceType)
                               ? ActionIds[sequenceType]
                               : 8 - (int) sequenceType;
            m_sequenceActions.Add(actionId, sequenceType);

            Fight.Do(
                character =>
                {
                    ActionsHandler.SendSequenceStartMessage(character.Client, Entity,
                                                            sequenceType);

                    sequence(character);

                    ActionsHandler.SendSequenceEndMessage(character.Client, Entity,
                                                          actionId,
                                                          sequenceType);
                });
        }

        public bool StartSequence(SequenceTypeEnum sequenceType)
        {
            if (CurrentSequence == null)
            {
                CurrentSequence = sequenceType;

                Fight.Do(
                    character => ActionsHandler.SendSequenceStartMessage(character.Client, Entity,
                                                                         sequenceType));

                return true;
            }

            return false;
        }

        public bool EndSequence(SequenceTypeEnum sequenceType)
        {
            int actionId = ActionIds.ContainsKey(sequenceType)
                               ? ActionIds[sequenceType]
                               : 8 - (int) sequenceType;
            m_sequenceActions.Add(actionId, sequenceType);

            if (CurrentSequence != null)
            {
                Fight.Do(
                    character => ActionsHandler.SendSequenceEndMessage(character.Client, Entity,
                                                                       actionId,
                                                                       sequenceType));

                CurrentSequence = null;

                return true;
            }

            return false;
        }

        internal void SequenceEndReply(int actionId)
        {
            if (m_sequenceActions.ContainsKey(actionId))
            {
                if (m_sequenceActions[actionId] == SequenceTypeEnum.SEQUENCE_MOVE)
                {
                    MovementEnded();
                }

                m_sequenceActions.Remove(actionId);
            }
        }

        public GameFightMinimalStats GetFightMinimalStats()
        {
            return new GameFightMinimalStats(
                (uint) CurrentHealth,
                (uint) Entity.MaxHealth,
                TotalAp,
                TotalMp,
                Entity.Stats["SummonLimit"].Total,
                Entity.Stats["NeutralResistPercent"].Total,
                Entity.Stats["EarthResistPercent"].Total,
                Entity.Stats["WaterResistPercent"].Total,
                Entity.Stats["AirResistPercent"].Total,
                Entity.Stats["FireResistPercent"].Total,
                (uint) Entity.Stats["DodgeAPProbability"].Total,
                (uint) Entity.Stats["DodgeMPProbability"].Total,
                0, // tackleblock
                (int) GameActionFightInvisibilityStateEnum.VISIBLE);
        }

        #region Nested type: FighterEndStatus

        private enum FighterEndStatus
        {
            Winner,
            Loser,
            Leaver,
        }

        #endregion
    }
}
// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Groups
{
    public sealed class FightGroupMember : IGroupMember, IMovable
    {
        private enum FighterEndStatus
        {
            Winner,
            Loser,
            Leaver,
        }

        // todo : change this. This is the only way i found to know the actionId to send in SequenceEndMessage. We need more investigation
        private static readonly Dictionary<SequenceTypeEnum, int> ActionIds = new Dictionary<SequenceTypeEnum, int>
            {
                {SequenceTypeEnum.SEQUENCE_MOVE, 3}
            };

        private Dictionary<int, SequenceTypeEnum> m_sequenceActions = new Dictionary<int, SequenceTypeEnum>();

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

        public event Action<FightGroupMember, VectorIsometric> PrePlacementChanged;

        private void NotifyPrePlacementChanged(VectorIsometric position)
        {
            Action<FightGroupMember, VectorIsometric> handler = PrePlacementChanged;
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


        public FightGroupMember(LivingEntity entity, FightGroup groupOwner)
        {
            Entity = entity;
            GroupOwner = groupOwner;

            Position = new VectorIsometric(entity.Map, entity.Position);
        }

        public LivingEntity Entity
        {
            get;
            private set;
        }

        public FightGroup GroupOwner
        {
            get;
            private set;
        }

        IGroup IGroupMember.GroupOwner
        {
            get
            {
                return GroupOwner;
            }
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

        #region IMovable Members

        public VectorIsometric Position
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

        public void MoveInstant(VectorIsometric to)
        {
            throw new NotImplementedException();
        }

        public void MovementEnded()
        {
            IsMoving = false;
        }

        public void StopMove(VectorIsometric currentVectorIsometric)
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
                damage = (ushort)CurrentHealth;

            DamageTaken += damage;

            if (IsDead)
                NotifyDead(from);

            return damage;
        }

        public ushort ReceiveDamage(ushort damage)
        {
            if (CurrentHealth - damage < 0)
                damage = (ushort) CurrentHealth;

            DamageTaken += damage;

            if (IsDead)
                NotifyDead(null);

            return damage;
        }

        public void StartSequence(SequenceTypeEnum sequenceType, Action<Character> sequence)
        {
            int actionId = ActionIds.ContainsKey(sequenceType)
                               ? ActionIds[sequenceType]
                               : 8 - (int) sequenceType;
            m_sequenceActions.Add(actionId, sequenceType);

            Fight.CallOnAllCharacters(character =>
            {
                ActionsHandler.SendSequenceStartMessage(character.Client, character,
                                                        sequenceType);

                sequence(character);

                ActionsHandler.SendSequenceEndMessage(character.Client, character,
                                                      actionId,
                                                      sequenceType);
            });
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
    }
}
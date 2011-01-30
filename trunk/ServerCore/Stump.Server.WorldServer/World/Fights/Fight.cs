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
using System.Linq;
using System.Threading.Tasks;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Manager;
using Stump.Server.WorldServer.Effects.Executor;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Fights
{
    public enum FightState
    {
        PreparePosition,
        Fighting,
        Ended
    }

    public class Fight : IInstance, IDisposable
    {
        /// <summary>
        ///   Delay for player's turn
        /// </summary>
        [Variable]
        public static int TurnTime = 35000;

        /// <summary>
        ///   Delay before force turn to end
        /// </summary>
        [Variable]
        public static int TurnEndTimeOut = 5000;

        public event Action<Fight> FightStarted;

        public void NotifyFightStarted()
        {
            Action<Fight> handler = FightStarted;
            if (handler != null)
                handler(this);
        }


        public delegate void FightEndedDelegate(Fight fight, FightGroup winners, FightGroup losers, bool draw);

        public event FightEndedDelegate FightEnded;

        private void NotifyFightEnded(FightGroup winners, FightGroup losers, bool draw)
        {
            FightEndedDelegate handler = FightEnded;
            if (handler != null)
                handler(this, winners, losers, draw);
        }

        #region Fields

        private readonly List<FightGroupMember> m_fighters = new List<FightGroupMember>();
        private readonly List<Character> m_characters = new List<Character>();

        private bool m_disposed;
        private DateTime m_startTime;
        private TimeLine m_timeline;

        #endregion

        #region Constructor

        /// <summary>
        ///   Create a fight with source as Team 0 and target as Team 1 and not started yet.
        /// </summary>
        /// <param name = "source">A first group</param>
        /// <param name = "target">An other group</param>
        /// <param name = "fightType">Fight type</param>
        public Fight(FightGroup source, FightGroup target, FightTypeEnum fightType)
        {
            SourceGroup = source;
            SourceGroup.TeamId = 0;
            SourceGroup.Fight = this;
            TargetGroup = target;
            TargetGroup.TeamId = 1;
            TargetGroup.Fight = this;

            SourceGroup.MemberAdded += OnGroupMemberAdded;
            SourceGroup.MemberRemoved += OnGroupMemberRemoved;
            TargetGroup.MemberAdded += OnGroupMemberAdded;
            TargetGroup.MemberRemoved += OnGroupMemberRemoved;

            AddFighters(SourceGroup.Members);
            AddFighters(TargetGroup.Members);

            m_timeline = new TimeLine(this);

            FightType = fightType;

            SetFightState(FightState.PreparePosition);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Starting a fight, from map to preparation time in fight
        /// </summary>
        public void StartingFight()
        {
            StartPreFightEvents();

            // TODO : write a routine that found correct pre-fight placements
            SourceGroup.Positions = new ushort[] {370, 355, 354};
            TargetGroup.Positions = new ushort[] {328, 356, 357};

            SourceGroup.Leader.Position.ChangeLocation(SourceGroup.Positions.First());
            TargetGroup.Leader.Position.ChangeLocation(TargetGroup.Positions.First());

            SetFightState(FightState.PreparePosition);

            CallOnAllCharacters(character => PrepareFight(character, character.FightGroup));
        }

        private void StartPreFightEvents()
        {
            foreach (FightGroupMember fighter in m_fighters)
            {
                fighter.ReadyStateChanged += OnSetReady;
                fighter.PrePlacementChanged += OnChangePreplacementPosition;
            }

            foreach (var character in m_characters)
            {
                character.LoggingOut += OnLoggedOut;
            }
        }

        private void OnGroupMemberAdded(Group<FightGroupMember> group, FightGroupMember fighter)
        {
            AddFighter(fighter);
        }

        private void AddFighter(FightGroupMember fighter)
        {
            m_fighters.Add(fighter);

            if (fighter.Entity is Character)
                m_characters.Add(fighter.Entity as Character);

            fighter.NotifyAssignedToFight();
        }

        private void AddFighters(IEnumerable<FightGroupMember> fighters)
        {
            foreach (var fighter in fighters)
            {
                AddFighter(fighter);
            }
        }

        private void OnGroupMemberRemoved(Group<FightGroupMember> group, FightGroupMember fighter)
        {
            RemoveFighter(fighter);
        }

        private void RemoveFighter(FightGroupMember fighter)
        {
            if (Started)
            {
                fighter.FightLeft -= OnFighterLeft;
                fighter.Dead -= OnFighterDead;
                fighter.Moved -= OnFighterMoved;
                fighter.TurnPassed -= OnTurnPassed;
            }
            else
            {
                fighter.ReadyStateChanged -= OnSetReady;
                fighter.PrePlacementChanged -= OnChangePreplacementPosition;
            }

            m_fighters.Remove(fighter);

            if (fighter.Entity is Character)
            {
                ( fighter.Entity as Character ).LoggingOut -= OnLoggedOut;

                m_characters.Remove(fighter.Entity as Character);
            }
        }

        private void RemoveFighters(IEnumerable<FightGroupMember> fighters)
        {
            foreach (var fighter in fighters)
            {
                RemoveFighter(fighter);
            }
        }

        /// <summary>
        ///   Prepare to Fight method, Showing cells and characters.
        /// </summary>
        private void PrepareFight(Character charac, FightGroup group)
        {
            // TODO : Show "swords", use events
            charac.Map.OnFightEnter(charac);


            ContextHandler.SendGameContextDestroyMessage(charac.Client);
            ContextHandler.SendGameContextCreateMessage(charac.Client, 2);

            ContextHandler.SendGameFightStartingMessage(charac.Client, FightType);
            ContextHandler.SendGameFightJoinMessage(charac.Client, true, true, false, false, 0, FightType);
            // todo : define this
            ContextHandler.SendGameFightPlacementPossiblePositionsMessage(charac.Client, this, group.TeamId);

            CharacterHandler.SendLifePointsRegenEndMessage(charac.Client);

            ContextHandler.SendGameFightShowFighterMessage(charac.Client, SourceGroup.Leader);
            ContextHandler.SendGameFightShowFighterMessage(charac.Client, TargetGroup.Leader);

            ContextHandler.SendGameEntitiesDispositionMessage(charac.Client, GetAllFighters());

            ContextHandler.SendGameFightUpdateTeamMessage(charac.Client, this, SourceGroup);
            ContextHandler.SendGameFightUpdateTeamMessage(charac.Client, this, TargetGroup);
        }

        /// <summary>
        ///   Cancel a fight by remove it
        /// </summary>
        /// <param name = "groupId">Group that have cancelled the fight</param>
        public void CancelFight(int groupId)
        {
            EndFight();
        }

        /// <summary>
        ///   Set the ready state of a character
        /// </summary>
        private void OnSetReady(FightGroupMember fighter, bool isReady)
        {
            CallOnAllCharacters(
                charac => ContextHandler.SendGameFightHumanReadyStateMessage(charac.Client, fighter));

            if (SourceGroup.IsAllReady() && TargetGroup.IsAllReady())
                StartFight();
        }

        /// <summary>
        ///   Check if a character can change position (before fight is started).
        /// </summary>
        /// <param name = "fighter"></param>
        /// <param name = "cellId">The cellId wanted</param>
        /// <returns>If change is possible</returns>
        public bool CanChangePosition(FightGroupMember fighter, ushort cellId)
        {
            return !Started && fighter.GroupOwner.Positions.Contains(cellId) && GetOneFighter(cellId) == null;
        }

        private void OnChangePreplacementPosition(FightGroupMember fighter, VectorIsometric vectorIsometric)
        {
            CallOnAllCharacters(character =>
                                ContextHandler.SendGameEntitiesDispositionMessage(character.Client,
                                                                                  GetAllFighters()));
        }

        /// <summary>
        ///   Start a fight, in combat time.
        /// </summary>
        private void StartFight()
        {
            CallOnAllCharacters(charac =>
            {
                ContextHandler.SendGameEntitiesDispositionMessage(charac.Client, GetAllFighters());

                ContextHandler.SendGameFightStartMessage(charac.Client);
                ContextHandler.SendGameFightTurnListMessage(charac.Client, this);

                ContextHandler.SendGameFightSynchronizeMessage(charac.Client, this);
                CharacterHandler.SendCharacterStatsListMessage(charac.Client);
            });

            StopPreFightEvents();
            StartEvents();

            m_timeline.TurnStarted += OnTurnStarted;
            m_timeline.TurnEndedRequest += OnTurnEndedRequest;
            m_timeline.TurnEnded += OnTurnEnded;

            m_timeline.Start();

            SetFightState(FightState.Fighting);

            NotifyFightStarted();
        }

        private void StopPreFightEvents()
        {
            SourceGroup.MemberAdded -= OnGroupMemberAdded;
            SourceGroup.MemberRemoved -= OnGroupMemberRemoved;
            TargetGroup.MemberAdded -= OnGroupMemberAdded;
            TargetGroup.MemberRemoved -= OnGroupMemberRemoved;

            foreach (FightGroupMember fighter in m_fighters)
            {
                fighter.ReadyStateChanged -= OnSetReady;
                fighter.PrePlacementChanged -= OnChangePreplacementPosition;
            }
        }

        private void StartEvents()
        {
            foreach (FightGroupMember fighter in m_fighters)
            {
                fighter.FightLeft += OnFighterLeft;
                fighter.Dead += OnFighterDead;
                fighter.Moved += OnFighterMoved;
                fighter.TurnPassed += OnTurnPassed;
            }
        }

        private void OnFighterDead(FightGroupMember fighter, FightGroupMember killer)
        {
            fighter.ExecuteInstantSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH,
                      character =>
                      ActionsHandler.SendGameActionFightDeathMessage(character.Client, fighter.Entity));

            CheckIfEnd();
        }

        private void OnFighterLeft(FightGroupMember fighter)
        {
            if (!fighter.IsDead)
            {
                fighter.ExecuteInstantSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH,
                                      character =>
                                      ActionsHandler.SendGameActionFightDeathMessage(character.Client, fighter.Entity));
            }

            CheckIfEnd();
        }

        private void OnLoggedOut(Character character)
        {
            if (character.IsInFight && character.Fight.Id == Id)
            {
                character.Fighter.LeaveFight();
            }
        }

        private void OnTurnPassed(FightGroupMember fighter)
        {
            CallOnAllCharacters(charac => ContextHandler.SendGameFightTurnReadyRequestMessage(charac.Client, fighter.Entity));
        }

        private void OnTurnStarted(TimeLine sender, FightGroupMember currentfighter)
        {
            CallOnAllCharacters(charac =>
            {
                ContextHandler.SendGameFightSynchronizeMessage(charac.Client, this);
                CharacterHandler.SendCharacterStatsListMessage(charac.Client);
                ContextHandler.SendGameFightTurnStartMessage(charac.Client, (int) currentfighter.Entity.Id,
                                                             (uint) TurnTime);
            });
        }

        public void FinishTurn(FightGroupMember fighter)
        {
            m_timeline.RequestTurnEnd(fighter);
        }

        private void OnTurnEndedRequest(TimeLine sender, FightGroupMember currentFighter)
        {
            CallOnAllCharacters(
                charac => ContextHandler.SendGameFightTurnEndMessage(charac.Client, currentFighter.Entity));

            if (currentFighter.Entity is Character)
                ContextHandler.SendGameFightTurnReadyRequestMessage((currentFighter.Entity as Character).Client,
                                                                    currentFighter.Entity);
            else
            {
                TurnEndConfirm(currentFighter);
            }
        }

        public void TurnEndConfirm(FightGroupMember fighter)
        {
            if (fighter.IsPlaying)
                m_timeline.ConfirmTurnEnd();
        }

        private void OnTurnEnded(TimeLine sender, FightGroupMember lastFighter, FightGroupMember currentFighter)
        {
        }

        public bool CheckIfEnd()
        {
            if (State == FightState.Ended)
                return true;

            if (SourceGroup.IsAllDead() || TargetGroup.IsAllDead())
            {
                EndFight();

                return true;
            }

            return false;
        }

        public void EndFight()
        {
            try
            {
                SetFightState(FightState.Ended);

                CallOnAllCharacters(character =>
                {
                    ContextHandler.SendGameFightEndMessage(character.Client, this);

                    ContextHandler.SendGameContextDestroyMessage(character.Client);
                    ContextHandler.SendGameContextCreateMessage(character.Client, 1);
                    CharacterHandler.SendCharacterStatsListMessage(character.Client);
                    CharacterHandler.SendLifePointsRegenBeginMessage(character.Client,
                                                                     60*(uint) character.Client.ActiveCharacter.Level);

                    character.Map.OnFightLeave(character);
                    character.LeaveFight();

                    ContextHandler.SendCurrentMapMessage(character.Client, character.Map.Id);
                });

                if (SourceGroup.IsAllDead() && TargetGroup.IsAllDead()) // logically it's impossible. todo : compare when the last fighter of each team is dead
                    NotifyFightEnded(null, null, true);
                else if (SourceGroup.IsAllDead())
                {
                    SourceGroup.IsWinner = false;
                    TargetGroup.IsWinner = true;

                    NotifyFightEnded(TargetGroup, SourceGroup, false);
                }
                else if (TargetGroup.IsAllDead())
                {
                    SourceGroup.IsWinner = true;
                    TargetGroup.IsWinner = false;


                    NotifyFightEnded(SourceGroup, TargetGroup, false);
                }

                GroupManager.RemoveGroup(SourceGroup);
                GroupManager.RemoveGroup(TargetGroup);

                FightManager.RemoveFight(this);
            }
            finally
            {
                Dispose();
            }
        }

        public bool CanCastSpell(FightGroupMember caster, CellData cell, SpellLevel spell)
        {
            return true;
        }

        public void OnFighterMoved(FightGroupMember fighter, MovementPath movement)
        {
            List<uint> movementsKeys = movement.GetServerMovementKeys();
            var delta = (short) (-movement.MpCost);

            fighter.ExecuteInstantSequence(SequenceTypeEnum.SEQUENCE_MOVE, character =>
            {
                ContextHandler.SendGameMapMovementMessage(character.Client, movementsKeys, fighter.Entity);
                ActionsHandler.SendGameActionFightPointsVariationMessage(character.Client,
                                                                         ActionsEnum.
                                                                             ACTION_CHARACTER_MOVEMENT_POINTS_USE,
                                                                         fighter.Entity, fighter.Entity, delta);
            });
        }

        /// <summary>
        ///   Change the actual state of the fight
        /// </summary>
        /// <param name = "state"></param>
        public void SetFightState(FightState state)
        {
            State = state;
        }

        /// <summary>
        ///   Gets the ids of alive entites.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetAlivesIds()
        {
            if (!Started)
                return new int[0];

            return SourceGroup.GetAlivesIds().Concat(TargetGroup.GetAlivesIds());
        }

        /// <summary>
        ///   Gets the ids of dead entites.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetDeadsIds()
        {
            if (!Started)
                return new int[0];

            return SourceGroup.GetDeadsIds().Concat(TargetGroup.GetDeadsIds());
        }

        /// <summary>
        ///   Get a FightGroup in this fight by his groupId.
        /// </summary>
        /// <param name = "groupId"></param>
        /// <returns></returns>
        public FightGroup GetGroup(int groupId)
        {
            return SourceGroup.Id == groupId ? SourceGroup : TargetGroup.Id == groupId ? TargetGroup : null;
        }

        /// <summary>
        ///   Get the FightGroup where the entity is in this fight
        /// </summary>
        /// <param name = "entityId"></param>
        /// <returns>The fightGroup where the entity is in this fight, if is not in both groups, return null</returns>
        public FightGroup GetGroupByEntity(int entityId)
        {
            Entity ent = SourceGroup.GetEntityById(entityId);

            if (ent != null)
                return SourceGroup;

            ent = TargetGroup.GetEntityById(entityId);

            if (ent != null)
                return TargetGroup;

            return null;
        }

        /// <summary>
        ///   Get the FightGroup where the character is in this fight
        /// </summary>
        /// <param name = "chrId">The character's id of the fighter</param>
        /// <returns>The fightGroup where the character is in this fight, if is not in both groups, return null</returns>
        public FightGroup GetGroupByCharacter(int chrId)
        {
            Character chr = SourceGroup.GetCharacterById(chrId);

            if (chr != null)
                return SourceGroup;

            chr = TargetGroup.GetCharacterById(chrId);

            if (chr != null)
                return TargetGroup;

            return null;
        }

        /// <summary>
        ///   Get the FightGroupMember corresponding to Character's Id.
        /// </summary>
        /// <param name = "characterId">The character's id of the fighter</param>
        /// <returns>The fightGroupMember of the character, else null</returns>
        public FightGroupMember GetGroupMemberByCharacter(int characterId)
        {
            return SourceGroup.GetMemberById(characterId) ?? TargetGroup.GetMemberById(characterId);
        }

        /// <summary>
        ///   Execute an action on every characters in this fight.
        /// </summary>
        /// <param name = "action"></param>
        public void CallOnAllCharacters(Action<Character> action)
        {
            IEnumerable<Character> chars = GetAllCharacters();

            Parallel.ForEach(chars, action);
        }

        public IEnumerable<FightGroupMember> GetAllFighters()
        {
            return Fighters;
        }

        public IEnumerable<FightGroupMember> GetAllFighters(Func<FightGroupMember, bool> predicate)
        {
            return GetAllFighters().Where(predicate);
        }

        public IEnumerable<FightGroupMember> GetAllFighters(CellData cell)
        {
            return GetAllFighters(entry => entry.Position.Cell == cell);
        }

        public FightGroupMember GetOneFighter(Func<FightGroupMember, bool> predicate)
        {
            return GetAllFighters().SingleOrDefault(predicate);
        }

        public FightGroupMember GetOneFighter(CellData cell)
        {
            return GetOneFighter(entry => entry.Position.Cell == cell);
        }

        public FightGroupMember GetOneFighter(ushort cellId)
        {
            return GetOneFighter(entry => entry.Position.Cell.Id == cellId);
        }

        /// <summary>
        ///   Get all entities contains in both sides groups
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity> GetAllEntities()
        {
            return GetAllFighters().Select(entry => entry.Entity);
        }

        /// <summary>
        ///   Get all characters contains in both sides groups
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Character> GetAllCharacters()
        {
            return m_characters;
        }

        #endregion

        #region Properties

        public FightTypeEnum FightType
        {
            get;
            private set;
        }

        public FightState State
        {
            get;
            set;
        }

        /// <summary>
        ///   Combat is started or in prepare.
        /// </summary>
        public bool Started
        {
            get
            {
                return State != FightState.PreparePosition;
            }
        }

        /// <summary>
        ///   FightGroup of the sourceGroupe (also named challengers)
        /// </summary>
        public FightGroup SourceGroup
        {
            get;
            private set;
        }

        /// <summary>
        ///   FightGroup of the targetGroup (also named defenders)
        /// </summary>
        public FightGroup TargetGroup
        {
            get;
            private set;
        }

        public IEnumerable<FightGroupMember> Fighters
        {
            get
            {
                return m_fighters;
            }
        }

        /// <summary>
        ///   Get the fighter who is playing his turn
        /// </summary>
        public FightGroupMember FighterPlaying
        {
            get { return m_timeline != null ? m_timeline.Current : null; }
        }

        /// <summary>
        ///   The age bonus (stars) of the fight.
        /// </summary>
        public int AgeBonus
        {
            get;
            private set;
        }

        /// <summary>
        ///   Duration of the fight.
        /// </summary>
        public int Duration
        {
            get { return (int) (DateTime.Now - m_startTime).TotalMilliseconds; }
        }

        /// <summary>
        ///   FightId of this fight
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (!m_disposed)
            {
                StopEvents();

                if (m_timeline != null)
                    m_timeline.Dispose();

                m_disposed = true;
            }
        }

        #endregion

        private void StopEvents()
        {
            if (m_characters != null)
                foreach (Character character in m_characters)
                {
                    character.LoggingOut -= OnLoggedOut;
                }

            if (m_fighters != null)
                foreach (FightGroupMember fighter in m_fighters)
                {
                    fighter.FightLeft -= OnFighterLeft;
                    fighter.Dead -= OnFighterDead;
                    fighter.Moved -= OnFighterMoved;
                    fighter.TurnPassed -= OnTurnPassed;
                }
        }
    }
}
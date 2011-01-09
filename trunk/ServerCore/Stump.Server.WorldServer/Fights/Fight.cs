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
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Fights
{
    public enum FightState
    {
        AskingForDuel,
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
        public static int TurnEndTimeOut = 5000; // 5s

        #region Fields

        private IEnumerable<FightGroupMember> m_fighters;
        private IEnumerable<Character> m_characters;
        private DateTime m_startTime;
        private TimeLine m_timeline;
        private bool m_disposed;

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

            FightType = fightType;

            SetFightState(FightState.PreparePosition);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Prepare to Fight method, Showing cells and characters.
        /// </summary>
        /// <param name = "charac"></param>
        /// <param name = "group"></param>
        private void PrepareFight(Character charac, FightGroup group)
        {
            var chrSource = SourceGroup.Leader.Entity as Character;
            var chrTarget = TargetGroup.Leader.Entity as Character;

            // TODO : Show "swords", use events
            charac.Map.OnFightEnter(charac);

            ContextHandler.SendGameContextDestroyMessage(charac.Client);
            ContextHandler.SendGameContextCreateMessage(charac.Client, 2);

            ContextHandler.SendGameFightStartingMessage(charac.Client, FightType);
            ContextHandler.SendGameFightJoinMessage(charac.Client, true, true, false, false, 0, FightType);
                // todo : define this
            ContextHandler.SendGameFightPlacementPossiblePositionsMessage(charac.Client, this, group.TeamId);

            CharacterHandler.SendLifePointsRegenEndMessage(charac.Client);

            ContextHandler.SendGameFightShowFighterMessage(charac.Client, chrTarget.CurrentFighter);
            ContextHandler.SendGameFightShowFighterMessage(charac.Client, chrSource.CurrentFighter);

            ContextHandler.SendGameEntitiesDispositionMessage(charac.Client, GetAllFighters());

            ContextHandler.SendGameFightUpdateTeamMessage(charac.Client, this, SourceGroup);
            ContextHandler.SendGameFightUpdateTeamMessage(charac.Client, this, TargetGroup);
        }

        /// <summary>
        ///   Starting a fight, from map to preparation time in fight
        /// </summary>
        public void StartingFight()
        {
            var group = new[] {SourceGroup, TargetGroup};

            SourceGroup.Positions = new ushort[] {370, 355, 354};
            // TODO : write a routine that found correct pre-fight placements
            TargetGroup.Positions = new ushort[] {328, 356, 357};

            ChangePosition(SourceGroup.Members.First() as FightGroupMember, SourceGroup.Positions.First());
            ChangePosition(TargetGroup.Members.First() as FightGroupMember, TargetGroup.Positions.First());

            SetFightState(FightState.PreparePosition);
            Parallel.For(0, 2, i => PrepareFight(group[i].Leader.Entity as Character, group[i]));
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
        public void SetReadyState(FightGroupMember fighter, bool isReady)
        {
            fighter.IsReady = isReady;

            CallOnAllCharacters(
                charac => ContextHandler.SendGameFightHumanReadyStateMessage(charac.Client, fighter));

            if (SourceGroup.AllAreReady() && TargetGroup.AllAreReady())
                StartFight();
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

                BasicHandler.SendBasicNoOperationMessage(charac.Client);

                ContextHandler.SendGameFightSynchronizeMessage(charac.Client, this);
                CharacterHandler.SendCharacterStatsListMessage(charac.Client);

                charac.LoggingOut += OnLoggedOut;
            });

            SetFightState(FightState.Fighting);

            m_timeline = new TimeLine(this);
            m_timeline.TurnStarted += OnTurnStarted;
            m_timeline.TurnEndedRequest += OnTurnEndedRequest;
            m_timeline.TurnEnded += OnTurnEnded;

            m_timeline.UpdateToNextFighter();
            m_timeline.StartTurn();
        }

        public void QuitFight(FightGroupMember fighter)
        {
            if (!fighter.IsDead)
            {
                CallOnAllCharacters(entity =>
                {
                    ActionsHandler.SendSequenceStartMessage(entity.Client, fighter.Entity,
                                                            SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
                    ActionsHandler.SendGameActionFightDeathMessage(entity.Client, fighter.Entity);
                    ActionsHandler.SendSequenceEndMessage(entity.Client, fighter.Entity,
                                                          SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
                });
                fighter.HasLeft = true;
            }

            CheckIfEnd();
        }

        private void OnLoggedOut(Character character)
        {
            if (character.IsInFight && character.CurrentFight.Id == Id)
            {
                QuitFight(character.CurrentFighter);
            }
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

            if (SourceGroup.GetAlivesIds().Length == 0 || TargetGroup.GetAlivesIds().Length == 0)
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

                CallOnAllCharacters(charac =>
                {
                    ContextHandler.SendGameFightEndMessage(charac.Client, this);

                    ContextHandler.SendGameContextDestroyMessage(charac.Client);
                    ContextHandler.SendGameContextCreateMessage(charac.Client, 1);
                    CharacterHandler.SendCharacterStatsListMessage(charac.Client);
                    CharacterHandler.SendLifePointsRegenBeginMessage(charac.Client,
                                                                     60*(uint) charac.Client.ActiveCharacter.Level);

                    charac.Map.OnFightLeave(charac);
                    ContextHandler.SendCurrentMapMessage(charac.Client, charac.Map.Id);
                });

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

        /// <summary>
        ///   Use the required spell by the fighter.
        /// </summary>
        public void UseSpell(FightGroupMember caster, int cellId, int spellId)
        {
            if (!caster.Entity.Spells.Contains((SpellIdEnum) spellId))
            {
                // Error : Character doesn't have this spell.
                return;
            }
            Spell spell = caster.Entity.Spells[(SpellIdEnum) spellId];
            if (spell == null)
            {
                // spell doesn't exist.
                return;
            }

            SpellLevel spellLevel = spell.CurrentSpellLevel;
            CellData cell = caster.Entity.Map.CellsData[cellId];

            if (CanCastSpell(caster, cell, spellLevel))
            {
                var rnd = new Random();
                bool critical = false;

                if (rnd.Next(1, 101) <=
                    caster.Entity.Spells[(SpellIdEnum) spellId].CurrentSpellLevel.CriticalFailureProbability)
                    // if the cast fail
                {
                    // TODO: Manage Fail
                }
                else if (rnd.Next(1, 101) <=
                         caster.Entity.Spells[(SpellIdEnum) spellId].CurrentSpellLevel.CriticalHitProbability)
                    // if the cast is critical
                {
                    critical = true;
                }

                FightEffectExecutor.ExecuteSpellEffects(spellLevel, this, caster, cell, critical);

                // TODO : Check range.
                // TODO: Get effect of spell then use it.
            }
        }

        /// <summary>
        ///   Move the fighter.
        /// </summary>
        public void MoveFighter(FightGroupMember fighter, MovementPath movement)
        {
            var requiredMp = movement.MpCost;
            if ((fighter.TotalMp - requiredMp) < 0)
            {
                // send a message error
                return;
            }

            fighter.UsedMp += requiredMp;
            var delta = (short) -requiredMp;

            fighter.Position.ChangeLocation(movement.End);
            var movementsKeys = movement.GetServerMovementKeys();

            Action<Character> action = charac =>
            {
                ActionsHandler.SendSequenceStartMessage(charac.Client, fighter.Entity, SequenceTypeEnum.SEQUENCE_MOVE);
                ContextHandler.SendGameMapMovementMessage(charac.Client, movementsKeys, fighter.Entity);
                ActionsHandler.SendGameActionFightPointsVariationMessage(charac.Client,
                                                                         ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE,
                                                                         fighter.Entity, fighter.Entity, delta);
                ActionsHandler.SendSequenceEndMessage(charac.Client, fighter.Entity, SequenceTypeEnum.SEQUENCE_MOVE);
            };
            CallOnAllCharacters(action);
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
        ///   Check if a character can change position (before fight started only).
        /// </summary>
        /// <param name = "fighter"></param>
        /// <param name = "cellId">The cellId wanted</param>
        /// <returns>If change is possible</returns>
        public bool CanChangePosition(FightGroupMember fighter, ushort cellId)
        {
            return !Started && ((FightGroup) fighter.GroupOwner).Positions.Contains(cellId);
        }

        public bool ChangePosition(FightGroupMember fighter, ushort cellId)
        {
            if (CanChangePosition(fighter, cellId))
            {
                fighter.Position.ChangeLocation(fighter.Entity.Map.CellsData[cellId]);

                return true;
            }

            return false;
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
        /// <param name = "chrId">The character's id of the fighter</param>
        /// <returns>The fightGroupMember of the character, else null</returns>
        public FightGroupMember GetGroupMemberByCharacter(int chrId)
        {
            return SourceGroup.GetMemberByCharacter(chrId) ?? TargetGroup.GetMemberByCharacter(chrId);
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
            if (m_fighters == null ||
                m_fighters.Count() == 0)
                m_fighters = SourceGroup.Members.Select(entry => (FightGroupMember) entry).Concat(
                    TargetGroup.Members.Select(entry => (FightGroupMember) entry));

            return m_fighters;
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
            if (m_characters == null ||
                m_characters.Count() == 0)
                m_characters = from entry in GetAllFighters()
                               where entry.Entity is Character
                               select entry.Entity as Character;

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
                return State != FightState.AskingForDuel &&
                       State != FightState.PreparePosition;
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

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_timeline.Dispose();

                if (m_characters != null)
                    foreach (var character in m_characters)
                    {
                        character.LoggingOut -= OnLoggedOut;
                    }

                m_disposed = true;
            }
        }
    }
}
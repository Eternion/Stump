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
using System.Threading;
using System.Threading.Tasks;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Manager;
using Stump.Server.WorldServer.Effects.Executor;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;
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

    public class Fight : IInstance
    {
        #region Fields

        private const int TurnTime = 35000; // 35s
        private const int TurnEndTimeOut = 5000; // 5s

        private bool m_hasPassed;
        private DateTime m_startTime;
        private Task m_task;
        private TimeLine m_timeline;

        #endregion

        #region Constructor

        /// <summary>
        ///   Create a fight with source as Team 0 and target as Team 1 and not started yet.
        /// </summary>
        /// <param name = "source">A first group</param>
        /// <param name = "target">An other group</param>
        public Fight(FightGroup source, FightGroup target)
        {
            SourceGroup = source;
            SourceGroup.TeamId = 0;
            SourceGroup.Fight = this;
            TargetGroup = target;
            TargetGroup.TeamId = 1;
            TargetGroup.Fight = this;
            Started = false;

            SetFightState(FightState.PreparePosition);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Set the ready state of a character
        /// </summary>
        /// <param name = "chr"></param>
        /// <param name = "isReady"></param>
        public void SetReadyState(Character chr, bool isReady)
        {
            if (chr.GroupMember == null)
                return; //Error
            if (!(chr.GroupMember is FightGroupMember))
                return;
            if (isReady ^ (chr.GroupMember as FightGroupMember).IsReady)
                (chr.GroupMember as FightGroupMember).IsReady = isReady;
            else
                return; // Error

            CallOnAllCharacters(
                charac => { FightHandler.SendGameFightHumanReadyStateMessage(charac.Client, chr, isReady); });

            if (SourceGroup.AllAreReady() && TargetGroup.AllAreReady())
                StartFight();
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
        ///   Start a fight, in combat time.
        /// </summary>
        /// <param name = "fight"></param>
        private void StartFight()
        {
            CallOnAllCharacters((Character charac) =>
            {
                FightHandler.SendGameEntitiesDispositionMessage(charac.Client, this);
                FightHandler.SendGameFightStartMessage(charac.Client);
                FightHandler.SendGameFightTurnListMessage(charac.Client, this);
                BasicHandler.SendBasicNoOperationMessage(charac.Client);
                FightHandler.SendGameFightSynchronizeMessage(charac.Client, this);
                CharacterHandler.SendCharacterStatsListMessage(charac.Client);
            });
            TurnStart();
        }

        /// <summary>
        ///   Starting a fight, from map to preparation time in fight
        /// </summary>
        public void StartingFight()
        {
            var group = new FightGroup[2] {SourceGroup, TargetGroup};

            SourceGroup.Positions = new short[] {370, 355, 354};
            // TODO : write a routine that found correct pre-fight placements
            TargetGroup.Positions = new short[] {328, 356, 357};

            ChangePosition(SourceGroup.Members.First() as FightGroupMember, SourceGroup.Positions.First());
            ChangePosition(TargetGroup.Members.First() as FightGroupMember, TargetGroup.Positions.First());

            SetFightState(FightState.PreparePosition);
            Parallel.For(0, 2, (i) => PrepareFight(group[i].Members.First().Entity as Character, group[i]));
        }

        /// <summary>
        ///   Prepare to Fight method, Showing cells and characters.
        /// </summary>
        /// <param name = "charac"></param>
        /// <param name = "group"></param>
        private void PrepareFight(Character charac, FightGroup group)
        {
            var chrSource = SourceGroup.Members.First().Entity as Character;
            var chrTarget = TargetGroup.Members.First().Entity as Character;

            // TODO : Show "swords"
            charac.Map.OnFightEnter(charac);

            CharacterHandler.SendGameContextDestroyMessage(charac.Client);
            CharacterHandler.SendGameContextCreateMessage(charac.Client, 2);

            FightHandler.SendGameFightStartingMessage(charac.Client);
            FightHandler.SendGameFightJoinMessage(charac.Client);
            FightHandler.SendGameFightPlacementPossiblePositionsMessage(charac.Client, this, group.TeamId);

            CharacterHandler.SendLifePointsRegenEndMessage(charac.Client);

            FightHandler.SendGameFightShowFighterMessage(charac.Client, chrTarget.CurrentFighter, TargetGroup.TeamId);
            FightHandler.SendGameFightShowFighterMessage(charac.Client, chrSource.CurrentFighter, SourceGroup.TeamId);

            FightHandler.SendGameEntitiesDispositionMessage(charac.Client, this);

            FightHandler.SendGameFightUpdateTeamMessage(charac.Client, chrSource, group.Id, SourceGroup.TeamId);
            FightHandler.SendGameFightUpdateTeamMessage(charac.Client, chrTarget, group.Id, TargetGroup.TeamId);
        }

        public void FinishTurn()
        {
            m_hasPassed = true;
        }

        /// <summary>
        /// </summary>
        /// <param name = "character"></param>
        public void QuitFight(Character character)
        {
            FightGroupMember fighter = GetGroupMemberByCharacter((int) character.Id);
            if (fighter == null)
            {
                // error
                return;
            }

            if (!fighter.IsDead)
            {
                CallOnAllCharacters((Character charac) =>
                {
                    FightHandler.SendSequenceStartMessage(charac.Client, SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
                    FightHandler.SendGameActionFightDeathMessage(charac.Client, character);
                    FightHandler.SendSequenceEndMessage(charac.Client, SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
                });
                fighter.HasLeft = true;
            }

            CheckIfEnd();
        }

        /// <summary>
        ///   Use the required spell by the fighter.
        /// </summary>
        /// <param name = "character"></param>
        /// <param name = "cellId"></param>
        /// <param name = "spellId"></param>
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
                // todo critic/fail hit
                bool echec = false;

                if (echec)
                {
                }

                bool critical = false;

                FightEffectExecutor.ExecuteSpellEffects(spellLevel, this, caster, cell, critical);

                // TODO : Check range.
                // TODO: Get effect of spell then use it.
            }
        }

        public bool CanCastSpell(FightGroupMember caster, CellData cell, SpellLevel spell)
        {
            return true;
        }

        /// <summary>
        ///   Move the fighter.
        /// </summary>
        /// <param name = "character"></param>
        /// <param name = "cellId"></param>
        /// <param name = "keyMovements"></param>
        public void MoveFighter(Character character, int cellId, List<int> keyMovements)
        {
            FightGroupMember fighter = GetGroupMemberByCharacter((int) character.Id);

            // TODO : PATHFINDER AND CALC requiredMP (current's false).
            var requiredMp = (short) (keyMovements.Count - 1);
            if ((fighter.TotalMp - requiredMp) < 0)
            {
                // Envoyer erreur "deplacement impossible car vous avez x pm et il vous en faut y pour ce déplacement.";
                return;
            }
            fighter.UsedMp += requiredMp;
            var delta = (short) -requiredMp;

            fighter.Cell = fighter.Entity.Map.CellsData[cellId];

            Action<Character> action = (Character charac) =>
            {
                FightHandler.SendSequenceStartMessage(charac.Client, SequenceTypeEnum.SEQUENCE_MOVE);
                MovementHandler.SendGameMapMovementMessage(charac.Client, keyMovements, character);
                FightHandler.SendGameActionFightPointsVariation(charac.Client, character, delta);
                FightHandler.SendSequenceEndMessage(charac.Client, SequenceTypeEnum.SEQUENCE_MOVE);
            };
            CallOnAllCharacters(action);
        }

        /// <summary>
        ///   Create the TimeLine and Start the fight.
        /// </summary>
        public void TurnStart()
        {
            m_timeline = new TimeLine(this);
            m_task = new Task(() =>
            {
                while (true)
                {
                    FightGroupMember next = m_timeline.GetNext();
                    CallOnAllCharacters(
                        charac =>
                        FightHandler.SendGameFightTurnStartMessage(charac.Client, (int) next.Entity.Id, TurnTime));

                    // start turn ...

                    m_hasPassed = false;
                    DateTime startTime = DateTime.Now;
                    do
                    {
                        Thread.Sleep(10);
                    } while (!(m_hasPassed || (DateTime.Now - startTime).TotalMilliseconds > TurnTime));

                    m_hasPassed = false;

                    CallOnAllCharacters((Character charac) =>
                    {
                        FightHandler.SendGameFightTurnEndMessage(charac.Client, next.Entity);
                        FightHandler.SendGameFightTurnReadyRequestMessage(charac.Client, next.Entity);
                    });

                    // turn ended now -> request if ready to start next turn

                    next.ReadyTurnEnd = false;
                    startTime = DateTime.Now;
                    do
                    {
                        Thread.Sleep(10);
                    } while (!(next.ReadyTurnEnd || (DateTime.Now - startTime).TotalMilliseconds > TurnEndTimeOut));

                    // reset properties and send fighters info (hp, ap, mp ...)

                    next.ResetUsedProperties();

                    CallOnAllCharacters(
                        (Character charac) => { FightHandler.SendGameFightSynchronizeMessage(charac.Client, this); });
                }
            });
            m_task.Start();
            m_startTime = DateTime.Now;
        }

        public bool CheckIfEnd()
        {
            if (SourceGroup.GetAlivesIds().Length == 0 || TargetGroup.GetAlivesIds().Length == 0)
            {
                EndFight();

                return true;
            }

            return false;
        }

        public void EndFight()
        {
            SetFightState(FightState.Ended);

            CallOnAllCharacters(charac =>
            {
                FightHandler.SendGameFightEndMessage(charac.Client, this);

                CharacterHandler.SendGameContextDestroyMessage(charac.Client);
                CharacterHandler.SendGameContextCreateMessage(charac.Client, 1);
                CharacterHandler.SendCharacterStatsListMessage(charac.Client);
                CharacterHandler.SendLifePointsRegenBeginMessage(charac.Client);

                charac.Map.OnFightLeave(charac);
                MapHandler.SendCurrentMapMessage(charac.Client, charac.Map.Id);
            });

            GroupManager.RemoveGroup(SourceGroup);
            GroupManager.RemoveGroup(TargetGroup);

            FightManager.RemoveFight(this);
        }

        /// <summary>
        ///   Change the actual state of the fight
        /// </summary>
        /// <param name = "state"></param>
        public void SetFightState(FightState state)
        {
            FightState = state;
        }

        /// <summary>
        ///   Gets the ids of alive entites.
        /// </summary>
        /// <returns></returns>
        public int[] GetAlivesIds()
        {
            if (!Started)
                return new int[0];

            var result = new List<int>();
            result.AddRange(SourceGroup.GetAlivesIds());
            result.AddRange(TargetGroup.GetAlivesIds());

            return result.ToArray();
        }

        /// <summary>
        ///   Gets the ids of dead entites.
        /// </summary>
        /// <returns></returns>
        public int[] GetDeadsIds()
        {
            if (!Started)
                return new int[0];

            var result = new List<int>();
            result.AddRange(SourceGroup.GetDeadsIds());
            result.AddRange(TargetGroup.GetDeadsIds());

            return result.ToArray();
        }

        /// <summary>
        ///   Check if a character can change position (before fight started only).
        /// </summary>
        /// <param name = "fighter"></param>
        /// <param name = "cellId">The cellId wanted</param>
        /// <returns>If change is possible</returns>
        public bool CanChangePosition(FightGroupMember fighter, short cellId)
        {
            if (FightState != FightState.PreparePosition && FightState != FightState.AskingForDuel)
                return false;

            return ((FightGroup) fighter.GroupOwner).Positions.Contains(cellId);
        }

        public bool ChangePosition(FightGroupMember fighter, short cellId)
        {
            if (CanChangePosition(fighter, cellId))
            {
                fighter.Cell = fighter.Entity.Map.CellsData[cellId];

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
            return SourceGroup.Id == groupId ? SourceGroup : TargetGroup;
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
            Character[] chars = GetAllCharacters();
            Parallel.For(0, chars.Length, i => action(chars[i]));

            return;
        }

        public FightGroupMember[] GetAllFighters()
        {
            return
                SourceGroup.Members.Select(entry => (FightGroupMember) entry).Concat(
                    TargetGroup.Members.Select(entry => (FightGroupMember) entry)).ToArray();
        }

        public FightGroupMember[] GetAllFighters(Func<FightGroupMember, bool> predicate)
        {
            return GetAllFighters().Where(predicate).ToArray();
        }

        public FightGroupMember[] GetAllFighters(CellData cell)
        {
            return GetAllFighters(entry => entry.Cell == cell);
        }

        public FightGroupMember GetOneFighter(Func<FightGroupMember, bool> predicate)
        {
            return GetAllFighters().Single(predicate);
        }

        public FightGroupMember GetOneFighter(CellData cell)
        {
            return GetOneFighter(entry => entry.Cell == cell);
        }

        /// <summary>
        ///   Get all entities contains in both sides groups
        /// </summary>
        /// <returns></returns>
        public Entity[] GetAllEntities()
        {
            return GetAllFighters().Select(entry => entry.Entity).ToArray();
        }

        /// <summary>
        ///   Get all characters contains in both sides groups
        /// </summary>
        /// <returns></returns>
        public Character[] GetAllCharacters()
        {
            return GetAllFighters().Select(entry => entry.Entity as Character).ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   The age bonus (stars) of the fight.
        /// </summary>
        public int AgeBonus
        {
            get;
            set;
        }

        /// <summary>
        ///   Duration of the fight.
        /// </summary>
        public int Duration
        {
            get { return (int) (DateTime.Now - m_startTime).TotalMilliseconds; }
        }

        /// <summary>
        ///   FightGroup of the sourceGroupe (also named challengers)
        /// </summary>
        public FightGroup SourceGroup
        {
            get;
            set;
        }

        /// <summary>
        ///   FightGroup of the targetGroup (alors named defenders)
        /// </summary>
        public FightGroup TargetGroup
        {
            get;
            set;
        }

        /// <summary>
        ///   Combat is started or in prepare.
        /// </summary>
        public bool Started
        {
            get;
            set;
        }

        public FightState FightState
        {
            get;
            set;
        }

        public FightGroupMember CurrentFighter
        {
            get { return m_timeline != null ? m_timeline.Current : null; }
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
    }
}
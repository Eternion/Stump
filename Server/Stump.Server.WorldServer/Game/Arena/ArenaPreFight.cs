using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Arena;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaPreFight
    {

        private WorldClientCollection m_clients = new WorldClientCollection();

        private ArenaPreFightTeam m_team1;
        private ArenaPreFightTeam m_team2;
        private Dictionary<Character, Map> m_charactersMaps = new Dictionary<Character, Map>();

        private ArenaFight m_fight;

        public ArenaPreFight(int id, ArenaRecord arena)
        {
            Id = id;
            Arena = arena;
            m_team1 = new ArenaPreFightTeam(TeamEnum.TEAM_DEFENDER, this);
            m_team2 = new ArenaPreFightTeam(TeamEnum.TEAM_CHALLENGER, this);

            m_team1.MemberAdded += OnMemberAdded;
            m_team2.MemberAdded += OnMemberAdded;

            m_team1.MemberRemoved += OnMemberRemoved;
            m_team2.MemberRemoved += OnMemberRemoved;
        }

        public int Id
        {
            get;
            private set;
        }

        public WorldClientCollection Clients
        {
            get { return m_clients; }
        }

        public ArenaRecord Arena
        {
            get;
            private set;
        }

        public ArenaPreFightTeam DefendersTeam
        {
            get { return m_team1; }
        }

        public ArenaPreFightTeam ChallengersTeam
        {
            get { return m_team2; }
        }

        public bool IsInQueue
        {
            get;
            private set;
        }

        public int AverageElo
        {
            get
            {
                return DefendersTeam.Members.Concat(ChallengersTeam.Members).Sum(x => x.Character.ArenaRank)/
                       (DefendersTeam.Members.Count + ChallengersTeam.Members.Count);
            }
        }

        public DateTime InQueueSince
        {
            get;
            private set;
        }

        public int MaxMatchableRank
        {
            get { return (int) (AverageElo + ArenaQueueMember.ArenaMargeIncreasePerMinutes*(DateTime.Now - InQueueSince).TotalMinutes); }
        }
        
        public int MinMatchableRank
        {
            get { return (int) (AverageElo - ArenaQueueMember.ArenaMargeIncreasePerMinutes*(DateTime.Now - InQueueSince).TotalMinutes); }
        }

        public bool IsCompatibleWith(ArenaQueueMember member)
        {
            return (ChallengersTeam.MissingMembers >= member.MembersCount ||
                    DefendersTeam.MissingMembers >= member.MembersCount) &&
                   (member.MinMatchableRank < MinMatchableRank || member.MaxMatchableRank > MaxMatchableRank);
        }

        public bool ReplaceMissings(ArenaQueueMember member)
        {
            var team = (ChallengersTeam.MissingMembers >= member.MembersCount) ? ChallengersTeam : DefendersTeam;

            if (team.MissingMembers < member.MembersCount)
                return false;

            foreach (var character in member.EnumerateCharacters())
                team.AddCharacter(character);

            if (ChallengersTeam.MissingMembers == 0 && DefendersTeam.MissingMembers == 0)
            {
               
                IsInQueue = false;
                ShowPopups();
                return true;
            }

            return false;
        }

        public void ShowPopups()
        { 
            foreach (var character in DefendersTeam.Members)
            {
                var popup = new ArenaPopup(character);
                popup.Display();
            }            
            
            foreach (var character in ChallengersTeam.Members)
            {
                var popup = new ArenaPopup(character);
                popup.Display();
            }
        }

        private void OnMemberRemoved(ArenaPreFightTeam arg1, ArenaWaitingCharacter arg2)
        {
            arg2.ReadyChanged -= OnReadyChanged;
            arg2.FightDenied -= OnFightDenied;

            m_clients.Remove(arg2.Character.Client);

            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(Clients, false,
                    PvpArenaStepEnum.ARENA_STEP_UNREGISTER, PvpArenaTypeEnum.ARENA_TYPE_3VS3);

        }

        private void OnMemberAdded(ArenaPreFightTeam arg1, ArenaWaitingCharacter arg2)
        {
            arg2.ReadyChanged += OnReadyChanged;
            arg2.FightDenied += OnFightDenied;

            m_clients.Add(arg2.Character.Client);
        }

        private void OnFightDenied(ArenaWaitingCharacter obj)
        {
            ArenaManager.Instance.ArenaTaskPool.ExecuteInContext(() =>
            {
                if (obj.Character.ArenaParty != null)
                {
                    foreach (var character in obj.Team.Members.ToArray())
                    {
                        obj.Team.RemoveCharacter(character);
                    }

                    obj.Character.ArenaParty.RemoveMember(obj.Character);
                }
                else
                    obj.Team.RemoveCharacter(obj);

                if (!IsInQueue)
                {
                    ArenaManager.Instance.AddIncompleteFight(this);
                    InQueueSince = DateTime.Now;
                    IsInQueue = true;

                    foreach (var character in DefendersTeam.Members.Concat(ChallengersTeam.Members))
                    {
                        if (character.Character.ArenaPopup != null)
                            character.Character.ArenaPopup.Cancel();
                    }

                }
            });
        }

        private void OnReadyChanged(ArenaWaitingCharacter character, bool ready)
        {
            ArenaManager.Instance.ArenaTaskPool.ExecuteInContext(() =>
            {
                ContextHandler.SendGameRolePlayArenaFighterStatusMessage(m_clients, Id, character.Character, ready);

                if (!IsInQueue && DefendersTeam.MissingMembers == 0 && ChallengersTeam.MissingMembers == 0 &&
                    DefendersTeam.Members.All(x => x.Ready) && ChallengersTeam.Members.All(x => x.Ready))
                {
                    m_fight = FightManager.Instance.CreateArenaFight(this);

                    TeleportFighters();
                }
            });
        }

        private void TeleportFighters()
        {
            var count = ChallengersTeam.Members.Count + DefendersTeam.Members.Count;
            foreach (var character in ChallengersTeam.Members.Concat(DefendersTeam.Members).Select(x => x.Character))
            {
                var character1 = character;
                character.Area.AddMessage(() =>
                {
                    try
                    {
                        lock(m_charactersMaps)
                            m_charactersMaps.Add(character1, character1.Map);

                        if (character1.IsFighting())
                        {
                            character1.NextMap = m_fight.Map;
                            character1.Fighter.LeaveFight();
                        }
                        else if (character1.IsSpectator())
                        {
                            character1.NextMap = m_fight.Map;
                            character1.Spectator.Leave();
                        }
                        else
                        {
                            character1.Teleport(m_fight.Map, m_fight.Map.Cells[character1.Cell.Id]);
                        }
                    }
                    finally
                    {
                        if (Interlocked.Decrement(ref count) <= 0)
                        {
                            m_fight.Map.Area.AddMessage(PrepareFight);
                        }
                    }
                });
            }
        }

        private void PrepareFight()
        {
            foreach (var character in ChallengersTeam.Members.Select(x => x.Character))
            {
                m_fight.ChallengersTeam.AddFighter(character.CreateFighter(m_fight.ChallengersTeam));
                character.NextMap = m_charactersMaps[character];
            }

            foreach (var character in DefendersTeam.Members.Select(x => x.Character))
            {
                m_fight.DefendersTeam.AddFighter(character.CreateFighter(m_fight.DefendersTeam));
                character.NextMap = m_charactersMaps[character];
            }

            m_fight.StartPlacement();
        }
    }
}
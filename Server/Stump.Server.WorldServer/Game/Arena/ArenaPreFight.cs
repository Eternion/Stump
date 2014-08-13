using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Arena;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaPreFight
    {

        private readonly WorldClientCollection m_clients = new WorldClientCollection();

        private readonly Dictionary<Character, Map> m_charactersMaps = new Dictionary<Character, Map>();
        private int m_readyPlayersCount;

        private ArenaFight m_fight;

        public ArenaPreFight(int id, ArenaRecord arena)
        {
            Id = id;
            Arena = arena;
            DefendersTeam = new ArenaPreFightTeam(TeamEnum.TEAM_DEFENDER, this);
            ChallengersTeam = new ArenaPreFightTeam(TeamEnum.TEAM_CHALLENGER, this);

            DefendersTeam.MemberAdded += OnMemberAdded;
            ChallengersTeam.MemberAdded += OnMemberAdded;

            DefendersTeam.MemberRemoved += OnMemberRemoved;
            ChallengersTeam.MemberRemoved += OnMemberRemoved;
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
            get;
            private set;
        }

        public ArenaPreFightTeam ChallengersTeam
        {
            get;
            private set;
        }

        public void ShowPopups()
        {
            foreach (var popup in DefendersTeam.Members.Select(character => new ArenaPopup(character)))
            {
                popup.Display();
            }

            foreach (var popup in ChallengersTeam.Members.Select(character => new ArenaPopup(character)))
            {
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
                ContextHandler.SendGameRolePlayArenaFighterStatusMessage(m_clients, Id, obj.Character, false);

                // %1 a refusé le combat en Kolizéum.
                BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 275, obj.Character.Name);

                if (obj.Character.ArenaParty != null)
                {
                    foreach (var character in obj.Team.Members.ToArray())
                    {
                        obj.Team.RemoveCharacter(character);
                        // Combat de Kolizéum annulé/non validé par votre équipe
                        character.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 273);
                    }

                    obj.Character.ArenaParty.RemoveMember(obj.Character);
                }
                else
                    obj.Team.RemoveCharacter(obj);

                var opposedTeam = obj.Team == DefendersTeam ? ChallengersTeam : DefendersTeam;
                foreach (var character in opposedTeam.Members)
                {
                    // Combat de Kolizéum annulé/non validé par l'autre équipe.
                    character.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 274);
                }
                
                foreach (var character in DefendersTeam.Members.Concat(ChallengersTeam.Members).Where(character => character.Character.ArenaPopup != null))
                {
                    character.Character.ArenaPopup.Cancel();
                }

                foreach (var character in obj.Team.Members.Where(character => character.Character.ArenaParty == null))
                {
                    ArenaManager.Instance.AddToQueue(character.Character);
                }

                var once = false;
                foreach (var character in opposedTeam.Members)
                {
                    if (character.Character.ArenaParty == null)
                        ArenaManager.Instance.AddToQueue(character.Character);
                    else if (!once)
                    {
                        ArenaManager.Instance.AddToQueue(character.Character.ArenaParty);
                        once = true;
                    }
                }
            });
        }

        private void OnReadyChanged(ArenaWaitingCharacter character, bool ready)
        {
            ArenaManager.Instance.ArenaTaskPool.ExecuteInContext(() =>
            {
                ContextHandler.SendGameRolePlayArenaFighterStatusMessage(m_clients, Id, character.Character, ready);

                if (DefendersTeam.MissingMembers != 0 || ChallengersTeam.MissingMembers != 0 ||
                    !DefendersTeam.Members.All(x => x.Ready) || !ChallengersTeam.Members.All(x => x.Ready))
                    return;

                m_fight = FightManager.Instance.CreateArenaFight(this);

                TeleportFighters();
            });
        }

        private void TeleportFighters()
        {
            // it's a mess to manage all these contexts together
            m_readyPlayersCount = ChallengersTeam.Members.Count + DefendersTeam.Members.Count;
            foreach (var character in ChallengersTeam.Members.Concat(DefendersTeam.Members).Select(x => x.Character))
            {
                var character1 = character;
                character.Area.AddMessage(() =>
                {
                    lock(m_charactersMaps)
                        m_charactersMaps.Add(character1, character1.Map);

                    if (character1.IsFighting())
                    {
                        character1.EnterMap += OnFightLeft;
                        character1.NextMap = m_fight.Map;
                        character1.Fighter.LeaveFight();
                    }
                    else if (character1.IsSpectator())
                    {
                        character1.EnterMap += OnFightLeft;
                        character1.NextMap = m_fight.Map;
                        character1.Spectator.Leave();
                    }
                    else
                    {
                        RemoveShield(character1);

                        character1.Teleport(m_fight.Map, m_fight.Map.Cells[character1.Cell.Id]);
                             
                        if (Interlocked.Decrement(ref m_readyPlayersCount) <= 0)
                        {
                            m_fight.Map.Area.AddMessage(PrepareFight);
                        }
                    }
                });
            }
        }

        private static void RemoveShield(IInventoryOwner character)
        {
            foreach (var item in character.Inventory.GetItems(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD))
            {
                character.Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }
        }

        private void OnFightLeft(RolePlayActor rolePlayActor, Map map)
        {
            if (map != m_fight.Map)
                return;

            RemoveShield(rolePlayActor as Character);

            rolePlayActor.EnterMap -= OnFightLeft;

            if (Interlocked.Decrement(ref m_readyPlayersCount) <= 0)
            {
                m_fight.Map.Area.AddMessage(PrepareFight);
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
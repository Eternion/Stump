using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Arena;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Parties;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaManager : DataManager<ArenaManager>
    {
        [Variable] public static int MaxPlayersPerFights = 3;

        [Variable] public static int ArenaMinLevel = 50;

        [Variable] public static int ArenaMaxLevelDifference = 40;
        /// <summary>
        /// in ms
        /// </summary>
        [Variable] public static int ArenaUpdateInterval = 100;

        /// <summary>
        /// is seconds
        /// </summary>
        [Variable] public static int ArenaMatchmakingInterval = 30;

        /// <summary>
        /// in minutes
        /// </summary>
        [Variable] public static int ArenaPenalityTime = 30;

        /// <summary>
        /// in minutes
        /// </summary>
        [Variable]
        public static int ArenaWaitTime = 10;

        public ItemTemplate TokenItemTemplate
        {
            get
            {
                return m_tokenTemplate ??
                       (m_tokenTemplate = ItemManager.Instance.TryGetTemplate((int) ItemIdEnum.Kolizeton));
            }
        }

        private Dictionary<int, ArenaRecord> m_arenas;
        private readonly SelfRunningTaskPool m_arenaTaskPool = new SelfRunningTaskPool(ArenaUpdateInterval, "Arena");
        private readonly List<ArenaQueueMember> m_queue = new List<ArenaQueueMember>();
        private ItemTemplate m_tokenTemplate;

        [Initialization(InitializationPass.Fifth)]
        public override void Initialize()
        {
            m_arenas = Database.Query<ArenaRecord>(ArenaRelator.FetchQuery).ToDictionary(x => x.Id);
            m_arenaTaskPool.CallPeriodically(ArenaMatchmakingInterval*1000, ComputeMatchmaking);
            m_arenaTaskPool.Start();
        }

        public SelfRunningTaskPool ArenaTaskPool
        {
            get { return m_arenaTaskPool; }
        }

        public Dictionary<int, ArenaRecord> Arenas
        {
            get { return m_arenas; }
        }

        public bool CanJoinQueue(Character character)
        {
            if (m_arenas.Count == 0)
                return false;

            //Already in queue
            if (IsInQueue(character))
                return false;

            return character.CanEnterArena();
        }

        public bool IsInQueue(Character character)
        {
            return m_queue.Exists(x => x.Character == character);
        }

        public bool IsInQueue(ArenaParty party)
        {
            return m_queue.Exists(x => x.Party == party);
        }

        public ArenaQueueMember GetQueueMember(Character character)
        {
            return m_queue.FirstOrDefault(x => x.Character == character);
        }

        public void AddToQueue(Character character)
        {
            if (!CanJoinQueue(character))
                return;
            
            lock (m_queue)
                m_queue.Add(new ArenaQueueMember(character));

            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(character.Client, true,
                PvpArenaStepEnum.ARENA_STEP_REGISTRED, PvpArenaTypeEnum.ARENA_TYPE_3VS3);
        }

        public void RemoveFromQueue(Character character)
        {
            lock (m_queue)
                m_queue.RemoveAll(x => x.Character == character);

            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(character.Client, false, 
                PvpArenaStepEnum.ARENA_STEP_UNREGISTER, PvpArenaTypeEnum.ARENA_TYPE_3VS3);
        }

        public void AddToQueue(ArenaParty party)
        {
            if (!party.Members.All(CanJoinQueue))
                return;
            
            lock (m_queue)
                m_queue.Add(new ArenaQueueMember(party));

            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(party.Clients, true,
                PvpArenaStepEnum.ARENA_STEP_REGISTRED, PvpArenaTypeEnum.ARENA_TYPE_3VS3);

            foreach (var client in party.Clients.Where(client => client != party.Leader.Client))
            {
                //%1 vous a inscrit à un combat en Kolizéum.
                BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 272, party.Leader.Name);
            }
        }

        public void RemoveFromQueue(ArenaParty party)
        {
            lock (m_queue)
                m_queue.RemoveAll(x => x.Party == party);

            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(party.Clients, false, 
                PvpArenaStepEnum.ARENA_STEP_UNREGISTER, PvpArenaTypeEnum.ARENA_TYPE_3VS3);
        }
        public void ComputeMatchmaking()
        {
            List<ArenaQueueMember> queue;
            lock (m_queue)
            {
                queue = m_queue.Where(x => !x.IsBusy()).ToList();
            }

            ArenaQueueMember current;
            while ((current = queue.FirstOrDefault()) != null)
            {
                queue.Remove(current);

                var matchs = queue.Where(x => x.IsCompatibleWith(current)).ToList();
                var allies = new List<ArenaQueueMember> {current};
                var enemies = new List<ArenaQueueMember>();

                var missingAllies = MaxPlayersPerFights - current.MembersCount;
                var i = 0;
                while (missingAllies > 0 && i < matchs.Count)
                {
                    if (matchs[i].MembersCount <= missingAllies)
                    {
                        allies.Add(matchs[i]);
                        missingAllies -= matchs[i].MembersCount;
                        matchs.Remove(matchs[i]);
                    }
                    else
                        i++;
                }

                if (missingAllies > 0)
                    continue;

                var missingEnemies = MaxPlayersPerFights;
                i = 0;
                while (missingEnemies > 0 && i < matchs.Count)
                {
                    if (matchs[i].MembersCount <= missingEnemies)
                    {
                        enemies.Add(matchs[i]);
                        missingEnemies -= matchs[i].MembersCount;
                        matchs.Remove(matchs[i]);
                    }
                    else
                        i++;
                }

                if (missingEnemies > 0)
                    continue;

                // start fight
                StartFight(allies, enemies);

                queue.RemoveAll(x => allies.Contains(x) || enemies.Contains(x));
                lock(m_queue)
                    m_queue.RemoveAll(x => allies.Contains(x) || enemies.Contains(x));
            }
        }

        private void StartFight(IEnumerable<ArenaQueueMember> team1, IEnumerable<ArenaQueueMember> team2)
        {
            var arena = m_arenas.RandomElementOrDefault().Value;
            var preFight = FightManager.Instance.CreateArenaPreFight(arena);

            foreach (var character in team1.SelectMany(x => x.EnumerateCharacters()))
            {
                character.DenyAllInvitations(PartyTypeEnum.PARTY_TYPE_ARENA);
                preFight.DefendersTeam.AddCharacter(character);
            }

            foreach (var character in team2.SelectMany(x => x.EnumerateCharacters()))
            {
                character.DenyAllInvitations(PartyTypeEnum.PARTY_TYPE_ARENA);
                preFight.ChallengersTeam.AddCharacter(character);
            }

            var challengersParty = preFight.ChallengersTeam.Members.Select(x => x.Character.GetParty(PartyTypeEnum.PARTY_TYPE_ARENA)).FirstOrDefault() ??
                                 PartyManager.Instance.Create(PartyTypeEnum.PARTY_TYPE_ARENA);
            var defendersParty = preFight.DefendersTeam.Members.Select(x => x.Character.GetParty(PartyTypeEnum.PARTY_TYPE_ARENA)).FirstOrDefault() ??
                                 PartyManager.Instance.Create(PartyTypeEnum.PARTY_TYPE_ARENA);

            challengersParty.RemoveAllGuest();
            defendersParty.RemoveAllGuest();

            foreach (var character in preFight.ChallengersTeam.Members.Select(x => x.Character).Where(character => !challengersParty.IsInGroup(character)))
            {
                if (challengersParty.Leader != null)
                    challengersParty.Leader.Invite(character, PartyTypeEnum.PARTY_TYPE_ARENA, true);
                else
                    character.EnterParty(challengersParty);
            }

            foreach (var character in preFight.DefendersTeam.Members.Select(x => x.Character).Where(character => !defendersParty.IsInGroup(character)))
            {
                if (defendersParty.Leader != null)
                    defendersParty.Leader.Invite(character, PartyTypeEnum.PARTY_TYPE_ARENA, true);
                else
                    character.EnterParty(defendersParty);
            }

            preFight.ShowPopups();
        }
    }
}
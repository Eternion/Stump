using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Arena;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaManager : DataManager<ArenaManager>
    {
        [Variable] public static int MaxPlayersPerFights = 3;

        [Variable] public static int ArenaMinLevel = 50;
        /// <summary>
        /// in ms
        /// </summary>
        [Variable] public static int ArenaUpdateInterval = 100;

        /// <summary>
        /// is seconds
        /// </summary>
        [Variable] public static int ArenaMatchmakingInterval = 60;

        private Dictionary<int, ArenaRecord> m_arenas;
        private readonly SelfRunningTaskPool m_arenaTaskPool = new SelfRunningTaskPool(ArenaUpdateInterval, "Arena");
        private readonly List<ArenaQueueMember> m_queue = new List<ArenaQueueMember>();
        private readonly List<ArenaPreFight> m_incompleteFights = new List<ArenaPreFight>();
        
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

        public bool CanJoinQueue(Character character)
        {
            if (m_arenas.Count == 0)
                return false;

            if (character.Fight is ArenaFight)
                return false;

            //Already in queue
            if (m_queue.Exists(x => x.Character == character))
                return false;

            return character.Level >= ArenaMinLevel;
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
        }

        public void RemoveFromQueue(ArenaParty party)
        {
            lock (m_queue)
                m_queue.RemoveAll(x => x.Party == party);

            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(party.Clients, false, 
                PvpArenaStepEnum.ARENA_STEP_UNREGISTER, PvpArenaTypeEnum.ARENA_TYPE_3VS3);
        }

        public void AddIncompleteFight(ArenaPreFight preFight)
        {
            lock (m_incompleteFights)
                m_incompleteFights.Add(preFight);

            ContextHandler.SendGameRolePlayArenaRegistrationStatusMessage(preFight.Clients, true,
                PvpArenaStepEnum.ARENA_STEP_REGISTRED, PvpArenaTypeEnum.ARENA_TYPE_3VS3);
        }

        public void ComputeMatchmaking()
        {
            List<ArenaQueueMember> queue;
            lock (m_queue)
            {
                queue = m_queue.ToList();
            }

            ArenaQueueMember current;
            while ((current = queue.FirstOrDefault()) != null)
            {
                queue.Remove(current);

                lock(m_incompleteFights)
                {
                    m_incompleteFights.RemoveAll(
                        x => x.ChallengersTeam.Members.Count == 0 && x.DefendersTeam.Members.Count == 0);

                    var incompleteFightMatch = m_incompleteFights.Where(x => x.IsCompatibleWith(current)).OrderBy(x => Math.Abs(x.AverageElo - current.ArenaRank)).FirstOrDefault();

                    if (incompleteFightMatch != null)
                    {
                        if (incompleteFightMatch.ReplaceMissings(current))
                            m_incompleteFights.Remove(incompleteFightMatch);
                    }
                }

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
                preFight.DefendersTeam.AddCharacter(character);
            }

            foreach (var character in team2.SelectMany(x => x.EnumerateCharacters()))
            {
                preFight.ChallengersTeam.AddCharacter(character);
            }

            preFight.ShowPopups();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Arena;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;

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
        private SelfRunningTaskPool m_arenaTaskPool = new SelfRunningTaskPool(ArenaUpdateInterval, "Arena");
        private List<ArenaQueueMember> m_queue = new List<ArenaQueueMember>();
        
        [Initialization(InitializationPass.Fifth)]
        public override void Initialize()
        {
            m_arenas = Database.Query<ArenaRecord>(ArenaRelator.FetchQuery).ToDictionary(x => x.Id);
            m_arenaTaskPool.CallPeriodically(ArenaMatchmakingInterval*1000, ComputeMatchmaking);
        }


        public bool CanJoinQueue(Character character)
        {
            if (m_arenas.Count == 0)
                return false;

            //not in arena fight

            if (character.Level < ArenaMinLevel)
                return false;

            return true;
        }

        public void AddToQueue(Character character)
        {
            if (!CanJoinQueue(character))
                return;
            
            lock (m_queue)
                m_queue.Add(new ArenaQueueMember(character));
        }

        public void RemoveFromQueue(Character character)
        {
            lock (m_queue)
                m_queue.RemoveAll(x => x.Character == character);
        }

        public void ComputeMatchmaking()
        {
            var queue = m_queue.ToList();
            ArenaQueueMember current;

            current = queue.FirstOrDefault();
            while (current != null)
            {
                queue.Remove(current);

                var matchs = queue.Where(x => x.IsCompatibleWith(current)).ToList();
                var allies = new List<ArenaQueueMember>() {current};
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

                m_queue.RemoveAll(x => allies.Contains(x) || enemies.Contains(x));

                current = m_queue.FirstOrDefault();
            }
        }

        private void StartFight(IEnumerable<ArenaQueueMember> team1, IEnumerable<ArenaQueueMember> team2)
        {
            var arena = m_arenas.RandomElementOrDefault().Value;
            var fight = FightManager.Instance.CreateArenaFight(arena.Map);

            foreach (var member in team1)
            {
                fight.BlueTeam.AddQueueMember(member);
            }

            foreach (var member in team2)
            {
                fight.RedTeam.AddQueueMember(member);
            }
        }
    }
}
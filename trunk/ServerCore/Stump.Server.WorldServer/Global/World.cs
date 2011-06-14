
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Reflection;
using Stump.BaseCore.Framework.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Chat;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Manager;

namespace Stump.Server.WorldServer.Global
{
    public partial class World : Singleton<World>
    {
        #region Fields

        #region Variables

        private static long m_worldUpdateInterval = 100;

        /// <summary>
        /// Welcome message
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static string MessageOfTheDay = "Bienvenue. Ce serveur est propulsé par Stump.";

        [Variable(DefinableRunning = true)]
        public static long WorldUpdateInterval
        {
            get { return m_worldUpdateInterval; }
            set
            {
                m_worldUpdateInterval = value;
                if (Instance != null && Instance.TaskPool != null)
                    Instance.TaskPool.ChangeUpdateFrequency(m_worldUpdateInterval);
            }
        }

        #endregion

        private static readonly object m_syncObject = new object();
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Character's Map where you retrieve a character by his Id.
        /// </summary>
        private readonly ConcurrentDictionary<long, Character> m_charactersId =
            new ConcurrentDictionary<long, Character>();

        /// <summary>
        ///   Character's Map where you retrieve a character by his name.
        /// </summary>
        private readonly ConcurrentDictionary<string, Character> m_charactersName =
            new ConcurrentDictionary<string, Character>();

        /// <summary>
        ///   Task pool which will handle and execute all messages given.
        /// </summary>
        private AsyncTaskPool m_taskPool;

        #endregion

        public void Start()
        {
            Running = true;
        }

        #region Character Management

        /// <summary>
        ///   Save all characters in world.
        /// </summary>
        public void Save()
        {
            if (Running)
            {
                lock (m_syncObject)
                {
                    logger.Info("Saving World...");

                    Parallel.ForEach(m_charactersId.Values, entry => entry.SaveNow());

                    logger.Info("Saved World !");
                }
            }
        }

        public bool IsCharacterOnline(string chrname)
        {
            return m_charactersName.ContainsKey(chrname);
        }

        public Character GetCharacter(string name)
        {
            Character chr;

            return m_charactersName.TryGetValue(name, out chr) ? chr : null;
        }

        public Character GetCharacter(long id)
        {
            Character[] result = m_charactersName.Values.Where(entry => entry.Id == id).ToArray();

            return result.Length > 0 ? result.First() : null;
        }

        /// <summary>
        /// Get a character by a search pattern. *account = current character used by account, name = character by his name.
        /// </summary>
        /// <returns></returns>
        public Character GetCharacterByPattern(string pattern)
        {
            if (pattern[0] == '*')
            {
                string name = pattern.Remove(0, 1);

                var characters = from entry in WorldServer.Instance.GetClients()
                                 where entry.Account.Login == name
                                 select entry.ActiveCharacter;

                return characters.FirstOrDefault();
            }

            return GetCharacter(pattern);
        }

        /// <summary>
        /// Get a character by a search pattern. * = caller, *(account) = current character used by account, name = character by his name.
        /// </summary>
        /// <returns></returns>
        public Character GetCharacterByPattern(Character caller, string pattern)
        {
            if (pattern == "*")
                return caller;

            return GetCharacterByPattern(pattern);
        }

        public void CallOnAllCharacters(Action<Character> action)
        {
            Parallel.ForEach(m_charactersId, entry => action(entry.Value));
        }

        /// <summary>
        ///   Add a character to the world.
        /// </summary>
        /// <param name = "chr">the character to add</param>
        public void AddCharacter(Character chr)
        {
            if (m_charactersId.TryAdd(chr.Id, chr) &&
                m_charactersName.TryAdd(chr.Name, chr))
                Interlocked.Increment(ref m_characterCount);
        }

        /// <summary>
        ///   Removes a character from the world manager.
        /// </summary>
        /// <param name = "chr">the character to stop tracking</param>
        public void RemoveCharacter(Character chr)
        {
            Character dummy;

            if (m_charactersId.TryRemove(chr.Id, out dummy) &&
                m_charactersName.TryRemove(chr.Name, out dummy))
                Interlocked.Decrement(ref m_characterCount);
        }

        #endregion

        #region Zones Getters

        public Zone GetZone(int zoneid)
        {
            return Zones[zoneid];
        }

        public Zone RetrieveZoneByMapId(int mapid)
        {
            Map map;
            if (Maps.TryGetValue(mapid, out map))
            {
                return Zones[map.ParentSpace.Id];
            }
            return null;
        }

        #endregion

        #region Message Of The Day

        public void SendMessageOfTheDay(Character character)
        {
            Handlers.ChatHandler.SendChatServerMessage(character.Client, MessageOfTheDay);
        }

        #endregion

        #region ...Maps Getters

        /// <summary>
        ///   Get map with the given map id.
        /// </summary>
        /// <returns></returns>
        public Map GetMap(int mapid)
        {
            Map map;

            return Maps.TryGetValue(mapid, out map) ? map : null;
        }

        /// <summary>
        ///   Get map with the given map id.
        /// </summary>
        /// <returns></returns>
        public Map GetMap(uint mapid)
        {
            Map map;

            return Maps.TryGetValue((int) mapid, out map) ? map : null;
        }

        /// <summary>
        ///   Get maps with the given map's id list.
        /// </summary>
        /// <returns></returns>
        public Map[] GetMaps(IEnumerable<uint> mapsids)
        {
            return mapsids.Select(GetMap).Where(map => map != null).ToArray();
        }

        #endregion

        #region Properties

        private int m_characterCount;

        public bool Running
        {
            get;
            set;
        }

        public AsyncTaskPool TaskPool
        {
            get { return m_taskPool; }
        }


        public int CharacterCount
        {
            get { return m_characterCount; }
            private set { m_characterCount = value; }
        }

        public Dictionary<int, Continent> Continents
        {
            get;
            set;
        }

        public Dictionary<int, Region> Regions
        {
            get;
            set;
        }

        public ConcurrentDictionary<int, Map> Maps
        {
            get;
            set;
        }

        public Dictionary<int, Zone> Zones
        {
            get;
            set;
        }

        #endregion
    }
}
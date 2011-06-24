
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Database.Data.World;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global
{
    public class Region
    {
        #region Fields


        /// <summary>
        ///   Synchronization object to avoid behaviors since we are in a multi threaded environment.
        /// </summary>
        protected ReaderWriterLockSlim Sync = new ReaderWriterLockSlim();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Numbers of characters in this region. (Stats purpose)
        /// </summary>
        private int m_charactercount;

        private bool m_monstersLoaded;
        private bool m_npcsLoaded;

        //private bool m_ContainHouses;
        //private bool m_ContainPaddocks;

        #endregion

        /// <summary>
        ///   Constructor.
        /// </summary>
        public Region(AreaRecord record, Continent parent)
        {
            Record = record;
            Parent = parent;
            Parent.Childrens.Add(this);
        }

        public Continent Parent
        {
            get;
            private set;
        }

        public List<Zone> Childrens
        {
            get;
            internal set;
        }

        #region Entities's Spawning

        public void SpawnRegion()
        {
            LoadNpcs();
            LoadMonsters();
        }

        private void LoadNpcs()
        {
            if (!m_npcsLoaded)
            {
                // should we lazy load the npcs ?
                
                m_npcsLoaded = true;
            }
        }

        private void LoadMonsters()
        {
            if (!m_monstersLoaded)
            {
                // todo

                m_monstersLoaded = true;
            }
        }

        #endregion

        #region Region's Events

        /// <summary>
        ///   Called when a entity enter this region.
        /// </summary>
        internal void OnCharacterEnter()
        {
                Interlocked.Increment(ref m_charactercount);
                if (!IsRunning)
                {
                    Start();
                }
        }

        public void OnCharacterLeave(Character character)
        {
                Interlocked.Decrement(ref m_charactercount);

                if (m_charactercount <= 0 &&
                    character.NextMap == null ||
                     character.NextMap.Parent.Parent != this)
                    Stop();
        }

        #endregion

        #region Start/Stop

        /// <summary>
        ///   Start region's context.
        /// </summary>
        public void Start()
        {
            lock (Sync)
            {
                if (!IsRunning)
                {
                    logger.Info("Started {0}.", this);
                    IsRunning = true;

                    SpawnRegion();
                }
            }
        }

        public void Stop()
        {
            lock (Sync)
            {
                logger.Info("Stopped {0}.", this);

                foreach (var character in from zone in Childrens
                                          let maps = zone.Childrens
                                          from map in maps
                                          select map.Characters)
                {
                    character.Client.Disconnect();
                }

                IsRunning = false;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Region {0}",
                                 Id);
        }

        #region Properties

        public AreaRecord Record
        {
            get;
            private set;
        }

        public int Id
        {
            get
            {
                return Record.Id;
            }
        }

            public bool IsRunning
        {
            get;
            set;
        }

        #endregion
    }
}
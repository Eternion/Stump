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
using System.Threading;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global
{
    public class Region : WorldSpace
    {
        #region Fields

        /// <summary>
        ///   Numbers of characters in this region. (Stats purpose)
        /// </summary>
        private int m_charactercount;

        private bool m_monstersLoaded;
        private bool m_npcsLoaded;

        //private bool m_ContainHouses;
        //private bool m_ContainPaddocks;
        /// <summary>
        ///   Determines if this area contains houses.
        /// </summary>
        /// <summary>
        ///   Determines if this area contains paddocks.
        /// </summary>
        /// <summary>
        ///   Indicate if region is started.
        /// </summary>
        private bool m_running;

        #endregion

        /// <summary>
        ///   Constructor.
        /// </summary>
        public Region()
        {
            m_charactercount = 0;
            m_npcsLoaded = false;
            m_monstersLoaded = false;
        }

        public override WorldSpaceType Type
        {
            get { return WorldSpaceType.Region; }
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
                // todo
                m_npcsLoaded = true;
            }
        }

        private void LoadMonsters()
        {
            if (!m_monstersLoaded)
            {
                OnMonsterSpawning();
                m_monstersLoaded = true;
            }
        }

        #endregion

        #region Region's Events

        /// <summary>
        ///   Called when a entity enter this region. (Actually only characters)
        /// </summary>
        /// <param name = "entity"></param>
        public override void OnEnter(Entity entity)
        {
            base.OnEnter(entity);

            ParentSpace.OnEnter(entity);
            entity.Region = this;

            if (!m_running)
            {
                Start();
            }

            Interlocked.Increment(ref m_charactercount);
        }

        public override void OnLeave(Entity entity)
        {
            base.OnLeave(entity);

            ParentSpace.OnLeave(entity);
            if (entity is Character)
            {
                Interlocked.Decrement(ref m_charactercount);
                if (m_charactercount == 0 &&
                    ((entity as Character).NextMap == null ||
                     ((entity as Character).NextMap.ParentSpace.ParentSpace as Region) != this))
                    Stop();
            }
        }

        #endregion

        #region Start/Stop

        /// <summary>
        ///   Start region's context.
        /// </summary>
        public void Start()
        {
            if (!m_running)
            {
                logger.Info("Started {0}.", this);
                m_running = true;

                SpawnRegion();
            }
        }

        public void Stop()
        {
            logger.Info("Stopped {0}.", this);
            // Todo : disconnect characters in case region was stopped by admin
            m_running = false;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Region {0} (Id: {1})",
                                 Name,
                                 Id);
        }

        #region Properties

        public bool IsRunning
        {
            get { return m_running; }
            set { m_running = value; }
        }

        #endregion
    }
}
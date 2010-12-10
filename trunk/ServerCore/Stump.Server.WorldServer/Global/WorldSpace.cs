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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global
{
    public abstract class WorldSpace : IEntityContainer
    {
        #region Fields

        /// <summary>
        ///   Synchronization object to avoid behaviors since we are in a multi threaded environment.
        /// </summary>
        protected ReaderWriterLockSlim Sync = new ReaderWriterLockSlim();

        protected Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        ///   Constructor
        /// </summary>
        protected WorldSpace()
        {
            Childrens = new List<WorldSpace>();
            Entities = new ConcurrentDictionary<long, Entity>();
        }

        /// <summary>
        ///   Type of this World space.
        /// </summary>
        public abstract WorldSpaceType Type
        {
            get;
        }

        #region Events

        public virtual void OnMonsterSpawning()
        {
            foreach (WorldSpace child in Childrens)
            {
                child.OnMonsterSpawning();
            }
        }

        public virtual void OnEnter(Entity entity)
        {
            if (Entities.ContainsKey(entity.Id))
            {
                // WorldSpace change...
                Entity dummyent;
                Entities.TryRemove(entity.Id, out dummyent);
            }

            if (!Entities.TryAdd(entity.Id, entity))
            {
                throw new Exception("Couldn't add entity in world space");
            }
        }

        public virtual void OnLeave(Entity entity)
        {
            Entity tmp;
            if (!Entities.TryRemove(entity.Id, out tmp))
            {
                throw new Exception("Couldn't remove entity in world space");
            }
        }

        #endregion

        #region Entity Management

        /// <summary>
        ///   Find and returns all entities in this world space.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEntity> FindAll()
        {
            return Entities.Values;
        }

        /// <summary>
        ///   Get an entity with the given id.
        /// </summary>
        /// <param name = "id"></param>
        /// <returns></returns>
        public IEntity Get(long id)
        {
            return Entities[id];
        }

        public List<Character> GetAllCharacters()
        {
            return Entities.Values.OfType<Character>().ToList();
        }

        /// <summary>
        ///   Execute an action of every characters in this world space.
        /// </summary>
        /// <param name = "action"></param>
        public void CallOnAllCharacters(Action<Character> action)
        {
            List<Character> chars = GetAllCharacters();

            Parallel.For(0, chars.Count, i => action(chars[i]));
        }

        #endregion

        #region Properties

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<WorldSpace> Childrens
        {
            get;
            set;
        }

        public WorldSpace ParentSpace
        {
            get;
            set;
        }

        public ConcurrentDictionary<long, Entity> Entities
        {
            get;
            set;
        }

        #endregion
    }
}
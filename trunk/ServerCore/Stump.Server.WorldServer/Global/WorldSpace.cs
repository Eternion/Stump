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
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global
{
    public abstract partial class WorldSpace : IEntityContainer, IContext
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
            Characters = new ConcurrentDictionary<long, Character>();
            Npcs = new ConcurrentDictionary<long, NpcSpawn>();

            EntityAdded += OnEntityAdded;
            EntityRemoved += OnEntityRemoved;
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
            // todo : use reals events ...
            foreach (WorldSpace child in Childrens)
            {
                child.OnMonsterSpawning();
            }
        }

        public virtual void AddEntity(Entity entity)
        {
            if (Entities.ContainsKey(entity.Id))
            {
                // WorldSpace change...
                Entity removedEntity;
                Entities.TryRemove(entity.Id, out removedEntity);

                if (EntityRemoved != null)
                    EntityRemoved(removedEntity);
            }

            if (!Entities.TryAdd(entity.Id, entity))
            {
                throw new Exception("Couldn't add entity in world space");
            }

            if (EntityAdded != null)
                EntityAdded(entity);
        }

        public virtual void RemoveEntity(Entity entity)
        {
            Entity removedEntity;
            if (!Entities.TryRemove(entity.Id, out removedEntity))
            {
                throw new Exception("Couldn't remove entity in world space");
            }

            if (EntityRemoved != null)
                EntityRemoved(removedEntity);
        }

        protected virtual void OnEntityAdded(Entity entity)
        {
            if (entity is Character)
                if (!Characters.TryAdd(entity.Id, entity as Character))
                {
                    throw new Exception("Couldn't add character in world space");
                }

            if (entity is NpcSpawn)
                if (!Npcs.TryAdd(entity.Id, entity as NpcSpawn))
                {
                    throw new Exception("Couldn't add npc in world space");
                }
        }

        protected virtual void OnEntityRemoved(Entity entity)
        {
            if (entity is Character)
            {
                Character removedCharacter;
                if (!Characters.TryRemove(entity.Id, out removedCharacter))
                {
                    throw new Exception("Couldn't remove character in world space");
                }
            }
            if (entity is Character)
            {
                NpcSpawn removedNpc;
                if (!Npcs.TryRemove(entity.Id, out removedNpc))
                {
                    throw new Exception("Couldn't remove npc in world space");
                }
            }
        }

        #endregion

        #region Entity Management

        #region IContext Members

        public virtual IEnumerable<Character> GetAllCharacters()
        {
            return Characters.Values;
        }

        /// <summary>
        ///   Execute an action of every characters in this world space.
        /// </summary>
        /// <param name = "action"></param>
        public virtual void CallOnAllCharacters(Action<Character> action)
        {
            Parallel.ForEach(Characters.Values, action);
        }

        #endregion

        #region IEntityContainer Members

        /// <summary>
        ///   Find and returns all entities in this world space.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity> FindAll()
        {
            return Entities.Values;
        }

        /// <summary>
        ///   Get an entity with the given id.
        /// </summary>
        /// <param name = "id"></param>
        /// <returns></returns>
        public Entity Get(long id)
        {
            return !Entities.ContainsKey(id) ? null : Entities[id];
        }

        #endregion

        /// <summary>
        ///   Get an entity with the given id.
        /// </summary>
        /// <param name = "id"></param>
        /// <returns></returns>
        public T Get<T>(long id) where T : Entity
        {
            Entity entity;
            if ((entity = Get(id)) != null && entity is T)
                return entity as T;

            return default(T);
        }

        #endregion

        #region Properties

        public int Id
        {
            get;
            internal set;
        }

        public string Name
        {
            get;
            internal set;
        }

        public List<WorldSpace> Childrens
        {
            get;
            internal set;
        }

        public virtual ContextType ContextType
        {
            get { return ContextType.Unknown; }
        }

        public WorldSpace ParentSpace
        {
            get;
            internal set;
        }

        public ConcurrentDictionary<long, Character> Characters
        {
            get;
            private set;
        }

        public ConcurrentDictionary<long, NpcSpawn> Npcs
        {
            get;
            private set;
        }

        public ConcurrentDictionary<long, Entity> Entities
        {
            get;
            private set;
        }

        #endregion
    }
}
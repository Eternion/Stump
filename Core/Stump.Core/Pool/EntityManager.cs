using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stump.Core.Reflection;

namespace Stump.Core.Pool
{
    /// <summary>
    /// Provide a thread safe entity manager
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public class EntityManager<TSingleton, T> : EntityManager<TSingleton, T, int>
        where T : class
        where TSingleton : class
    {

    }

    /// <summary>
    /// Provide a thread safe entity manager
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <typeparam name="TC">Type of identifier</typeparam>
    public class EntityManager<TSingleton, T, TC> : Singleton<TSingleton>
        where TSingleton : class
        where T : class
    {
        private readonly ConcurrentDictionary<TC, T> m_entities = new ConcurrentDictionary<TC, T>();

        protected IReadOnlyDictionary<TC, T> Entities => new ReadOnlyDictionary<TC, T>(m_entities); 

        public event Action<T> EntityAdded;

        protected virtual void OnEntityAdded(T obj)
        {
            Action<T> handler = EntityAdded;
            if (handler != null) handler(obj);
        }

        public event Action<T> EntityRemoved;

        protected virtual void OnEntityRemoved(T obj)
        {
            Action<T> handler = EntityRemoved;
            if (handler != null) handler(obj);
        }

        protected void AddEntity(TC identifier, T entity)
        {
            if (!m_entities.TryAdd(identifier, entity))
                throw new Exception(string.Format("Cannot add entity, identifier {0} may already exist", identifier));

            OnEntityAdded(entity);
        }

        protected T RemoveEntity(TC identifier)
        {
            T entity;
            if (!m_entities.TryRemove(identifier, out entity))
                throw new Exception(string.Format("Cannot remove entity, identifier {0} may not exist", identifier));

            OnEntityRemoved(entity);

            return entity;
        }

        protected T GetEntityOrDefault(TC identifier)
        {
            T entity;
            return !m_entities.TryGetValue(identifier, out entity) ? default(T) : entity;
        }

        protected T GetEntity(TC identifier)
        {
            T entity;
            if (!m_entities.TryGetValue(identifier, out entity))
                throw new KeyNotFoundException(string.Format("Entity with identifier {0} not found", identifier));

            return entity;
        }
    }
}
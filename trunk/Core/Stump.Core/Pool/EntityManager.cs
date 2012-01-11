using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        protected void AddEntity(TC identifier, T entity)
        {
            if (!m_entities.TryAdd(identifier, entity))
                throw new Exception(string.Format("Cannot add entity, identifier {0} may already exist", identifier));
        }

        protected T RemoveEntity(TC identifier)
        {
            T entity;
            if (!m_entities.TryRemove(identifier, out entity))
                throw new Exception(string.Format("Cannot remove entity, identifier {0} may not exist", identifier));

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
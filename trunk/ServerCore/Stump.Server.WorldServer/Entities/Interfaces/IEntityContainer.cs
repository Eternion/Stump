
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Stump.Server.WorldServer.Entities
{
    public interface IEntityContainer
    {
        /// <summary>
        /// Thread Safe set containing characters.
        /// </summary>
        ConcurrentDictionary<long, Entity> Entities { get; }
        /// <summary>
        /// Find all entities contained in this set. 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Entity> FindAll();
        /// <summary>
        /// Get an entity with the given id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Entity Get(long id);

    }
}

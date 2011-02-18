using System;
using System.Collections;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using Stump.BaseCore.Framework.Utils;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.World.Actors.Character;

namespace Stump.Server.WorldServer.World
{
    public partial class World : Singleton<World>
    {

        /* Initialized to optimized value */
        private readonly ConcurrentDictionary<long, Character> m_charactersById = new ConcurrentDictionary<long, Character>(WorkerManager.WorkerThreadNumber, MessageListener.MaxConcurrentConnections);
        private readonly ConcurrentDictionary<string, Character> m_charactersByName = new ConcurrentDictionary<string, Character>(WorkerManager.WorkerThreadNumber, MessageListener.MaxConcurrentConnections);

        private int m_characterCount;
        public int CharacterCount
        { get { return m_characterCount; } }


        public void Enter(Character character)
        {
            if (m_charactersById.TryAdd(character.Id, character) && m_charactersByName.TryAdd(character.Name, character))
                Interlocked.Increment(ref m_characterCount);
            else
                logger.Warn(string.Format("Impossible d'ajouter le personnage {0}:{{1}} au World", character.Id, character.Name));
        }

        public void Leave(Character character)
        {
            Character dummy;
            if (m_charactersById.TryRemove(character.Id, out dummy) && m_charactersByName.TryRemove(character.Name, out dummy))
                Interlocked.Decrement(ref m_characterCount);
            else
                logger.Warn(string.Format("Impossible de supprimer le personnage {0}:{{1}} du World", character.Id, character.Name));
        }

        public bool IsConnected(long id)
        {
            return m_charactersById.ContainsKey(id);
        }

        public bool IsConnected(string name)
        {
            return m_charactersByName.ContainsKey(name);
        }

        public Character GetCharacter(long id)
        {
            Character character;
            return m_charactersById.TryGetValue(id, out character) ? character : null;
        }

        public Character GetCharacter(string name)
        {
            Character character;
            return m_charactersByName.TryGetValue(name, out character) ? character : null;
        }

        public Character GetCharacter(Predicate<Character> predicate)
        {
            return m_charactersById.FirstOrDefault(k => predicate(k.Value)).Value;
        }

        public IEnumerable GetCharacters(Predicate<Character> predicate)
        {
            return m_charactersById.Where(k => predicate(k.Value));
        }

        public void CallOnCharacters(Action<Character> action)
        {
            foreach (var key in m_charactersById)
                action(key.Value);
        }

        public void CallOnCharacters(Predicate<Character> predicate, Action<Character> action)
        {
            foreach (var key in m_charactersById.Where(k => predicate(k.Value)))
                action(key.Value);
        }

    }
}
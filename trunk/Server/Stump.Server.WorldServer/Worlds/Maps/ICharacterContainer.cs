using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Maps
{
    public interface ICharacterContainer
    {
        IEnumerable<Character> GetAllCharacters();
        void ForEach(Action<Character> action);

        WorldClientCollection Clients
        {
            get;
        }
    }
}
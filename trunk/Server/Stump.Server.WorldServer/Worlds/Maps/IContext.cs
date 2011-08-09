using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Maps
{
    public interface IContext
    {
        IEnumerable<Character> GetAllCharacters();
        void Do(Action<Character> action);
    }
}
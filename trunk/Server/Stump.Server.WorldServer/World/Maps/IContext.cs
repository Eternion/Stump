using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.World.Actors.RolePlay;
using Stump.Server.WorldServer.World.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.World.Maps
{
    public interface IContext
    {
        IEnumerable<Character> GetAllCharacters();
        void Do(Action<Character> action);
    }
}
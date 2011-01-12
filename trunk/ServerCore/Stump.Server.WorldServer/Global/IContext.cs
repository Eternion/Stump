using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global
{
    public enum ContextType
    {
        Map,
        Fight,
        Unknown,
    }

    public interface IContext
    {
        ContextType ContextType
        {
            get;
        }

        IEnumerable<Character> GetAllCharacters();
        void CallOnAllCharacters(Action<Character> action);
    }
}

using System;
using System.Collections.Generic;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        private CharacterHandler()
        {
            Predicates = new Dictionary<Type, Predicate<WorldClient>>
                         {
                         };
        }
    }
}
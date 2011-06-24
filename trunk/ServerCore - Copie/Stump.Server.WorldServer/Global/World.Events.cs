
using System;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global
{
    public partial class World
    {
        public event Character.CharacterLoginHandler LoggedIn;

        internal void NotifyLoggedIn(Character character)
        {
            Character.CharacterLoginHandler handler = LoggedIn;

            if (handler != null)
                handler(character);
        }

        public event Character.CharacterLoginHandler LoggingOut;

        internal void NotifyLoggingOut(Character character)
        {
            Character.CharacterLoginHandler handler = LoggingOut;

            if (handler != null)
                handler(character);
        }
    }
}
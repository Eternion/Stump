using System;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities
{
    public partial class Character
    {
        public delegate void CharacterLoginHandler(Character chr);

        public event CharacterLoginHandler LoggedIn;

        private void NotifyLoggedIn()
        {
            CharacterLoginHandler handler = LoggedIn;

            if (handler != null)
                handler(this);

            World.Instance.NotifyLoggedIn(this);
        }

        public event CharacterLoginHandler LoggingOut;

        private void NotifyLoggingOut()
        {
            CharacterLoginHandler handler = LoggingOut;

            if (handler != null)
                handler(this);

            World.Instance.NotifyLoggingOut(this);
        }
    }
}
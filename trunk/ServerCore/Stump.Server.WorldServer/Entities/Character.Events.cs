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

        /// <summary>
        /// Occurs when the character loose a fight and loose energy.
        /// </summary>
        public event Action<Character, uint> Dead;

        private void NotifyDead(uint energyLost)
        {
            Action<Character, uint> handler = Dead;
            if (handler != null)
                handler(this, energyLost);
        }

        /// <summary>
        /// Occurs when the character loose all his energy and became a ghost
        /// </summary>
        public event Action<Character> BecameGhost;

        private void NotifyBecameGhost()
        {
            Action<Character> handler = BecameGhost;
            if (handler != null)
                handler(this);
        }

        public event Action<Character, long> ExperienceGained;

        private void NotifyExperienceGained(long gainedPoints)
        {
            Action<Character, long> handler = ExperienceGained;
            if (handler != null)
                handler(this, gainedPoints);
        }

        public event Action<Character> ExperienceModified;

        public void NotifyExperienceModified()
        {
            Action<Character> handler = ExperienceModified;
            if (handler != null)
                handler(this);
        }

        public event Action<Character, uint> LeveledUp;

        public void NotifyLeveledUp(uint gainedLevel)
        {
            Action<Character, uint> handler = LeveledUp;
            if (handler != null)
                handler(this, gainedLevel);
        }

        public event Action<Character, uint> LeveledDown;

        public void NotifyLeveledDown(uint loosedLevel)
        {
            Action<Character, uint> handler = LeveledDown;
            if (handler != null)
                handler(this, loosedLevel);
        }
    }
}
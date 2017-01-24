using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace GameplayPlugin.AnnouncePopup
{
    public static class AnnouncePopup
    {
        private static bool m_active = false;

        [Variable(true, DefinableRunning = true)]
        public static bool Active
        {
            get { return m_active; }
            set
            {
                if (value && !m_active && Plugin.CurrentPlugin.Initialized)
                    Initialize();
                else if (!value && m_active && Plugin.CurrentPlugin.Initialized)
                    TearDown();

                m_active = value;
            }
        }

        [Variable(true, DefinableRunning = true)]
        public static string AnnounceTitle = "Welcome !";

        [Variable(true, DefinableRunning = true)]
        public static string AnnounceContent = "Welcome on this server.";

        [Initialization(typeof(World), Silent = true)]
        public static void Initialize()
        {
            if (Active)
                World.Instance.CharacterJoined += OnCharacterJoined;
        }

        public static void TearDown()
        {
            World.Instance.CharacterJoined -= OnCharacterJoined;
        }

        private static void OnCharacterJoined(Character character)
        {
            character.OpenPopup(AnnounceContent, AnnounceTitle, 0);
        }
    }
}
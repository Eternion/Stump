using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Linq;

namespace GameplayPlugin.AnnouncePopup
{
    public class LevelPopup
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
        public static string PopupMessage = "";

        [Variable(true, DefinableRunning = true)]
        public static int[] PopupLevels = new int[] { 50, 100, 150, 200 };

        [Initialization(typeof(World), Silent = true)]
        public static void Initialize()
        {
            if (!Active)
                return;

            World.Instance.CharacterJoined += OnCharacterJoined;
            World.Instance.CharacterLeft += OnCharacterLeft;
        }

        public static void TearDown()
        {
            World.Instance.CharacterJoined -= OnCharacterJoined;
            World.Instance.CharacterLeft -= OnCharacterLeft;
        }

        private static void OnCharacterJoined(Character character)
        {
            character.LevelChanged += OnLevelChanged;
        }

        private static void OnLevelChanged(Character character, byte currentLevel, int difference)
        {
            if (!PopupLevels.Contains(currentLevel))
                return;

            character.DisplayNotification(PopupMessage, NotificationEnum.INFORMATION);
        }

        private static void OnCharacterLeft(Character character)
        {
            character.LevelChanged -= OnLevelChanged;
        }
    }
}

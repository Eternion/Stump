using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps;
using System.Linq;

namespace GameplayPlugin.AnnouncePopup
{
    public class MapPopup
    {
        private static bool m_active = true;

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
        public static int[] POPUP_MAPS = {
            162267138
        };

        [Variable(true, DefinableRunning = true)]
        public static string POPUP_MESSAGE = "";

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
            character.EnterMap += OnEnterMap;
        }

        private static void OnCharacterLeft(Character character)
        {
            character.EnterMap -= OnEnterMap;
        }

        private static void OnEnterMap(RolePlayActor actor, Map map)
        {
            if (!(actor is Character character))
                return;

            if (!POPUP_MAPS.Contains(map.Id))
                return;

            character.DisplayNotification(POPUP_MESSAGE, NotificationEnum.INFORMATION);
        }
    }
}

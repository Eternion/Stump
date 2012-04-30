using System.Drawing;
using NLog;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace ArkalysPlugin.Commands
{
    public static class IsleTrigger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static bool m_active;

        [Initialization(typeof(World))]
        public static void Initialize()
        {
            logger.Debug("Isle trigger actived");
            m_active = true;

            World.Instance.CharacterJoined += OnCharacterJoined;
            World.Instance.CharacterLeft += OnCharacterLeft;
        }

        public static void TearDown()
        {
            if (World.Instance != null)
            {
                World.Instance.CharacterJoined -= OnCharacterJoined;
                World.Instance.CharacterLeft -= OnCharacterLeft;
            }
        }

        private static void OnCharacterLeft(Character character)
        {
            character.LevelChanged += OnLevelChanged;
        }

        private static void OnCharacterJoined(Character character)
        {
            character.LevelChanged -= OnLevelChanged;
        }

        private static void OnLevelChanged(Character character, byte currentlevel, int difference)
        {
            if (!m_active)
            {
                character.LevelChanged -= OnLevelChanged;
                return;
            }

            foreach (var isle in IsleCommand.ProfilsIsle)
            {
                if (currentlevel >= isle.Level && currentlevel - difference < isle.Level)
                {
                    character.SendServerMessage("Vous pouvez désormais accéder à l'île {0} en tapant dans le chat '.ile {0}'",
                        Color.RoyalBlue);
                }
            }
        }

    }
}
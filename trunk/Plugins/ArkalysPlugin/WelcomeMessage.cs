using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace ArkalysPlugin
{
    public static class WelcomeMessage
    {
        [Variable(true)]
        public static string WelcomeMsg = "Welcome !";

        [Variable(true)]
        public static string MessageAuthor = "L'équipe Arkalys";

        [Initialization(typeof(World), Silent=true)]
        public static void Initialize()
        {
            World.Instance.CharacterJoined += OnCharacterJoined;
        }

        private static void OnCharacterJoined(Character character)
        {
            if (character.Account.FirstConnection)
                ShowWelcomeMessage(character);
        }

        public static void ShowWelcomeMessage(Character character)
        {
            character.OpenPopup(WelcomeMsg, MessageAuthor, 0);
        }
    }
}
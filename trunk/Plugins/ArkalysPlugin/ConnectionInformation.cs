using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace ArkalysPlugin
{
    public static class ConnectionInformation
    {
        [Variable(true)]
        public static string WelcomeMsg = "Bienvenue sur Arkalys !\n\nLe serveur vous offre ...\n\n'.shop' => Vous téléporte à ...\n\n.ile # => ...";

        [Initialization(typeof(World))]
        public static void Initialize()
        {
            World.Instance.CharacterJoined += OnCharacterJoined;
        }

        private static void OnCharacterJoined(Character character)
        {
            if (character.Account.FirstConnection)
                character.OpenPopup(WelcomeMsg, "L'équipe Arkalys", 0);
        }
    }
}
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Breeds;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler
    {

        [WorldHandler(CharacterReplayRequestMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterReplayRequestMessage(WorldClient client, CharacterReplayRequestMessage message)
        {
            // TODO mhh ?
        }
    }
}
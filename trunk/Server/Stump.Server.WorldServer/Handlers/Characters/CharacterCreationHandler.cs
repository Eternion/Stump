using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [Variable]
        public static bool EnableNameSuggestion = true;

        [WorldHandler(CharacterCreationRequestMessage.Id, RequiresLogin = false, IsGamePacket = false)]
        public static void HandleCharacterCreationRequestMessage(WorldClient client, CharacterCreationRequestMessage message)
        {
            if (!IpcAccessor.Instance.IsConnected)
            {
                SendCharacterCreationResultMessage(client, CharacterCreationResultEnum.ERR_NO_REASON);
                return;
            }

            var result = CharacterManager.Instance.CreateCharacter(client, message.name, message.breed, message.sex, message.colors);
           
            SendCharacterCreationResultMessage(client, result);
            
            if (result == CharacterCreationResultEnum.OK)
            {
                BasicHandler.SendBasicNoOperationMessage(client);
                SendCharactersListMessage(client);
            }
        }

        [WorldHandler(CharacterNameSuggestionRequestMessage.Id, RequiresLogin = false, IsGamePacket = false)]
        public static void HandleCharacterNameSuggestionRequestMessage(WorldClient client, CharacterNameSuggestionRequestMessage message)
        {
            if (!EnableNameSuggestion)
            {
                client.Send(new CharacterNameSuggestionFailureMessage((sbyte)NicknameGeneratingFailureEnum.NICKNAME_GENERATOR_UNAVAILABLE));
                return;
            }

            string generatedName = CharacterManager.Instance.GenerateName();

            client.Send(new CharacterNameSuggestionSuccessMessage(generatedName));
        }

        public static void SendCharacterCreationResultMessage(IPacketReceiver client, CharacterCreationResultEnum result)
        {
            client.Send(new CharacterCreationResultMessage((sbyte) result));
        }
    }
}
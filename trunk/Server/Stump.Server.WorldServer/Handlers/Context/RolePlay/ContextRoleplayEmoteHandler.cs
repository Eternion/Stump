using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        [WorldHandler(EmotePlayRequestMessage.Id)]
        public static void HandleEmotePlayRequestMessage(WorldClient client, EmotePlayRequestMessage message)
        {
            // todo : found the duration of each emote
            //client.ActiveCharacter.((EmotesEnum) message.emoteId, 0);
            SendEmotePlayMessage(client.ActiveCharacter.Map.Clients, client.ActiveCharacter, (EmotesEnum)message.emoteId, 0);
        }

        public static void SendEmotePlayMessage(IPacketReceiver client, Character character, EmotesEnum emote, byte duration)
        {
            client.Send(new EmotePlayMessage(
                            (sbyte) emote,
                            duration,
                            character.Id,
                            (int) character.Client.Account.Id
                            ));
        }

        public static void SendEmotePlayMessage(IPacketReceiver client, ContextActor actor, EmotesEnum emote, byte duration)
        {
            client.Send(new EmotePlayMessage(
                            (sbyte) emote,
                            duration,
                            actor.Id,
                            0
                            ));
        }

        public static void SendEmoteListMessage(IPacketReceiver client, IEnumerable<sbyte> emoteList)
        {
            client.Send(new EmoteListMessage(emoteList));
        }
    }
}
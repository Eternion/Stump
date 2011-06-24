
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {
        [WorldHandler(typeof (EmotePlayRequestMessage))]
        public static void HandleEmotePlayRequestMessage(WorldClient client, EmotePlayRequestMessage message)
        {
            // todo : found the duration of each emote
            client.ActiveCharacter.StartEmote((EmotesEnum) message.emoteId, 0);
        }

        public static void SendEmotePlayMessage(WorldClient client, Character character, EmotesEnum emote, uint duration)
        {
            client.Send(new EmotePlayMessage(
                            (byte) emote,
                            duration,
                            (int) character.Id,
                            (int) character.Client.Account.Id
                            ));
        }

        public static void SendEmotePlayMessage(WorldClient client, Entity entity, EmotesEnum emote, uint duration)
        {
            client.Send(new EmotePlayMessage(
                            (byte) emote,
                            duration,
                            (int) entity.Id,
                            0
                            ));
        }

        public static void SendEmoteListMessage(WorldClient client, IEnumerable<uint> emoteList)
        {
            client.Send(new EmoteListMessage(emoteList.ToList()));
        }
    }
}
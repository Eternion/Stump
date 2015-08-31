using System;
using System.Collections.Generic;
using Stump.Core.Extensions;
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
            if (!client.Character.AvailableEmotes.Contains((EmotesEnum) message.emoteId))
                return;

            client.Character.PlayEmote((EmotesEnum) message.emoteId);
        }

        public static void SendEmotePlayMessage(IPacketReceiver client, Character character, EmotesEnum emote)
        {
            client.Send(new EmotePlayMessage(
                            (byte) emote,
                            DateTime.Now.GetUnixTimeStampLong(),
                            character.Id,
                            character.Account.Id
                            ));
        }

        public static void SendEmotePlayMessage(IPacketReceiver client, ContextActor actor, EmotesEnum emote)
        {
            client.Send(new EmotePlayMessage(
                            (byte) emote,
                            DateTime.Now.GetUnixTimeStampLong(),
                            actor.Id,
                            0
                            ));
        }

        public static void SendEmoteListMessage(IPacketReceiver client, IEnumerable<byte> emoteList)
        {
            client.Send(new EmoteListMessage(emoteList));
        }
    }
}
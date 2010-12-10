using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Chat;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public class ChatHandler : WorldHandlerContainer
    {
        public static void SendChatServerMessage(WorldClient client, Character sender, ChannelId channel, string msg)
        {
            if (sender.Client.Account.Role <= RoleEnum.Moderator)
                msg = StringUtils.HtmlEntities(msg);

            client.Send(new ChatServerMessage(
                (uint)channel,
                msg,
                0, // timestamp
                "", // fingerprint
                (int)sender.Id,
                sender.Name,
                (int)sender.Client.Account.Id));
        }
    }
}
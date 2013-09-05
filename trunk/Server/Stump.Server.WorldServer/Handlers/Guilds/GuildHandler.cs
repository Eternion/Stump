using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Core.Extensions;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Game.Guilds;

namespace Stump.Server.WorldServer.Handlers.Guilds
{
    public class GuildHandler : WorldHandlerContainer
    {
        enum GuildInfoTypes
        {
            INFO_MEMBERS = 1,
            INFO_PERCEPTOR = 2,
            INFO_HOUSES = 3
        }

        [WorldHandler(GuildGetInformationsMessage.Id)]
        public static void HandleGuildGetInformationsMessage(WorldClient client, GuildGetInformationsMessage message)
        {
            var infoType = message.infoType;

            var guildId = client.Character.GuildId;
            if (guildId == 0) return;

            var guildInfo = GuildManager.Instance.GetGuildMembers(guildId);

            client.Send(new GuildInformationsMembersMessage(guildInfo));
        }

        public static void SendGuildMembershipMessage(WorldClient client)
        {
            var guildId = client.Character.GuildId;
            if (guildId == 0) return;

            var guildInfo = GuildManager.Instance.FindById(guildId);

            client.Send(new GuildMembershipMessage(new GuildInformations(guildId, guildInfo.Name, guildInfo.GuildEmblem), 262148, true));
        }

        public static void SendGuildInformationsGeneralMessage(WorldClient client)
        {
            var guildId = client.Character.GuildId;
            if (guildId == 0) return;

            var guildInfo = GuildManager.Instance.FindById(guildId);

            client.Send(new GuildInformationsGeneralMessage(true, false, (byte)guildInfo.Level, 10, guildInfo.Experience, 100, guildInfo.CreationDate.GetUnixTimeStamp()));
        }
    }
}

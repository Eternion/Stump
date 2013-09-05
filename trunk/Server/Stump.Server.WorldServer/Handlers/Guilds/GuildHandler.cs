using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Game.Guilds;

namespace Stump.Server.WorldServer.Handlers.Guilds
{
    public class GuildHandler : WorldHandlerContainer
    {
        public static void SendGuildMembershipMessage(WorldClient client)
        {
            var guildId = client.Character.GuildId;
            if (guildId == 0) return;

            var guildInfo = GuildManager.Instance.FindById(guildId);

            client.Send(new GuildMembershipMessage(new GuildInformations(guildId, guildInfo.Name, guildInfo.GuildEmblem), 262148, true));
        }
    }
}

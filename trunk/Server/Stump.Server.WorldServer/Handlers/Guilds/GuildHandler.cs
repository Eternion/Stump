using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Core.Extensions;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Guilds;
using GuildMember = Stump.Server.WorldServer.Game.Guilds.GuildMember;

namespace Stump.Server.WorldServer.Handlers.Guilds
{
    public class GuildHandler : WorldHandlerContainer
    {
        [WorldHandler(GuildGetInformationsMessage.Id)]
        public static void HandleGuildGetInformationsMessage(WorldClient client, GuildGetInformationsMessage message)
        {
            if (client.Character.Guild == null)
                return;

            switch (message.infoType)
            {
                case (sbyte)GuildInformationsTypeEnum.INFO_GENERAL:
                    SendGuildInformationsGeneralMessage(client, client.Character.Guild);
                    break;
                case (sbyte)GuildInformationsTypeEnum.INFO_MEMBERS:
                    SendGuildInformationsMembersMessage(client, client.Character.Guild);
                    break;
            }
        }

        [WorldHandler(GuildCreationValidMessage.Id)]
        public static void HandleGuildCreationValidMessage(WorldClient client, GuildCreationValidMessage message)
        {
            if (client.Character.Guild == null)
            {
                var result = GuildManager.Instance.CreateGuild(client.Character, message.guildName, message.guildEmblem);
                SendGuildCreationResultMessage(client, result);

                if (result == GuildCreationResultEnum.GUILD_CREATE_OK)
                {
                    GuildManager.Instance.SetBoss(client.Character.GuildMember);
                    SendGuildJoinedMessage(client, client.Character.GuildMember);
                }
            }
            else
            {
                SendGuildCreationResultMessage(client, GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD);
            }
        }

        [WorldHandler(GuildChangeMemberParametersMessage.Id)]
        public static void HandleGuildChangeMemberParametersMessage(WorldClient client, GuildChangeMemberParametersMessage message)
        {
            if (client.Character.Guild == null)
                return;

            //todo: Write a method to get offline GuildMember
            var target = World.Instance.GetCharacter(message.memberId);
            if (target == null)
                return;

            if (!client.Character.Guild.ChangeParameters(client.Character, target, message.rank, message.experienceGivenPercent, message.rights))
                return;

            SendGuildInformationsMemberUpdateMessage(client, target.GuildMember);
            SendGuildMembershipMessage(target.Client, target.GuildMember);
        }

        [WorldHandler(GuildKickRequestMessage.Id)]
        public static void HandleGuildKickRequestMessage(WorldClient client, GuildKickRequestMessage message)
        {
            if (client.Character.Guild == null)
                return;

            //todo: Write a method to get offline GuildMember
            var target = World.Instance.GetCharacter(message.kickedId);
            if (target == null)
                return;

            if (!target.Guild.KickMember(client.Character, target))
                return;

            SendGuildLeftMessage(target.Client);
        }

        public static void SendGuildLeftMessage(IPacketReceiver client)
        {
            client.Send(new GuildLeftMessage());
        }

        public static void SendGuildCreationResultMessage(IPacketReceiver client, GuildCreationResultEnum result)
        {
            client.Send(new GuildCreationResultMessage((sbyte)result));
        }

        public static void SendGuildMembershipMessage(IPacketReceiver client, GuildMember member)
        {
            client.Send(new GuildMembershipMessage(member.Guild.GetGuildInformations(), (uint)member.Rights, true));
        }

        public static void SendGuildInformationsGeneralMessage(IPacketReceiver client, Guild guild)
        {
            client.Send(new GuildInformationsGeneralMessage(true, false, guild.Level, guild.ExperienceLevelFloor, guild.Experience, guild.ExperienceNextLevelFloor, guild.CreationDate.GetUnixTimeStamp())); 
        }

        public static void SendGuildInformationsMembersMessage(IPacketReceiver client, Guild guild)
        {
            client.Send(new GuildInformationsMembersMessage(guild.Members.Select(x => x.GetNetworkGuildMember())));
        }

        public static void SendGuildInformationsMemberUpdateMessage(IPacketReceiver client, GuildMember member)
        {
            client.Send(new GuildInformationsMemberUpdateMessage(member.GetNetworkGuildMember()));
        }

        public static void SendGuildJoinedMessage(IPacketReceiver client, GuildMember member)
        {
            client.Send(new GuildJoinedMessage(member.Guild.GetGuildInformations(), (uint)member.Rights, true));
        }
    }
}

using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Core.Extensions;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
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
                    client.Character.GuildMember.SetBoss();
                    SendGuildJoinedMessage(client, client.Character.GuildMember);
                    SendGuildInformationsMembersMessage(client, client.Character.Guild);
                    SendGuildInformationsGeneralMessage(client, client.Character.Guild);
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

            var target = GuildManager.Instance.TryGetGuildMember(message.memberId);
            if (target == null || target.Guild != client.Character.Guild)
                return;

            if (!client.Character.Guild.ChangeParameters(client.Character, target, message.rank, message.experienceGivenPercent, message.rights))
                return;

            SendGuildInformationsMemberUpdateMessage(client, target);

            if (target.Character != null)
                SendGuildMembershipMessage(target.Character.Client, target);
        }

        [WorldHandler(GuildKickRequestMessage.Id)]
        public static void HandleGuildKickRequestMessage(WorldClient client, GuildKickRequestMessage message)
        {
            if (client.Character.Guild == null)
                return;

            var target = GuildManager.Instance.TryGetGuildMember(message.kickedId);
            if (target == null)
                return;

            if (!target.Guild.KickMember(client.Character, target))
                return;

            if (target.Character != null)
                SendGuildLeftMessage(target.Character.Client);
        }

        [WorldHandler(GuildInvitationMessage.Id)]
        public static void HandleGuildInvitationMessage(WorldClient client, GuildInvitationMessage message)
        {
            if (client.Character.Guild == null)
                return;

            if (client.Character.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_INVITE_NEW_MEMBERS))
            {
                var target = World.Instance.GetCharacter(message.targetId);
                if (target == null)
                {
                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 208);
                    return;
                }


                if (target.Guild != null)
                {
                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 206);
                    return;
                }

                SendGuildInvitationStateRecruterMessage(client, target, GuildInvitationStateEnum.GUILD_INVITATION_SENT);
                SendGuildInvitedMessage(target.Client, client.Character);
            }
            else
            {
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 207);
            }
        }

        [WorldHandler(GuildInvitationAnswerMessage.Id)]
        public static void HandleGuildInvitationAnswerMessage(WorldClient client, GuildInvitationAnswerMessage message)
        {
            if (client.Character.Guild != null)
                return;
            if (client.Character.GuildInvitation == 0)
                return;

            var recruter = World.Instance.GetCharacter(client.Character.GuildInvitation);
            if (recruter == null)
                return;

            SendGuildInvitationStateRecruterMessage(recruter.Client, client.Character,
                                                    message.accept
                                                        ? GuildInvitationStateEnum.GUILD_INVITATION_OK
                                                        : GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);

            if (!message.accept)
                return;

            client.Character.GuildMember.JoinGuild();

            SendGuildJoinedMessage(client, client.Character.GuildMember);
            SendGuildInformationsMembersMessage(client, client.Character.Guild);
            SendGuildInformationsGeneralMessage(client, client.Character.Guild);
        }

        public static void SendGuildInvitedMessage(IPacketReceiver client, Character recruter)
        {
            client.Send(new GuildInvitedMessage(recruter.Id, recruter.Name, recruter.Guild.GetGuildInformations()));
        }

        public static void SendGuildInvitationStateRecruterMessage(IPacketReceiver client, Character recruted, GuildInvitationStateEnum state)
        {
            client.Send(new GuildInvitationStateRecruterMessage(recruted.Name, (sbyte)state));
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

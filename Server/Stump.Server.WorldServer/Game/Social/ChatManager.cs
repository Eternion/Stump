using System;
using System.Collections.Generic;
using System.Text;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Chat;

namespace Stump.Server.WorldServer.Game.Social
{
    public class ChatManager : Singleton<ChatManager>
    {
        #region Delegates

        /// <summary>
        ///   Delegate for parsing incomming in game messages.
        /// </summary>
        public delegate void ChatParserDelegate(WorldClient client, string msg);

        #endregion

        /// <summary>
        /// Prefix used for chat commands
        /// </summary>
        [Variable]
        public static readonly string CommandPrefix = ".";

        /// <summary>
        /// Minimal role level to access the admin chat
        /// </summary>
        [Variable]
        public static readonly RoleEnum AdministratorChatMinAccess = RoleEnum.Moderator;

        /// <summary>
        /// In milliseconds
        /// </summary>
        [Variable]
        public static int AntiFloodTimeBetweenTwoMessages = 500;

        /// <summary>
        /// In seconds
        /// </summary>
        [Variable] 
        public static int AntiFloodTimeBetweenTwoGlobalMessages = 60;

        /// <summary>
        /// Amount of messages allowed in a given time
        /// </summary>
        [Variable]
        public static int AntiFloodAllowedMessages = 4;

        /// <summary>
        /// Time in seconds
        /// </summary>
        [Variable]
        public static int AntiFloodAllowedMessagesResetTime = 10;

        /// <summary>
        /// Time in seconds
        /// </summary>
        [Variable]
        public static int AntiFloodMuteTime = 10;

        /// <summary>
        ///   Chat handler for each channel Id.
        /// </summary>
        public readonly Dictionary<ChatActivableChannelsEnum, ChatParserDelegate> ChatHandlers = new Dictionary<ChatActivableChannelsEnum, ChatParserDelegate>();

        [Initialization(InitializationPass.First)]
        public void Initialize()
        {
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_GLOBAL, SayGlobal);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_GUILD, SayGuild);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_PARTY, SayParty);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_ARENA, SayArena);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_SALES, SaySales);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_SEEK, SaySeek);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_ADMIN, SayAdministrators);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_TEAM, SayTeam);
        }

        public bool CanUseChannel(Character character, ChatActivableChannelsEnum channel)
        {
            switch (channel)
            {
                case ChatActivableChannelsEnum.CHANNEL_GLOBAL:
                    return (!character.Map.IsMuted || character.UserGroup.Role >= AdministratorChatMinAccess);
                case ChatActivableChannelsEnum.CHANNEL_TEAM:
                    return character.IsFighting();
                case ChatActivableChannelsEnum.CHANNEL_ARENA:
                    return character.IsInParty(PartyTypeEnum.PARTY_TYPE_ARENA);
                case ChatActivableChannelsEnum.CHANNEL_GUILD:
                    return character.Guild != null;
                case ChatActivableChannelsEnum.CHANNEL_ALIGN:
                    return false;
                case ChatActivableChannelsEnum.CHANNEL_PARTY:
                    return character.IsInParty(PartyTypeEnum.PARTY_TYPE_CLASSICAL);
                case ChatActivableChannelsEnum.CHANNEL_SALES:
                    return !character.IsMuted();
                case ChatActivableChannelsEnum.CHANNEL_SEEK:
                    return !character.IsMuted();
                case ChatActivableChannelsEnum.CHANNEL_NOOB:
                    return true;
                case ChatActivableChannelsEnum.CHANNEL_ADMIN:
                    return character.UserGroup.Role >= AdministratorChatMinAccess;
                case ChatActivableChannelsEnum.CHANNEL_ADS:
                    return !character.IsMuted();
                case ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE:
                    return !character.IsMuted();
                case ChatActivableChannelsEnum.PSEUDO_CHANNEL_INFO:
                    return false;
                case ChatActivableChannelsEnum.PSEUDO_CHANNEL_FIGHT_LOG:
                    return false;
                default:
                    return false;
            }
        }

        #region Handlers

        public void HandleChat(WorldClient client, ChatActivableChannelsEnum channel, string message)
        {
            if (!CanUseChannel(client.Character, channel))
                return;

            if (!ChatHandlers.ContainsKey(channel))
                return;

            if (message.StartsWith(CommandPrefix) &&
                ( message.Length < CommandPrefix.Length * 2 || message.Substring(CommandPrefix.Length, CommandPrefix.Length) != CommandPrefix )) // ignore processing command whenever there is the preffix twice
            {
                message = message.Remove(0, CommandPrefix.Length); // remove our prefix
                WorldServer.Instance.CommandManager.HandleCommand(new TriggerChat(new StringStream(UnescapeChatCommand(message)),
                                                                                  client.Character));
            }
            else
            {
                if (client.Character.IsMuted())
                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 124,
                                                            (int) client.Character.GetMuteRemainingTime().TotalSeconds);
                else
                {
                    if (client.Character.ChatHistory.RegisterAndCheckFlood(new ChatEntry(message, channel, DateTime.Now)))
                        ChatHandlers[channel](client, message);
                }
            }
        }

        public void HandleChat(WorldClient client, ChatActivableChannelsEnum channel, string message, IEnumerable<ObjectItem> objectItems)
        {
            if (!CanUseChannel(client.Character, channel))
                return;

            if (!ChatHandlers.ContainsKey(channel))
                return;

            if (client.Character.IsMuted())
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 124,
                                                        (int)client.Character.GetMuteRemainingTime().TotalSeconds);
            else
            {
                if (!client.Character.ChatHistory.RegisterAndCheckFlood(new ChatEntry(message, channel, DateTime.Now)))
                    return;

                if (channel == ChatActivableChannelsEnum.CHANNEL_ARENA || channel == ChatActivableChannelsEnum.CHANNEL_GUILD
                    || channel == ChatActivableChannelsEnum.CHANNEL_PARTY || channel == ChatActivableChannelsEnum.CHANNEL_SALES
                    || channel == ChatActivableChannelsEnum.CHANNEL_TEAM || channel == ChatActivableChannelsEnum.CHANNEL_ADMIN)
                {
                    ChatHandler.SendChatServerWithObjectMessage(client, client.Character, channel, message, "", objectItems);
                }
                else
                {
                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 114);
                }
            }
        }

        private static string UnescapeChatCommand(string command)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < command.Length; i++)
            {
                if (command[i] == '&')
                {
                    var index = command.IndexOf(';', i, 5);
                    if (index == -1)
                        continue;

                    var str = command.Substring(i + 1, index - i - 1);

                    switch (str)
                    {
                        case "lt":
                            sb.Append("<");
                            break;
                        case "gt":
                            sb.Append(">");
                            break;
                        case "quot":
                            sb.Append("\"");
                            break;
                        case "amp":
                            sb.Append("&");
                            break;
                        default:
                            int id;
                            if (!int.TryParse(str, out id))
                                continue;
                            sb.Append((char) id);
                            break;
                    }

                    i = index + 1;
                }
                else
                    sb.Append(command[i]);
            }

            return sb.ToString();
        }

        private static void SendChatServerMessage(IPacketReceiver client, Character sender, ChatActivableChannelsEnum channel, string message)
        {
            if (sender.AdminMessagesEnabled)
                ChatHandler.SendChatAdminServerMessage(client, sender, channel, message);
            else
                ChatHandler.SendChatServerMessage(client, sender, channel, message);
        }

        private static void SendChatServerMessage(IPacketReceiver client, INamedActor sender, ChatActivableChannelsEnum channel, string message)
        {
            ChatHandler.SendChatServerMessage(client, sender, channel, message);
        }

        public void SayGlobal(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_GLOBAL))
                return;

            if (client.Character.IsFighting())
                SendChatServerMessage(client.Character.Fight.Clients, client.Character, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
            else if (client.Character.IsSpectator())
                SendChatServerMessage(client.Character.Fight.SpectatorClients, client.Character, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
            else
                SendChatServerMessage(client.Character.Map.Clients, client.Character, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }

        public void SayGlobal(NamedActor actor, string msg)
        {
            SendChatServerMessage(actor.CharacterContainer.Clients, actor, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }

        public void SayAdministrators(WorldClient client, string msg)
        {
            if (client.UserGroup.Role < AdministratorChatMinAccess)
                return;

            World.Instance.ForEachCharacter(entry =>
            {
                if (CanUseChannel(entry, ChatActivableChannelsEnum.CHANNEL_ADMIN))
                    ChatHandler.SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_ADMIN, msg);
            });
        }

        public void SayParty(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_PARTY))
            {
                ChatHandler.SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_NO_PARTY);
                return;
            }

            client.Character.Party.ForEach(entry => SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_PARTY, msg));
        }
        public void SayArena(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_ARENA))
            {
                ChatHandler.SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_NO_PARTY_ARENA);
                return;
            }

            client.Character.ArenaParty.ForEach(entry => SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_ARENA, msg));
        }

        public void SayGuild(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_GUILD))
            {
                ChatHandler.SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_NO_GUILD);
                return;
            }

            client.Character.Guild.Clients.ForEach(entry => SendChatServerMessage(entry, client.Character, ChatActivableChannelsEnum.CHANNEL_GUILD, msg));
        }

        public void SayTeam(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_TEAM))
            {
                ChatHandler.SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_NO_TEAM);
                return;
            }

            foreach (var fighter in client.Character.Fighter.Team.GetAllFighters<CharacterFighter>())
            {
                SendChatServerMessage(fighter.Character.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_TEAM, msg);
            }
        }

        public void SaySeek(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_SEEK))
                return;

            World.Instance.ForEachCharacter(entry =>
                SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_SEEK, msg));
        }

        public void SaySales(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_SALES))
                return;

            World.Instance.ForEachCharacter(entry =>
                SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_SALES, msg));
        }

        public static bool IsGlobalChannel(ChatActivableChannelsEnum channel)
        {
            return channel == ChatActivableChannelsEnum.CHANNEL_SALES ||
                   channel == ChatActivableChannelsEnum.CHANNEL_SEEK;
        }

        #endregion
    }
}
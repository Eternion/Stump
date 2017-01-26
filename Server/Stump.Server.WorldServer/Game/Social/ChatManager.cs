using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Stump.Core.Attributes;
using Stump.Core.IO;
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
using MongoDB.Bson;
using Stump.Server.BaseServer.Logging;
using System.Globalization;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Social;
using System.Linq;
using Stump.Core.Extensions;

namespace Stump.Server.WorldServer.Game.Social
{
    public class ChatManager : DataManager<ChatManager>, ISaveable
    {
        #region Delegates

        /// <summary>
        ///   Delegate for parsing incomming in game messages.
        /// </summary>
        public delegate void ChatParserDelegate(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems);

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

        public List<BadWordRecord> BadWords;

        /// <summary>
        ///   Chat handler for each channel Id.
        /// </summary>
        public readonly Dictionary<ChatActivableChannelsEnum, ChatParserDelegate> ChatHandlers = new Dictionary<ChatActivableChannelsEnum, ChatParserDelegate>();

        private Dictionary<int, Emote> m_emotes = new Dictionary<int, Emote>();

        public IReadOnlyDictionary<int, Emote> Emotes => new ReadOnlyDictionary<int, Emote>(m_emotes);

        [Initialization(InitializationPass.First)]
        public void Initialize()
        {
            ChatHandlers.Clear();

            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_GLOBAL, SayGlobal);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_GUILD, SayGuild);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_PARTY, SayParty);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_ARENA, SayArena);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_SALES, SaySales);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_SEEK, SaySeek);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_ADMIN, SayAdministrators);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_TEAM, SayTeam);

            BadWords = Database.Query<BadWordRecord>(BadWordRelator.FetchQuery).ToList();
            m_emotes = Database.Query<Emote>(EmoteRelator.FetchQuery).ToDictionary(x => x.Id);
            World.Instance.RegisterSaveableInstance(this);
        }

        public string CanSendMessage(string message)
        {
            foreach (var badWord in BadWords)
            {
                if (message.ToLower().RemoveWhitespace().Contains(badWord.Text.ToLower()))
                    return badWord.Text;
            }

            return string.Empty;
        }

        public bool CanUseChannel(Character character, ChatActivableChannelsEnum channel)
        {
            switch (channel)
            {
                case ChatActivableChannelsEnum.CHANNEL_GLOBAL:
                    {
                        if (character.Map.IsMuted && character.UserGroup.Role <= AdministratorChatMinAccess)
                        {
                            character.SendServerMessage("La map est actuellement réduite au silence !");
                            return false;
                        }
                        return true;
                    }
                case ChatActivableChannelsEnum.CHANNEL_TEAM:
                    return character.IsFighting();
                case ChatActivableChannelsEnum.CHANNEL_ARENA:
                    return character.IsInParty(PartyTypeEnum.PARTY_TYPE_ARENA);
                case ChatActivableChannelsEnum.CHANNEL_GUILD:
                    return character.Guild != null;
                case ChatActivableChannelsEnum.CHANNEL_ALLIANCE:
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

        public Emote GetEmote(int id)
        {
            return m_emotes.TryGetValue(id, out var emote) ? emote : null;
        }

        #region Handlers

        public void HandleChat(WorldClient client, ChatActivableChannelsEnum channel, string message, IEnumerable<ObjectItem> objectItems = null)
        {
            if (!ChatHandlers.ContainsKey(channel))
                return;

            if (message.StartsWith(CommandPrefix) &&
                (message.Length < CommandPrefix.Length * 2 || message.Substring(CommandPrefix.Length, CommandPrefix.Length) != CommandPrefix)) // ignore processing command whenever there is the preffix twice
            {
                message = message.Remove(0, CommandPrefix.Length); // remove our prefix
                WorldServer.Instance.CommandManager.HandleCommand(new TriggerChat(new StringStream(UnescapeChatCommand(message)),
                                                                                  client.Character));
            }
            else
            {
                if (!CanUseChannel(client.Character, channel))
                    return;

                var badword = CanSendMessage(message);
                if (badword != string.Empty)
                {
                    client.Character.SendServerMessage($"Message non envoyé. Le terme <b>{badword}</b> est interdit sur le serveur !");
                    return;
                }

                if (client.Character.IsMuted())
                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 124, (int)client.Character.GetMuteRemainingTime().TotalSeconds);
                else
                {
                    if (client.Character.ChatHistory.RegisterAndCheckFlood(new ChatEntry(message, channel, DateTime.Now)))
                        ChatHandlers[channel](client, message, objectItems);
                }
            }

            var document = new BsonDocument
                    {
                        { "SenderId", client.Character.Id },
                        { "SenderName", client.Character.Name },
                        { "SenderAccountId", client.Account.Id },
                        { "ReceiverId", 0 },
                        { "ReceiverName", "" },
                        { "ReceiverAccountId", 0 },
                        { "Message", message },
                        { "Channel", (int)channel },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

            MongoLogger.Instance.Insert("Chats", document);
        }

        static string UnescapeChatCommand(string command)
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
                            sb.Append((char)id);
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

        private static void SendChatServerWithObjectMessage(IPacketReceiver client, INamedActor sender, ChatActivableChannelsEnum channel, string message, IEnumerable<ObjectItem> objectItems)
        {
            ChatHandler.SendChatServerWithObjectMessage(client, sender, channel, message, "", objectItems);
        }

        public void SayGlobal(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_GLOBAL))
                return;

            var clients = client.Character.Map.Clients;

            if (client.Character.IsFighting())
                clients = client.Character.Fight.Clients;
            else if (client.Character.IsSpectator())
                clients = client.Character.Fight.SpectatorClients;

            if (objectItems != null)
                //Impossible d'afficher des objets dans ce canal.
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 114);
            else
                SendChatServerMessage(clients, client.Character, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }

        public void SayGlobal(NamedActor actor, string msg)
        {
            SendChatServerMessage(actor.CharacterContainer.Clients, actor, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }

        public void SayAdministrators(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems)
        {
            if (client.UserGroup.Role < AdministratorChatMinAccess)
                return;

            World.Instance.ForEachCharacter(entry =>
            {
                if (!CanUseChannel(entry, ChatActivableChannelsEnum.CHANNEL_ADMIN))
                    return;

                if (objectItems != null)
                    SendChatServerWithObjectMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_ADMIN, msg, objectItems);
                else
                    SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_ADMIN, msg);
            });
        }

        public void SayParty(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_PARTY))
            {
                ChatHandler.SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_NO_PARTY);
                return;
            }

            client.Character.Party.ForEach(entry =>
            {
                if (objectItems != null)
                    SendChatServerWithObjectMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_PARTY, msg, objectItems);
                else
                    SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_PARTY, msg);
            });
        }
        public void SayArena(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_ARENA))
            {
                ChatHandler.SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_NO_PARTY_ARENA);
                return;
            }

            client.Character.ArenaParty.ForEach(entry =>
            {
                if (objectItems != null)
                    SendChatServerWithObjectMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_ARENA, msg, objectItems);
                else
                    SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_ARENA, msg);
            });
        }

        public void SayGuild(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_GUILD))
            {
                ChatHandler.SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_NO_GUILD);
                return;
            }

            if (objectItems != null)
                SendChatServerWithObjectMessage(client.Character.Guild.Clients, client.Character, ChatActivableChannelsEnum.CHANNEL_GUILD, msg, objectItems);
            else
                SendChatServerMessage(client.Character.Guild.Clients, client.Character, ChatActivableChannelsEnum.CHANNEL_GUILD, msg);
        }

        public void SayTeam(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_TEAM))
            {
                ChatHandler.SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_NO_TEAM);
                return;
            }

            foreach (var fighter in client.Character.Fighter.Team.GetAllFighters<CharacterFighter>())
            {
                if (objectItems != null)
                    SendChatServerWithObjectMessage(fighter.Character.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_TEAM, msg, objectItems);
                else
                    SendChatServerMessage(fighter.Character.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_TEAM, msg);
            }
        }

        public void SaySeek(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_SEEK))
                return;

            if (objectItems != null)
            {
                //Impossible d'afficher des objets dans ce canal.
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 114);
                return;
            }

            World.Instance.ForEachCharacter(entry =>
            {
                SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_SEEK, msg);
            });
        }

        public void SaySales(WorldClient client, string msg, IEnumerable<ObjectItem> objectItems)
        {
            if (!CanUseChannel(client.Character, ChatActivableChannelsEnum.CHANNEL_SALES))
                return;

            if (objectItems != null)
            {
                //Impossible d'afficher des objets dans ce canal.
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 114);
                return;
            }

            World.Instance.ForEachCharacter(entry =>
            {
                SendChatServerMessage(entry.Client, client.Character, ChatActivableChannelsEnum.CHANNEL_SALES, msg);
            });
        }

        public static bool IsGlobalChannel(ChatActivableChannelsEnum channel)
        {
            return channel == ChatActivableChannelsEnum.CHANNEL_SALES ||
                   channel == ChatActivableChannelsEnum.CHANNEL_SEEK;
        }

        public void Save()
        {
            foreach (var badWord in BadWords.Where(x => x.IsDirty || x.IsNew))
            {
                if (badWord.IsNew)
                    Database.Insert(badWord);
                else
                    Database.Update(badWord);

                badWord.IsNew = false;
                badWord.IsDirty = false;
            }
        }

        #endregion
    }
}
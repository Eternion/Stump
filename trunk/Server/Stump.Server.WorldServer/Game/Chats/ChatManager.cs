using System.Collections.Generic;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Chat;

namespace Stump.Server.WorldServer.Game.Chats
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
        ///   Chat handler for each channel Id.
        /// </summary>
        public readonly Dictionary<ChatActivableChannelsEnum, ChatParserDelegate> ChatHandlers = new Dictionary<ChatActivableChannelsEnum, ChatParserDelegate>();

        [Initialization(InitializationPass.First)]
        public void Initialize()
        {
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_GLOBAL, SayGlobal);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_PARTY, SayParty);
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
                    return true;
                case ChatActivableChannelsEnum.CHANNEL_TEAM:
                    return character.IsFighting();
                case ChatActivableChannelsEnum.CHANNEL_GUILD:
                    return false;
                case ChatActivableChannelsEnum.CHANNEL_ALIGN:
                    return false;
                case ChatActivableChannelsEnum.CHANNEL_PARTY:
                    return character.IsInParty();
                case ChatActivableChannelsEnum.CHANNEL_SALES:
                    return true;
                case ChatActivableChannelsEnum.CHANNEL_SEEK:
                    return true;
                case ChatActivableChannelsEnum.CHANNEL_NOOB:
                    return true;
                case ChatActivableChannelsEnum.CHANNEL_ADMIN:
                    return character.Client.Account.Role >= AdministratorChatMinAccess;
                case ChatActivableChannelsEnum.CHANNEL_ADS:
                    return false;
                case ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE:
                    return true;
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
            if (!CanUseChannel(client.ActiveCharacter, channel))
                return;

            if (!ChatHandlers.ContainsKey(channel))
                return;

            if (message.StartsWith(CommandPrefix))
            {
                message = message.Remove(0, CommandPrefix.Length); // remove our prefix
                WorldServer.Instance.CommandManager.HandleCommand(new TriggerChat(new StringStream(message),
                                                                                  client.ActiveCharacter));
            }
            else
                ChatHandlers[channel](client, message);
        }

        private void SendChatServerMessage(IPacketReceiver client, Character sender, ChatActivableChannelsEnum channel, string message)
        {
            if (sender.Client.Account.Role >= AdministratorChatMinAccess)
                ChatHandler.SendChatAdminServerMessage(client, sender, channel, message);
            else
                ChatHandler.SendChatServerMessage(client, sender, channel, message);
        }

        private void SendChatServerMessage(IPacketReceiver client, NamedActor sender, ChatActivableChannelsEnum channel, string message)
        {
            ChatHandler.SendChatServerMessage(client, sender, channel, message);
        }

        public void SayGlobal(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_GLOBAL))
                return;

            SendChatServerMessage(client.ActiveCharacter.CharacterContainer.Clients, client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }

        public void SayGlobal(NamedActor actor, string msg)
        {
            SendChatServerMessage(actor.CharacterContainer.Clients, actor, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }

        public void SayAdministrators(WorldClient client, string msg)
        {
            if (client.Account.Role < AdministratorChatMinAccess)
                return;

            World.Instance.ForEachCharacter(entry =>
            {
                if (CanUseChannel(entry, ChatActivableChannelsEnum.CHANNEL_ADMIN))
                    ChatHandler.SendChatAdminServerMessage(entry.Client, client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_ADMIN, msg);
            });
        }

        public void SayParty(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_PARTY))
                return;

            client.ActiveCharacter.Party.ForEach(entry => SendChatServerMessage(entry.Client, client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_PARTY, msg));
        }

        public void SayTeam(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_TEAM))
                return;

            foreach (var fighter in client.ActiveCharacter.Fighter.Team.GetAllFighters<CharacterFighter>())
            {
                SendChatServerMessage(fighter.Character.Client, client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_TEAM, msg);
            }
        }

        public void SaySeek(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_SEEK))
                return;

            World.Instance.ForEachCharacter(entry =>
                SendChatServerMessage(entry.Client, client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_SEEK, msg));
        }

        public void SaySales(WorldClient client, string msg)
        {
            if (!CanUseChannel(client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_SALES))
                return;

            World.Instance.ForEachCharacter(entry =>
                SendChatServerMessage(entry.Client, client.ActiveCharacter, ChatActivableChannelsEnum.CHANNEL_SALES, msg));
        }

        #endregion
    }
}
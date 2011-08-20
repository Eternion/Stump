using System;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Handlers.Chat;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Chat
{
    public class ChatManager : Singleton<ChatManager>
    {
        #region Delegates

        /// <summary>
        ///   Delegate for parsing incomming in game messages.
        /// </summary>
        public delegate void ChatParserDelegate(WorldClient client, ChannelId chanid, string msg);

        #endregion

        /// <summary>
        /// Prefix used for chat commands
        /// </summary>
        [Variable]
        public static readonly string CommandPrefix = "#";

        /// <summary>
        ///   Chat handler for each channel Id.
        /// </summary>
        public readonly ChatParserDelegate[] ChatHandlers = new ChatParserDelegate[(int) ChannelId.End];

        [Initialization(InitializationPass.First)]
        public void Initialize()
        {
            ChatHandlers[(int) ChannelId.General] = SayGeneral;
            /*ChatHandlers[(int)ChannelId.Team] = SayTeam;
            ChatHandlers[(int)ChannelId.Guild] = SayGuild;
            ChatHandlers[(int)ChannelId.Alignment] = SayAlignment;*/
            ChatHandlers[(int) ChannelId.Group] = SayGroup;
            /*ChatHandlers[(int)ChannelId.Trade] = SayTrade;
            ChatHandlers[(int)ChannelId.Recruitment] = SayRecruitment;
            ChatHandlers[(int)ChannelId.Newbies] = SayNewbies;*/
            ChatHandlers[(int) ChannelId.Administrators] = SayAdministrators;
            /*ChatHandlers[(int)ChannelId.Private] = SayPrivate;
            ChatHandlers[(int)ChannelId.Information] = SayInformation;
            ChatHandlers[(int)ChannelId.Fight] = SayFight;*/
        }

        #region Handlers

        public void SayGeneral(WorldClient client, ChannelId chanid, string msg)
        {
            if (msg.StartsWith(CommandPrefix))
            {
                msg = msg.Remove(0, 1); // remove our prefix
                WorldServer.Instance.CommandManager.HandleCommand(new TriggerChat(new StringStream(msg),
                                                                                  client.ActiveCharacter));
            }
            else
            {
                Action<Character> action =
                    charac => ChatHandler.SendChatServerMessage(charac.Client, client.ActiveCharacter, chanid, msg);

                    client.ActiveCharacter.Context.DoForAll(action);
            }
        }

        public void SayGeneral(NamedActor actor, ChannelId chanid, string msg)
        {
                Action<Character> action =
                    entry => ChatHandler.SendChatServerMessage(entry.Client, actor, chanid, msg);

                actor.Context.DoForAll(action);
        }

        public void SayAdministrators(WorldClient client, ChannelId chanid, string msg)
        {
            if (client.Account.Role == RoleEnum.Administrator)
            {
                Action<Character> action = charac =>
                {
                    if (charac.Client.Account.Role == RoleEnum.Administrator)
                        ChatHandler.SendChatServerMessage(charac.Client, client.ActiveCharacter, chanid, msg);
                };

                client.ActiveCharacter.Context.DoForAll(action);
            }
        }

        public void SayGroup(WorldClient client, ChannelId chanid, string msg)
        {
            if (client.ActiveCharacter.IsInParty())
            {
                client.ActiveCharacter.Party.DoForAll(entry => ChatHandler.SendChatServerMessage(entry.Client, client.ActiveCharacter, chanid, msg));
            }
            else
            {
                // todo : send message "no parties"
            }
        }

        #endregion
    }
}
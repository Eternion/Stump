// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Chat
{
    public static class ChatManager
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
        public static string CommandPrefix = "#";

        /// <summary>
        ///   Chat handler for each channel Id.
        /// </summary>
        public static readonly ChatParserDelegate[] ChatHandlers = new ChatParserDelegate[(int) ChannelId.End];

        static ChatManager()
        {
            RegisterHandlers();
        }


        private static void RegisterHandlers()
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

        private static void SayGeneral(WorldClient client, ChannelId chanid, string msg)
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

                if (client.ActiveCharacter.IsInFight)
                    client.ActiveCharacter.Fight.CallOnAllCharacters(action);
                else
                    client.ActiveCharacter.Map.CallOnAllCharactersWithoutFighters(action);
            }
        }

        private static void SayAdministrators(WorldClient client, ChannelId chanid, string msg)
        {
            if (client.Account.Role == RoleEnum.Administrator)
            {
                Action<Character> action = charac =>
                {
                    if (charac.Client.Account.Role == RoleEnum.Administrator)
                        ChatHandler.SendChatServerMessage(charac.Client, client.ActiveCharacter, chanid, msg);
                };

                client.ActiveCharacter.Map.CallOnAllCharacters(action);
            }
        }

        private static void SayGroup(WorldClient client, ChannelId chanid, string msg)
        {
//            if (client.ActiveCharacter.IsInGroup)
//            {
//                Action<Character> action = charac =>
//                {
//                    if (charac.Client.ActiveCharacter.GroupMember == client.ActiveCharacter.GroupMember)
//                        ChatHandler.SendChatServerMessage(charac.Client, client.ActiveCharacter, chanid, msg);
//                };
//
//                client.ActiveCharacter.Map.CallOnAllCharacters(action);
//            }
//            else
//            {
                // Envoyer message d'erreur "pas de groupe".
//            }
        }

        #endregion
    }
}
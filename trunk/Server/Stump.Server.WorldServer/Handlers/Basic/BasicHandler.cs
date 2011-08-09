using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Basic
{
    public class BasicHandler : WorldHandlerContainer
    {
        [WorldHandler(BasicSwitchModeRequestMessage.Id)]
        public static void HandleBasicSwitchModeRequestMessage(WorldClient client, BasicSwitchModeRequestMessage message)
        {
        }

        [WorldHandler(BasicWhoAmIRequestMessage.Id)]
        public static void HandleBasicWhoAmIRequestMessage(WorldClient client, BasicWhoAmIRequestMessage message)
        {
            /* Get Current character */
            Character character = client.ActiveCharacter;

            /* Send informations about it */
            client.Send(new BasicWhoIsMessage(true, (byte) character.Client.Account.Role,
                                              character.Client.WorldAccount.Nickname, character.Name, (short) character.Map.SubArea.Id));
        }

        [WorldHandler(BasicWhoIsRequestMessage.Id)]
        public static void HandleBasicWhoIsRequestMessage(WorldClient client, BasicWhoIsRequestMessage message)
        {
            /* Get character */
            Character character = World.Instance.GetCharacter(message.search);

            /* check null */
            if (character == null)
            {
                client.Send(new BasicWhoIsNoMatchMessage(message.search));
            }
                /* Send info about it */
            else
            {
                client.Send(new BasicWhoIsMessage(message.search == client.ActiveCharacter.Name,
                                                  (byte) character.Client.Account.Role,
                                                  character.Client.WorldAccount.Nickname, character.Name,
                                                  (short) character.Map.SubArea.Id));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name = "client"></param>
        /// <param name = "msgType"></param>
        /// <param name = "msgId"></param>
        /// <param name = "arguments"></param>
        /// <remarks>
        ///   Message id = <paramref name = "msgType" /> * 10000 + <paramref name = "msgId" />
        /// </remarks>
        public static void SendTextInformationMessage(WorldClient client, byte msgType, short msgId,
                                                      params string[] arguments)
        {
            client.Send(new TextInformationMessage(msgType, msgId, arguments));
        }

        public static void SendTextInformationMessage(WorldClient client, byte msgType, short msgId,
                                                      params object[] arguments)
        {
            client.Send(new TextInformationMessage(msgType, msgId, arguments.Select(entry => entry.ToString())));
        }

        public static void SendTextInformationMessage(WorldClient client, byte msgType, short msgId)
        {
            client.Send(new TextInformationMessage(msgType, msgId, new string[0]));
        }

        public static void SendBasicTimeMessage(WorldClient client)
        {
            var unixTimeStamp = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            var offset = (short) TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds;
            client.Send(new BasicTimeMessage(unixTimeStamp, offset));
        }

        public static void SendBasicNoOperationMessage(WorldClient client)
        {
            client.Send(new BasicNoOperationMessage());
        }
    }
}
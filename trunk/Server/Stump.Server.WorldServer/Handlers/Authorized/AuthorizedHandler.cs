using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Authorized
{
    public class AuthorizedHandler : WorldHandlerContainer
    {
        [WorldHandler(AdminQuietCommandMessage.Id)]
        public static void HandleAdminQuietCommandMessage(WorldClient client, AdminQuietCommandMessage message)
        {
            if (client.Account.Role < RoleEnum.GameMaster_Padawan)
                return;

            string[] data = message.content.Split(' ');
            string command = data[0];

            switch (command)
            {
                case ("look"):
                {
                    WorldServer.Instance.CommandManager.HandleCommand(new TriggerConsole("look " + data[2], client.Character));
                    break;
                }
                case ("moveto"):
                {
                    string id = data[1];

                    WorldServer.Instance.CommandManager.HandleCommand(
                        new TriggerConsole(string.Format("go {0}", id), client.Character));
                    break;
                }
            }
        }


        [WorldHandler(AdminCommandMessage.Id)]
        public static void HandleAdminCommandMessage(WorldClient client, AdminCommandMessage message)
        {
            if (client.Account.Role < RoleEnum.GameMaster_Padawan)
                return;

            if (client.Character == null)
                return;

            WorldServer.Instance.CommandManager.HandleCommand(new TriggerConsole(new StringStream(message.content),
                                                                                 client.Character));
        }

        public static void SendConsoleMessage(IPacketReceiver client, string text)
        {
            SendConsoleMessage(client, ConsoleMessageTypeEnum.CONSOLE_TEXT_MESSAGE, text);
        }

        public static void SendConsoleMessage(IPacketReceiver client, ConsoleMessageTypeEnum type, string text)
        {
            client.Send(new ConsoleMessage((sbyte) type, text));
        }
    }
}
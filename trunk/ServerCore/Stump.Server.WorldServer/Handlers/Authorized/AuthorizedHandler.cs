
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Commands;

namespace Stump.Server.WorldServer.Handlers
{
    public class AuthorizedHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (AdminQuietCommandMessage))]
        public static void HandleAdminQuietCommandMessage(WorldClient client, AdminQuietCommandMessage message)
        {
            if (client.Account.Role < RoleEnum.GameMaster_Padawan)
                return;

            string[] data = message.content.Split(' ');
            string command = data[0];
            string[] args = data[1].Split(' ');

            switch (command)
            {
                case ("look"):
                {
                    WorldServer.Instance.CommandManager.HandleCommand(
                        new TriggerConsole(message.content, client.ActiveCharacter));
                    break;
                }
                case ("moveto"):
                {
                    string x = args[0].Split(',').First();
                    string y = args[0].Split(',').Last();

                    WorldServer.Instance.CommandManager.HandleCommand(
                        new TriggerChat(string.Format("gopos * {0} {1}", x, y), client.ActiveCharacter));
                    break;
                }
            }
        }


        [WorldHandler(typeof (AdminCommandMessage))]
        public static void HandleAdminCommandMessage(WorldClient client, AdminCommandMessage message)
        {
            if (client.Account.Role < RoleEnum.GameMaster_Padawan)
                return;

            WorldServer.Instance.CommandManager.HandleCommand(new TriggerConsole(new StringStream(message.content),
                                                                                 client.ActiveCharacter));
        }

        public static void SendConsoleMessage(WorldClient client, string text)
        {
            SendConsoleMessage(client, ConsoleMessageTypeEnum.CONSOLE_TEXT_MESSAGE, text);
        }

        public static void SendConsoleMessage(WorldClient client, ConsoleMessageTypeEnum type, string text)
        {
            client.Send(new ConsoleMessage((uint) type, text));
        }
    }
}
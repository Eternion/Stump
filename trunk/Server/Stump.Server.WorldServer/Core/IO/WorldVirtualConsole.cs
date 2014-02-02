using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;

namespace Stump.Server.WorldServer.Core.IO
{
    public class WorldVirtualConsole : ConsoleBase, ICommandsUser
    {
        public void EnterCommand(string Cmd, Action<bool, string> callback)
        {
            if (!WorldServer.Instance.Running)
                return;

            if (Cmd == "")
                return;

            WorldServer.Instance.CommandManager.HandleCommand(
                new WorldVirtualConsoleTrigger(new StringStream(Cmd), callback));
        }

        private readonly List<string> m_commands = new List<string>();
        public List<string> Commands
        {
            get
            {
                return m_commands;
            }
        }

        private readonly List<KeyValuePair<string, Exception>> m_commandsError = new List<KeyValuePair<string, Exception>>();
        public List<KeyValuePair<string, Exception>> CommandsErrors
        {
            get
            {
                return m_commandsError;
            }
        }
    }
}

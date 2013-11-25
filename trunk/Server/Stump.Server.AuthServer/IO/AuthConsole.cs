using System;
using System.Collections.Generic;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Server.AuthServer.Commands.Trigger;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.IO
{
    public class AuthConsole : ConsoleBase, ICommandsUser
    {
        /// <summary>
        /// Prefix using for server's commands
        /// </summary>
        [Variable]
        public static string CommandPreffix = "";

        public AuthConsole()
        {
            m_conditionWaiter.Success += OnConsoleKeyPressed;
        }

        protected override void Process()
        {
            m_conditionWaiter.Start();
        }

        private void OnConsoleKeyPressed(object sender, EventArgs e)
        {
            EnteringCommand = true;

            if (!AuthServer.Instance.Running)
            {
                EnteringCommand = false;
                return;
            }

            try
            {
                Cmd = Console.ReadLine();
            }
            catch (Exception)
            {
                EnteringCommand = false;
                return;
            }

            if (Cmd == null || !AuthServer.Instance.Running)
            {
                EnteringCommand = false;
                return;
            }

            EnteringCommand = false;

            lock (Console.Out)
            {
                try
                {
                    if (!Cmd.StartsWith(CommandPreffix))
                        return;

                    Cmd = Cmd.Substring(CommandPreffix.Length);
                    AuthServer.Instance.CommandManager.HandleCommand(
                        new AuthConsoleTrigger(new StringStream(Cmd)));
                }
                finally
                {
                    m_conditionWaiter.Start();
                }
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
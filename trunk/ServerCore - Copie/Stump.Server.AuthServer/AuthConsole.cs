
using System;
using System.Threading;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.Server.AuthServer.Commands;
using Stump.Server.BaseServer;

namespace Stump.Server.AuthServer
{
    public class AuthConsole : ConsoleBase
    {
        /// <summary>
        /// Prefix using for server's commands
        /// </summary>
        [Variable]
        public static string CommandPreffix = "";

        public AuthConsole()
        {
            m_conditionWaiter.Success += m_conditionWaiter_Success;
        }

        protected override void Process()
        {
            m_conditionWaiter.Start();
        }

        private void m_conditionWaiter_Success(object sender, EventArgs e)
        {
            EnteringCommand = true;

            if (!AuthentificationServer.Instance.Running)
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

            if (Cmd == null || !AuthentificationServer.Instance.Running)
            {
                EnteringCommand = false;
                return;
            }

            EnteringCommand = false;

            lock (Console.Out)
            {
                try
                {
                    if (Cmd.StartsWith(CommandPreffix))
                    {
                        Cmd = Cmd.Substring(CommandPreffix.Length);
                        AuthentificationServer.Instance.CommandManager.HandleCommand(
                            new AuthConsoleTrigger(new StringStream(Cmd)));
                    }
                }
                finally
                {
                    m_conditionWaiter.Start();
                }
            }
        }
    }
}
using System;
using System.Diagnostics;
using Stump.Core.Attributes;
using Stump.Server.BaseServer;

namespace Stump.Server.WorldServer.IO
{
    public class WorldConsole : ConsoleBase
    {
        /// <summary>
        /// Prefix used for server's commands
        /// </summary>
        [Variable]
        public static string CommandPreffix = "";

        public WorldConsole()
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

            if (!WorldServer.Instance.Running)
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

            if (Cmd == null || !WorldServer.Instance.Running)
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
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        //WorldServer.Instance.CommandManager.HandleCommand(new WorldConsoleTrigger(Cmd));
                        stopwatch.Stop();

                        Debug.WriteLine(stopwatch.ElapsedMilliseconds + " ms");
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
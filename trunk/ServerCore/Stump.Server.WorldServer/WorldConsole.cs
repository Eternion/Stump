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
using Stump.Server.BaseServer;
using Stump.Server.WorldServer.Commands;

namespace Stump.Server.WorldServer
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
            m_conditionWaiter.Success += m_conditionWaiter_Success;
        }

        protected override void Process()
        {
            m_conditionWaiter.Start();
        }

        private void m_conditionWaiter_Success(object sender, EventArgs e)
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
                        WorldServer.Instance.CommandManager.HandleCommand(
                            new WorldConsoleTrigger(new StringStream(Cmd)));
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
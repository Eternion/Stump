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
using System.Threading;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.IO;
using Stump.BaseCore.Framework.Utils;
using Stump.Server.AuthServer.Commands;
using Stump.Server.BaseServer;

namespace Stump.Server.AuthServer
{
    public class AuthConsole : ConsoleBase
    {
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
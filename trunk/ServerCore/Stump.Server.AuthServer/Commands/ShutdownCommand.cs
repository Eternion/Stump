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
using System.Collections.Generic;
using System.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public class ShutdownCommand : AuthCommand
    {
        private Timer m_shutdownTimer;

        public ShutdownCommand()
        {
            Aliases = new[] {"shutdown", "stop"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Stop the server";
            Usage = "";

            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<int>("time", "t", "Stop after [time] seconds", true),
                    new CommandParameter<object>("cancel", "c", "Cancel a shutting down procedure", true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            if (trigger.ArgumentExists("cancel") && m_shutdownTimer != null)
            {
                m_shutdownTimer.Dispose();

                trigger.Reply("Shutting down procedure is cancel.");
                return;
            }

            var time = trigger.GetArgument<int>("time");

            if (time > 0)
                trigger.Reply("Server shutting down in {0} seconds", time);

            m_shutdownTimer = new Timer(Shutdown, null, time * 1000, Timeout.Infinite);
        }

        private static void Shutdown(object arg)
        {
            AuthentificationServer.Instance.Shutdown();
        }
    }
}
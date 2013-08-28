using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class ShutdownCommand : CommandBase
    {
        private Timer m_shutdownTimer;
        private int m_shutdownCountdown = 0;

        public ShutdownCommand()
        {
            Aliases = new[] {"shutdown", "stop"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Stop the server";
            Usage = "";

            AddParameter("time", "t", "Stop after [time] seconds", 0, true);
            AddParameter<string>("reason", "r", "Display a reason for the shutdown", isOptional: true);
            AddParameter<bool>("cancel", "c", "Cancel a shutting down procedure", isOptional:true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (trigger.Get<bool>("cancel"))
            {
                ServerBase.InstanceAsBase.CancelScheduledShutdown();
                trigger.Reply("Shutting down procedure is canceled.");
                return;
            }

            m_shutdownCountdown = trigger.Get<int>("time");

            if (m_shutdownCountdown > 0)
            {
                ServerBase.InstanceAsBase.ScheduleShutdown(TimeSpan.FromSeconds(m_shutdownCountdown),
                                                           trigger.Get<string>("reason"));
                trigger.Reply("Server shutting down in {0} seconds", m_shutdownCountdown);

            }
            else
            {
                ServerBase.InstanceAsBase.Shutdown();
            }
        }
    }
}
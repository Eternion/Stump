using System;
using System.Threading;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class ShutdownCommand : CommandBase
    {
        private Timer m_shutdownTimer;
        private int m_shutdownCountdown;

        public ShutdownCommand()
        {
            Aliases = new[] {"shutdown", "stop"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Stop the server";
            Usage = "";

            AddParameter<int>("time", "t", "Stop after [time] seconds");
            AddParameter<bool>("cancel", "c", "Cancel a shutting down procedure", isOptional:true);
            AddParameter<bool>("info", "i", "Informations about the current shutdown", isOptional: true);
            AddParameter<bool>("noexit", "n", "No exit shutdown", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (trigger.Get<bool>("cancel"))
            {
                ServerBase.InstanceAsBase.CancelScheduledShutdown();
                trigger.Reply("Shutting down procedure is canceled.");
                return;
            }

            if (trigger.Get<bool>("info"))
            {
                trigger.Reply($"Shutdown Date: {ServerBase.InstanceAsBase.ScheduledShutdownDate}");
                return;
            }

            trigger.Reply($"No exit shutdown: {ServerBase.InstanceAsBase.IsNoExitShutdown = trigger.Get<bool>("noexit")}");

            m_shutdownCountdown = trigger.Get<int>("time");

            if (m_shutdownCountdown > 0)
            {
                ServerBase.InstanceAsBase.ScheduleShutdown(TimeSpan.FromSeconds(m_shutdownCountdown));
                trigger.Reply("Server shutting down in {0} seconds", m_shutdownCountdown);

            }
            else
            {
                ServerBase.InstanceAsBase.Shutdown();
            }
        }
    }
}
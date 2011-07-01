using System.Collections.Generic;
using System.Threading;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class ShutdownCommand : CommandBase
    {
        private Timer m_shutdownTimer;

        public ShutdownCommand()
        {
            Aliases = new[] {"shutdown", "stop"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Stop the server";
            Usage = "";

            Parameters = new List<IParameterDefinition>
                {
                    new ParameterDefinition<int>("time", "t", "Stop after [time] seconds", 0, true),
                    new ParameterDefinition<bool>("cancel", "c", "Cancel a shutting down procedure", true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            if (trigger.IsArgumentDefined("cancel") && trigger.Get<bool>("cancel"))
            {
                if (m_shutdownTimer != null)
                    m_shutdownTimer.Dispose();

                trigger.Reply("Shutting down procedure is canceled.");
                return;
            }

            var time = trigger.Get<int>("time");

            if (time > 0)
                trigger.Reply("Server shutting down in {0} seconds", time);

            m_shutdownTimer = new Timer(Shutdown, null, time * 1000, Timeout.Infinite);
        }

        private static void Shutdown(object arg)
        {
            ServerBase.InstanceAsBase.Shutdown();
        }
    }
}
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

            m_shutdownCountdown = trigger.Get<int>("time");

            if (m_shutdownCountdown > 0)
                trigger.Reply("Server shutting down in {0} seconds", m_shutdownCountdown);

            m_shutdownTimer = new Timer(Shutdown, null, 1000, Timeout.Infinite);
        }

        private void Shutdown(object arg)
        {
            if (m_shutdownCountdown <= 0)
            {
                if (m_shutdownTimer != null)
                    m_shutdownTimer.Dispose();

                ServerBase.InstanceAsBase.Shutdown();
            }
            else
            {
                //World.Instance.SendAnnounce("Server will restart in " + m_shutdownCountdown + " seconds", Color.Red);
            }

            m_shutdownCountdown -= 1;
        }
    }
}
using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Plugins.DebugPlugin.Commands
{
    public class BugCommand : CommandBase
    {
        public BugCommand()
        {
            Aliases = new[] { "bug" };
            RequiredRole = RoleEnum.Player;
            Usage = "bug {bug description}";
            Description = "Report a bug";
        }

        public override void Execute(TriggerBase trigger)
        {
            var report = trigger.Args.NextWords();

            // todo : report
        }
    }
}
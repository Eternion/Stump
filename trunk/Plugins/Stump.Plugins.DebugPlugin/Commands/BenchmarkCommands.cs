using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Plugins.DebugPlugin.Commands
{
    public class BenchmarkCommand : SubCommandContainer
    {
        public BenchmarkCommand()
        {
            Aliases = new[] { "benchmark", "bench" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Survey emulator performances";
        }
    }

    public class BenchmarkOverviewCommand : SubCommand
    {
        public BenchmarkOverviewCommand()
        {
            Aliases = new[] { "overview" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Get a performance overview";
            ParentCommand = typeof(BenchmarkCommand);
        }

        public override void Execute(TriggerBase trigger)
        {
            
        }
    }
}
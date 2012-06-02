using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Benchmark;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class BenchmarkCommands : SubCommandContainer
    {
        public BenchmarkCommands()
        {
            Aliases = new[] { "benchmark", "bench" };
            RequiredRole = RoleEnum.Administrator;
        }
    }

    public class BenchmarkSummaryCommand : SubCommand
    {
        public BenchmarkSummaryCommand()
        {
            Aliases = new[] { "summary", "sum" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommand = typeof(BenchmarkCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(BenchmarkManager.Instance.GenerateReport());
        }
    }

    public class BenchmarkEnableCommand : SubCommand
    {
        public BenchmarkEnableCommand()
        {
            Aliases = new[] { "enable", "on" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommand = typeof(BenchmarkCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            BenchmarkManager.Enable = true;
        }
    }

    public class BenchmarkDisableCommand : SubCommand
    {
        public BenchmarkDisableCommand()
        {
            Aliases = new[] { "disable", "off" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommand = typeof(BenchmarkCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            BenchmarkManager.Enable = false;
        }
    }
}
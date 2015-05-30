using System;
using System.Collections.Generic;
using CSScriptLibrary;
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
            ParentCommandType = typeof(BenchmarkCommands);
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
            ParentCommandType = typeof(BenchmarkCommands);
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
            ParentCommandType = typeof(BenchmarkCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            BenchmarkManager.Enable = false;
        }
    }
    
    public class BenchmarkLimitCommand : SubCommand
    {
        public BenchmarkLimitCommand()
        {
            Aliases = new[] { "limit" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
            AddParameter<int>("limit", "l", "Entries limit");
        }

        public override void Execute(TriggerBase trigger)
        {
            BenchmarkManager.EntriesLimit = trigger.Get<int>("limit");
        }
    }

    public class BenchmarkIOInfoCommand : SubCommand
    {
         public BenchmarkIOInfoCommand()
        {
            Aliases = new[] { "ioinfo" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(ServerBase.InstanceAsBase.IOTaskPool.GetDebugInformations());
        }
    }

    public class BenchmarkPingIOCommand : SubCommand
    {
        public BenchmarkPingIOCommand()
        {
            Aliases = new[] { "pingio" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
            AddParameter<int>("times", "t", "Pings count", 20);
        }

        public override void Execute(TriggerBase trigger)
        {
            int i = 0;
            int count = trigger.Get<int>("times");
            PingIO(trigger, 0, trigger.Get<int>("times"), new List<DateTime>() { DateTime.Now});
        }

        private void PingIO(TriggerBase trigger, int i, int count, List<DateTime> dates)
        {
            if (i >= count)
            {
                double sum = 0;
                for (int j = 1; j < dates.Count; j++)
                {
                    sum += (dates[j] - dates[j - 1]).TotalMilliseconds;
                    trigger.Reply("{0:F1} ms", (dates[j] - dates[j - 1]).TotalMilliseconds);
                }

                trigger.Reply("Average : {0} ms", sum/(dates.Count - 1));
            }
            else
            {
                ServerBase.InstanceAsBase.IOTaskPool.AddMessage(() =>
                {
                    dates.Add(DateTime.Now);
                    PingIO(trigger, i + 1, count, dates);
                });
            }
        }
    }
}
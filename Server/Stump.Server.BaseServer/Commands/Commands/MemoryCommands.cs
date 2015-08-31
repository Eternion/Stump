using System;
using System.IO;
using System.Linq;
using System.Text;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class BufferCommand : CommandBase
    {
        public BufferCommand()
        {
            Aliases = new[] {"buffer"};
            Description = "Gives buffer manager current state";
            RequiredRole = RoleEnum.Administrator;
        }

        public override void Execute(TriggerBase trigger)
        {
            PrintBufferInformations(trigger, BufferManager.Default, "Default");
            PrintBufferInformations(trigger, BufferManager.Tiny, "Tiny");
            PrintBufferInformations(trigger, BufferManager.Small, "Small");
            PrintBufferInformations(trigger, BufferManager.Large, "Large");
            PrintBufferInformations(trigger, BufferManager.ExtraLarge, "ExtraLarge");
            PrintBufferInformations(trigger, BufferManager.SuperSized, "SuperSized");
        }

        private static void PrintBufferInformations(TriggerBase trigger, BufferManager manager, string name)
        {
            trigger.Reply("- {0} ({1}):", name, manager.SegmentSize);
            trigger.Reply("\tInUse : " + manager.InUse);
            trigger.Reply("\tTotalAllocatedMemory : " + manager.TotalAllocatedMemory.ToString(new FileSizeFormatProvider()));
            trigger.Reply("\tAvailableSegmentsCount : " + manager.AvailableSegmentsCount);
            trigger.Reply("\tUsedSegmentCount : " + manager.UsedSegmentCount);
            trigger.Reply("\tTotalSegmentCount : " + manager.TotalSegmentCount);
            trigger.Reply("");
        }
    }

#if DEBUG
    public class BufferLeaksCommand : CommandBase
    {
        public BufferLeaksCommand()
        {
            Aliases = new[] {"bufferleaks"};
            Description = "Find possible leaks";
            RequiredRole = RoleEnum.Administrator;
        }

        public override void Execute(TriggerBase trigger)
        {
            var path = string.Format("./bufferleaks {0}.txt", DateTime.Now.ToString("dd-MM hh-mm-ss"));
            


            using (var file = File.OpenWrite(path))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLine("Connected : {0}", ClientManager.Instance.Count);

                    PrintBufferInformations(writer, BufferManager.Default);
                    PrintBufferInformations(writer, BufferManager.Tiny);
                    PrintBufferInformations(writer, BufferManager.Small);
                    PrintBufferInformations(writer, BufferManager.Large);
                    PrintBufferInformations(writer, BufferManager.ExtraLarge);
                    PrintBufferInformations(writer, BufferManager.SuperSized);
                }
            }

            trigger.Reply("Wrote {0}", Path.GetFullPath(path));
        }

        private static void PrintBufferInformations(StreamWriter file, BufferManager manager)
        {
            var leaks = manager.GetSegmentsInUse();

            file.WriteLine("------------- {0} kB {1}/{2} used segments -------------", manager.SegmentSize/1024, manager.UsedSegmentCount, manager.TotalSegmentCount);
            foreach (var leak in leaks.OrderByDescending(x => DateTime.Now - x.LastUsage))
            {
                var client = leak.Token as BaseClient;
                if (client != null && !client.Connected)
                {
                    file.WriteLine("Client {0} Connected {1}", client, client.Connected);
                }
                if (client == null || !client.Connected)
                {
                    file.WriteLine("Buffer #{0} Size:{1} LastUsage:{2}ago Uses:{4}  Stack:{3}", leak.Number, leak.Length,
                        (DateTime.Now - leak.LastUsage).ToPrettyFormat(), leak.LastUserTrace, leak.Uses);

                    file.WriteLine("");
                }
            }
        }
    }
#endif
}
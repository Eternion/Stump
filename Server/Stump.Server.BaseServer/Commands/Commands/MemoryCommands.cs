using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;

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
}
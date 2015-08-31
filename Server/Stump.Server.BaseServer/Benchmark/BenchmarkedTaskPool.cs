using Stump.Core.Threading;

namespace Stump.Server.BaseServer.Benchmark
{
    public class BenchmarkedTaskPool : SelfRunningTaskPool
    {
        public BenchmarkedTaskPool(int interval, string name) : base(interval, name)
        {
        }

        public override void AddMessage(IMessage message)
        {
            if (BenchmarkManager.Enable)
                base.AddMessage(new BenchmarkingMessage(message));
            else
                base.AddMessage(message);
        }
    }
}
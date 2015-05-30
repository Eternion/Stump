using System.Diagnostics;
using Stump.Core.Threading;

namespace Stump.Server.BaseServer.Benchmark
{
    public class BenchmarkingMessage : IMessage
    {
        private readonly IMessage m_message;

        public BenchmarkingMessage(IMessage message)
        {
            m_message = message;
        }

        public void Execute()
        {
            if (!BenchmarkManager.Enable)
                m_message.Execute();
            else
            {
                var sw = new Stopwatch();
                sw.Start();
                m_message.Execute();
                sw.Stop();
                BenchmarkManager.Instance.Add(BenchmarkEntry.Create(m_message + "[IO]", sw.Elapsed));
            }
        }
    }
}
using System;
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

        public TimeSpan ElapsedTime
        {
            get;
            private set;
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
                ElapsedTime = sw.Elapsed;

                if (sw.ElapsedMilliseconds>1)
                    BenchmarkManager.Instance.Add(BenchmarkEntry.Create(m_message + "[IO]", sw.Elapsed, "type", "io"));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1:F} ms", m_message, ElapsedTime.TotalMilliseconds);
        }
    }
}
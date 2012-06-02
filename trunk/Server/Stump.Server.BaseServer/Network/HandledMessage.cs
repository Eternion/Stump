using System;
using System.Diagnostics;
using NLog;
using Stump.Core.Threading;
using Stump.Server.BaseServer.Benchmark;
using Stump.Server.BaseServer.Exceptions;
using Message = Stump.DofusProtocol.Messages.Message;

namespace Stump.Server.BaseServer.Network
{
    public class HandledMessage<T> : Message2<T, Message>
        where T : BaseClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public HandledMessage(Action<T, Message> callback, T client, Message message)
            : base (client, message, callback)
        {
            
        }

        public override void Execute()
        {
            try
            {
                var sw = Stopwatch.StartNew();

                base.Execute();
                sw.Stop();

                if (BenchmarkManager.Enable)
                    BenchmarkManager.Instance.RegisterEntry(BenchmarkEntry.Create(sw.Elapsed, Parameter2));
            }
            catch (Exception ex)
            {
                logger.Error("[Handler : {0}] Force disconnection of client {1} : {2}", Parameter2, Parameter1, ex);
                Parameter1.Disconnect();
                ExceptionManager.Instance.RegisterException(ex);
            }
        }
    }
}
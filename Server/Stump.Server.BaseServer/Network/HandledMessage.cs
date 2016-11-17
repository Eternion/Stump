using System;
using System.Diagnostics;
using NLog;
using Stump.Core.Threading;
using Stump.Server.BaseServer.Benchmark;
using Stump.Server.BaseServer.Exceptions;
using Message = Stump.DofusProtocol.Messages.Message;

namespace Stump.Server.BaseServer.Network
{
    public class HandledMessage<T> : Message3<object, T, Message>
        where T : IClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public HandledMessage(Action<object, T, Message> callback, T client, Message message)
            : base (null, client, message, callback)
        {
            
        }

        public override void Execute()
        {
            try
            {
                base.Execute();
            }
            catch (Exception ex)
            {
                logger.Error("[Handler : {0}] Force disconnection of client {1} : {2}", Parameter3, Parameter2, ex);
                Parameter2.Disconnect();
                ExceptionManager.RegisterException(ex);
            }
        }

        public override string ToString()
        {
            return Parameter3.ToString();
        }
    }
}
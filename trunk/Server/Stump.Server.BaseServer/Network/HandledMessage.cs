using System;
using NLog;
using Stump.Core.Threading;
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
                base.Execute();
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
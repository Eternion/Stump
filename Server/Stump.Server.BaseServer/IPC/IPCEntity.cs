using System;
using System.Collections.Generic;
using NLog;
using Stump.Core.Timers;
using Stump.Server.BaseServer.IPC.Messages;

namespace Stump.Server.BaseServer.IPC
{
    public delegate void RequestCallbackDelegate<in T>(T callbackMessage) where T : IPCMessage;

    public delegate void RequestCallbackErrorDelegate(IIPCErrorMessage errorMessage);

    public delegate void RequestCallbackDefaultDelegate(IPCMessage unattemptMessage);

    public abstract class IPCEntity
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public delegate void IPCMessageHandler(IPCMessage message);


        private readonly Dictionary<Guid, IIPCRequest> m_requests = new Dictionary<Guid, IIPCRequest>();
        protected abstract int RequestTimeout
        {
            get;
        }

        protected IDictionary<Guid, IIPCRequest> Requests
        {
            get { return m_requests; }
        }

        public abstract void Send(IPCMessage message);

        protected abstract TimedTimerEntry RegisterTimer(Action action, int timeout);

        protected abstract void ProcessRequest(IPCMessage request);
        protected abstract void ProcessAnswer(IIPCRequest request, IPCMessage answer);

        protected virtual void ProcessMessage(IPCMessage message)
        {
            if (message.RequestGuid == Guid.Empty)
                ProcessRequest(message);
            else
            {
                var request = TryGetRequest(message.RequestGuid);
                if (request != null)
                {
                    ProcessAnswer(request, message);
                    lock (m_requests)
                        m_requests.Remove(message.RequestGuid);
                }
                else
                    ProcessRequest(message);
            }
        }

        public void ReplyRequest(IPCMessage message, IPCMessage request)
        {
            if (request != null)
                message.RequestGuid = request.RequestGuid;

            Send(message);
        }

         public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback, RequestCallbackDefaultDelegate defaultCallback,
            int timeout) where T : IPCMessage
        {
            var guid = Guid.NewGuid();
            message.RequestGuid = guid;

            IPCRequest<T> request = null;
             if (timeout > 0)
             {
                 var timer = RegisterTimer(delegate { RequestTimedOut(request); }, timeout);
                 request = new IPCRequest<T>(message, guid, callback, errorCallback, defaultCallback, timer);
             }
             else
                 request = new IPCRequest<T>(message, guid, callback, errorCallback, defaultCallback, null);

             lock (m_requests)
                m_requests.Add(guid, request);

            Send(message);
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback,
            int timeout) where T : IPCMessage
        {
            SendRequest(message, callback, errorCallback, DefaultRequestUnexpectedCallback, timeout);
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback,
            RequestCallbackDefaultDelegate defaultCallback) where T : IPCMessage
        {
            SendRequest(message, callback, errorCallback, defaultCallback, RequestTimeout * 1000);
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback) 
            where T : IPCMessage
        {
            SendRequest(message, callback, errorCallback, RequestTimeout * 1000);
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback)
            where T : IPCMessage
        {
            SendRequest(message, callback, DefaultRequestErrorCallback, RequestTimeout * 1000);
        }

        public void SendRequest(IPCMessage message, RequestCallbackDelegate<CommonOKMessage> callback, RequestCallbackErrorDelegate errorCallback,
    int timeout)
        {
            SendRequest(message, callback, errorCallback, DefaultRequestUnexpectedCallback, timeout);
        }

        public void SendRequest(IPCMessage message, RequestCallbackDelegate<CommonOKMessage> callback, RequestCallbackErrorDelegate errorCallback,
            RequestCallbackDefaultDelegate defaultCallback)
        {
            SendRequest(message, callback, errorCallback, defaultCallback, RequestTimeout * 1000);
        }

        public void SendRequest(IPCMessage message, RequestCallbackDelegate<CommonOKMessage> callback, RequestCallbackErrorDelegate errorCallback)
        {
            SendRequest<CommonOKMessage>(message, callback, errorCallback, RequestTimeout * 1000);
        }

        public void SendRequest(IPCMessage message, RequestCallbackDelegate<CommonOKMessage> callback)
        {
            SendRequest<CommonOKMessage>(message, callback, DefaultRequestErrorCallback, RequestTimeout * 1000);
        }

        private IIPCRequest TryGetRequest(Guid guid)
        {
            if (guid == Guid.Empty)
                return null;

            IIPCRequest request;
            lock(m_requests)
                m_requests.TryGetValue(guid, out request);
            return request;
        }

        private void RequestTimedOut(IIPCRequest request)
        {
            request.TimedOut = true;
            request.ProcessMessage(new IPCErrorTimeoutMessage(string.Format("Request {0} timed out", request.RequestMessage.GetType())));
        }

        private void DefaultRequestErrorCallback(IIPCErrorMessage errorMessage)
        {
            var request = TryGetRequest(errorMessage.RequestGuid);
            logger.Error("Error received of type {0}. Request {1} Message : {2} StackTrace : {3}",
                errorMessage.GetType(), request.RequestMessage.GetType(), errorMessage.Message, errorMessage.StackTrace);
        }

        private void DefaultRequestUnexpectedCallback(IPCMessage message)
        {
            logger.Error("Unexpected message {0}. Request {1}", message.GetType(), TryGetRequest(message.RequestGuid).RequestMessage.GetType());
        }

    }
}
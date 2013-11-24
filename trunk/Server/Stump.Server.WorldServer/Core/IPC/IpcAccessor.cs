#region License GNU GPL
// IPCAccessor.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;

namespace Stump.Server.WorldServer.Core.IPC
{
    public class IPCAccessor : Singleton<IPCAccessor>
    {
        /// <summary>
        /// In seconds
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static int DefaultRequestTimeout = 5;

        /// <summary>
        /// In milliseconds
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static int TaskPoolInterval = 150;

        /// <summary>
        /// In milliseconds
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static int UpdateInterval = 10000;

        [Variable]
        public static string RemoteHost = "localhost";

        [Variable]
        public static int RemotePort = 9100;

        public delegate void IPCMessageHandler(IPCMessage message);

        public delegate void RequestCallbackDelegate<in T>(T callbackMessage) where T : IPCMessage;
        public delegate void RequestCallbackErrorDelegate(IPCErrorMessage errorMessage);
        public delegate void RequestCallbackDefaultDelegate(IPCMessage unattemptMessage);

        public event Action<IPCAccessor, IPCMessage> MessageReceived;
        public event Action<IPCAccessor, IPCMessage> MessageSent;
        public event Action<IPCAccessor> Connected;
        public event Action<IPCAccessor> Disconnected;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<Type, IPCMessageHandler> m_additionalsHandlers = new Dictionary<Type, IPCMessageHandler>(); 

        private bool m_requestingAccess;
        private bool m_wasConnected;
        private Dictionary<Guid, IIPCRequest> m_requests = new Dictionary<Guid, IIPCRequest>();
        private TimerEntry m_updateTimer;

        private void OnMessageReceived(IPCMessage message)
        {
            var handler = MessageReceived;
            if (handler != null)
                handler(this, message);
        }

        private void OnMessageSended(IPCMessage message)
        {
            var handler = MessageSent;
            if (handler != null)
                handler(this, message);
        }

        private void OnClientConnected()
        {
            logger.Info("IPC connection etablished");

            var handler = Connected;
            if (handler != null)
                handler(this);
        }

        private void OnClientDisconnected()
        {
            m_wasConnected = false;
            logger.Info("IPC connection lost");

            var handler = Disconnected;
            if (handler != null)
                handler(this);
        }

        public IPCAccessor()
        {
            TaskPool = new SelfRunningTaskPool(TaskPoolInterval, "IPCAccessor Task Pool");
            m_updateTimer = new TimerEntry(0, UpdateInterval, Tick);
        }

        public bool Running
        {
            get;
            set;
        }

        public SelfRunningTaskPool TaskPool
        {
            get;
            private set;
        }

        public Socket Socket
        {
            get;
            private set;
        }

        public bool AccessGranted
        {
            get;
            private set;
        }

        public bool IsReacheable
        {
            get
            {
                return Socket != null && Socket.Connected;
            }
        }

        public bool IsConnected
        {
            get { return IsReacheable && AccessGranted; }
        }

        public void Start()
        {
            if (Running)
                return;

            Running = true;
            TaskPool.Start();

            m_updateTimer.Start();
            TaskPool.AddTimer(m_updateTimer);
        }

        public void Stop()
        {
            if (!Running)
                return;

            Running = false;
            TaskPool.RemoveTimer(m_updateTimer);
            TaskPool.Stop();

            if (IsReacheable)
                Disconnect();
        }

        private void Connect()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(RemoteHost, RemotePort);
            OnClientConnected();

            ReceiveLoop();
        }

        private void OnAccessGranted(CommonOKMessage msg)
        {
            m_requestingAccess = false;
            AccessGranted = true;
            logger.Info("Access to auth. server granted");
        }

        private void OnAccessDenied(IPCErrorMessage error)
        {
            m_requestingAccess = false;

            if (error is IPCErrorTimeoutMessage)
                return;

            AccessGranted = false;
            logger.Error("Access to auth. server denied ! Reason : {0}", error.Message);
            WorldServer.Instance.Shutdown();
        }

        private void Disconnect()
        {
            try
            {
                Close();
            }
            finally 
            {
                OnClientDisconnected();
            }
        }

        private void Tick(int dt)
        {
            if (!Running)
            {
                if (IsReacheable)
                    Disconnect();
                return;
            }

            if (!IsReacheable)
            {
                if (m_requestingAccess)
                    return;

                if (m_wasConnected)
                    Disconnect();

                logger.Info("Attempt connection");
                try
                {
                    Connect();
                }
                catch (Exception ex)
                {
                    logger.Error("Connection to {0}:{1} failed. Try again in {2}s", RemoteHost, RemotePort, UpdateInterval / 1000);
                    return;
                }

                m_requestingAccess = true;
                m_wasConnected = true;
                SendRequest<CommonOKMessage>(new HandshakeMessage(WorldServer.ServerInformation), OnAccessGranted, OnAccessDenied);
            }
            else
            {
                // update server
                Send(new ServerUpdateMessage(WorldServer.Instance.ClientManager.Count));
            }
        }


        public void Send(IPCMessage message)
        {
            if (!IsReacheable)
            {
                return;
            }

            var args = new SocketAsyncEventArgs();
            args.Completed += OnSendCompleted;
            var data = IPCMessageSerializer.Instance.Serialize(message);

            // serialize stuff

            args.SetBuffer(data, 0, data.Length);
            Socket.SendAsync(args);
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Dispose();
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback, RequestCallbackDefaultDelegate defaultCallback,
            int timeout) where T : IPCMessage
        {
            var guid = Guid.NewGuid();
            message.RequestGuid = guid;

            var timer = new TimerEntry();
            var request = new IPCRequest<T>(message, guid, callback, errorCallback, defaultCallback, timer);
            timer.Action = delegate { RequestTimedOut(request); };
            timer.InitialDelay = timeout;
            TaskPool.AddTimer(timer);

            lock (m_requests)
                m_requests.Add(guid, request);

            Send(message);
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback,
            int timeout) where T : IPCMessage
        {
            SendRequest(message, callback, errorCallback, DefaultRequestUnattemptCallback, timeout);
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback,
            RequestCallbackDefaultDelegate defaultCallback) where T : IPCMessage
        {
            SendRequest(message, callback, errorCallback, defaultCallback, DefaultRequestTimeout * 1000);
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback) 
            where T : IPCMessage
        {
            SendRequest(message, callback, errorCallback, DefaultRequestTimeout * 1000);
        }

        public void SendRequest<T>(IPCMessage message, RequestCallbackDelegate<T> callback)
            where T : IPCMessage
        {
            SendRequest(message, callback, DefaultRequestErrorCallback, DefaultRequestTimeout);
        }

        public void SendRequest(IPCMessage message, RequestCallbackDelegate<CommonOKMessage> callback, RequestCallbackErrorDelegate errorCallback,
    int timeout)
        {
            SendRequest(message, callback, errorCallback, DefaultRequestUnattemptCallback, timeout);
        }

        public void SendRequest(IPCMessage message, RequestCallbackDelegate<CommonOKMessage> callback, RequestCallbackErrorDelegate errorCallback,
            RequestCallbackDefaultDelegate defaultCallback)
        {
            SendRequest(message, callback, errorCallback, defaultCallback, DefaultRequestTimeout * 1000);
        }

        public void SendRequest(IPCMessage message, RequestCallbackDelegate<CommonOKMessage> callback, RequestCallbackErrorDelegate errorCallback)
        {
            SendRequest<CommonOKMessage>(message, callback, errorCallback, DefaultRequestTimeout * 1000);
        }

        public void SendRequest(IPCMessage message, RequestCallbackDelegate<CommonOKMessage> callback)
        {
            SendRequest<CommonOKMessage>(message, callback, DefaultRequestErrorCallback, DefaultRequestTimeout);
        }

        private IIPCRequest TryGetRequest(Guid guid)
        {
            if (guid == Guid.Empty)
                return null;

            IIPCRequest request;
            m_requests.TryGetValue(guid, out request);
            return request;
        }

        private void RequestTimedOut(IIPCRequest request)
        {
            request.ProcessMessage(new IPCErrorTimeoutMessage(string.Format("Request {0} timed out", request.RequestMessage.GetType())));
        }

        private void DefaultRequestErrorCallback(IPCErrorMessage errorMessage)
        {
            var request = TryGetRequest(errorMessage.RequestGuid);
            logger.Error("Error received of type {0}. Request {1} Message : {2} StackTrace : {3}",
                errorMessage.GetType(), request.RequestMessage.GetType(), errorMessage.Message, errorMessage.StackTrace);
        }

        private void DefaultRequestUnattemptCallback(IPCMessage message)
        {
            logger.Error("Unattempt message {0}. Request {1}", message.GetType(), TryGetRequest(message.RequestGuid).RequestMessage.GetType());
        }

        private void ReceiveLoop()
        {
            if (!IsReacheable)
                return;

            var args = new SocketAsyncEventArgs();
            args.Completed += OnReceiveCompleted;
            args.SetBuffer(new byte[8192], 0, 8192);

            if (!Socket.ReceiveAsync(args))
            {
                ProcessReceiveCompleted(args);
            }
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceiveCompleted(args);
                    break;
                case SocketAsyncOperation.Disconnect:
                    Disconnect();
                    break;
            }
        }

        private void ProcessReceiveCompleted(SocketAsyncEventArgs args)
        {
            if (!IsReacheable)
                return;

            if (args.BytesTransferred <= 0 ||
                args.SocketError != SocketError.Success)
            {
                args.Dispose();
                Disconnect();
            }
            else
            {
                Receive(args.Buffer, args.Offset, args.BytesTransferred);
                
                args.Dispose();
                ReceiveLoop();
            }
        }

        private void Receive(byte[] data, int offset, int count)
        {
            IPCMessage message;

            try
            {
                message = IPCMessageSerializer.Instance.Deserialize(data, offset, count);
            }
            catch (Exception ex)
            {
                logger.Error("Cannot deserialize received message ! Exception : {0}" + ex);
                return;
            }

            TaskPool.AddMessage(() =>
                {
                    if (message.RequestGuid != Guid.Empty)
                    {
                        IIPCRequest request;
                        lock (m_requests)
                        {
                            if (!(m_requests.TryGetValue(message.RequestGuid, out request)))
                            {
                                logger.Error(
                                    "Received request guid with message {0} but no is request bound to this guid ({1})",
                                    message.GetType(), message.RequestGuid);
                                return;
                            }
                            m_requests.Remove(request.Guid);
                            if (request.TimeoutTimer != null)
                            TaskPool.RemoveTimer(request.TimeoutTimer);
                        }

                        request.ProcessMessage(message);
                    }
                    else
                    {
                        HandleMessage(message);
                    }
                });
        }

        public void AddMessageHandler(Type messageType, IPCMessageHandler handler)
        {
            m_additionalsHandlers.Add(messageType, handler);
        }

        private void HandleMessage(IPCMessage message)
        {
            if (message is IPCErrorMessage)
                HandleError(message as IPCErrorMessage);
            if (message is DisconnectClientMessage)
                HandleMessage(message as DisconnectClientMessage);

            if (m_additionalsHandlers.ContainsKey(message.GetType()))
                m_additionalsHandlers[message.GetType()](message);
        }

        private void HandleMessage(DisconnectClientMessage message)
        {
            WorldServer.Instance.DisconnectClient(message.AccountId);
        }

        private void HandleError(IPCErrorMessage error)
        {
            logger.Error("Error received of type {0}. Message : {1} StackTrace : {2}",
                error.GetType(), error.Message, error.StackTrace);
        }

        private void Close()
        {
            if (Socket != null && Socket.Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Socket = null;
            }
        }
    }
}
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
using System.IO;
using System.Linq;
using System.Net.Sockets;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Core.IPC
{
    public class IPCAccessor : IPCEntity
    {
        public delegate void IPCMessageHandler(IPCMessage message);

        public delegate void RequestCallbackDefaultDelegate(IPCMessage unattemptMessage);

        public delegate void RequestCallbackDelegate<in T>(T callbackMessage) where T : IPCMessage;

        public delegate void RequestCallbackErrorDelegate(IPCErrorMessage errorMessage);

        /// <summary>
        ///     In seconds
        /// </summary>
        [Variable(DefinableRunning = true)] public static int DefaultRequestTimeout = 5;

        /// <summary>
        ///     In milliseconds
        /// </summary>
        [Variable(DefinableRunning = true)] public static int TaskPoolInterval = 150;

        /// <summary>
        ///     In milliseconds
        /// </summary>
        [Variable(DefinableRunning = true)] public static int UpdateInterval = 10000;

        [Variable] public static string RemoteHost = "localhost";

        [Variable] public static int RemotePort = 9100;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static IPCAccessor m_instance;

        private readonly Dictionary<Type, IPCMessageHandler> m_additionalsHandlers =
            new Dictionary<Type, IPCMessageHandler>();

        private readonly TimerEntry m_updateTimer;
        private IPCMessagePart m_messagePart;
        private bool m_requestingAccess;
        private Dictionary<Guid, IIPCRequest> m_requests = new Dictionary<Guid, IIPCRequest>();
        private bool m_wasConnected;

        public IPCAccessor()
        {
            TaskPool = new SelfRunningTaskPool(TaskPoolInterval, "IPCAccessor Task Pool");
            m_updateTimer = new TimerEntry(0, UpdateInterval, Tick);
        }

        public static IPCAccessor Instance
        {
            get { return m_instance ?? (m_instance = new IPCAccessor()); }
            private set { m_instance = value; }
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
            get { return Socket != null && Socket.Connected; }
        }

        public bool IsConnected
        {
            get { return IsReacheable && AccessGranted; }
        }

        protected override int RequestTimeout
        {
            get { return DefaultRequestTimeout; }
        }

        public event Action<IPCAccessor, IPCMessage> MessageReceived;
        public event Action<IPCAccessor, IPCMessage> MessageSent;
        public event Action<IPCAccessor> Connected;
        public event Action<IPCAccessor> Disconnected;
        public event Action<IPCAccessor> Granted;

        private void OnMessageReceived(IPCMessage message)
        {
            Action<IPCAccessor, IPCMessage> handler = MessageReceived;
            if (handler != null)
                handler(this, message);
        }

        private void OnMessageSended(IPCMessage message)
        {
            Action<IPCAccessor, IPCMessage> handler = MessageSent;
            if (handler != null)
                handler(this, message);
        }

        private void OnClientConnected()
        {
            logger.Info("IPC connection etablished");

            Action<IPCAccessor> handler = Connected;
            if (handler != null)
                handler(this);
        }

        private void OnClientDisconnected()
        {
            m_wasConnected = false;
            logger.Info("IPC connection lost");

            Action<IPCAccessor> handler = Disconnected;
            if (handler != null)
                handler(this);
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

            Action<IPCAccessor> handler = Granted;
            if (handler != null)
                handler(this);
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
                    logger.Error("Connection to {0}:{1} failed. Try again in {2}s", RemoteHost, RemotePort,
                        UpdateInterval/1000);
                    return;
                }

                m_requestingAccess = true;
                m_wasConnected = true;
                SendRequest<CommonOKMessage>(new HandshakeMessage(WorldServer.ServerInformation), OnAccessGranted,
                    OnAccessDenied);
            }
            else
                // update server
                Send(new ServerUpdateMessage(WorldServer.Instance.ClientManager.Count));
        }


        public override void Send(IPCMessage message)
        {
            if (!IsReacheable)
                return;

            var args = new SocketAsyncEventArgs();
            args.Completed += OnSendCompleted;
            byte[] data = IPCMessageSerializer.Instance.SerializeWithLength(message);

            // serialize stuff

            args.SetBuffer(data, 0, data.Length);
            Socket.SendAsync(args);
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Dispose();
        }

        protected override TimerEntry RegisterTimer(Action<int> action, int timeout)
        {
            var timer = new TimerEntry {Action = action, InitialDelay = timeout};
            TaskPool.AddTimer(timer);

            return timer;
        }

        private void ReceiveLoop()
        {
            if (!IsReacheable)
                return;

            var args = new SocketAsyncEventArgs();
            args.Completed += OnReceiveCompleted;
            args.SetBuffer(new byte[8192], 0, 8192);

            if (!Socket.ReceiveAsync(args))
                ProcessReceiveCompleted(args);
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
            var reader = new BinaryReader(new MemoryStream(data, offset, count));

            while (reader.BaseStream.Length - reader.BaseStream.Position > 0)
            {
                if (m_messagePart == null)
                    m_messagePart = new IPCMessagePart();

                m_messagePart.Build(reader, reader.BaseStream.Length - reader.BaseStream.Position);

                if (!m_messagePart.IsValid)
                    return;

                IPCMessage message;

                try
                {
                    message = IPCMessageSerializer.Instance.Deserialize(m_messagePart.Data);
                }
                catch (Exception ex)
                {
                    logger.Error("Cannot deserialize received message ! Exception : {0}" + ex);
                    return;
                }
                finally
                {
                    m_messagePart = null;
                }

                TaskPool.AddMessage(() => ProcessMessage(message));
            }
        }

        protected override void ProcessAnswer(IIPCRequest request, IPCMessage answer)
        {
            request.TimeoutTimer.Stop();
            TaskPool.RemoveTimer(request.TimeoutTimer);
            request.ProcessMessage(answer);
        }

        protected override void ProcessRequest(IPCMessage request)
        {
            if (request is IPCErrorMessage)
                HandleError(request as IPCErrorMessage);
            if (request is DisconnectClientMessage)
                HandleMessage(request as DisconnectClientMessage);

            if (m_additionalsHandlers.ContainsKey(request.GetType()))
                m_additionalsHandlers[request.GetType()](request);
        }

        public void AddMessageHandler(Type messageType, IPCMessageHandler handler)
        {
            m_additionalsHandlers.Add(messageType, handler);
        }

        private void HandleMessage(DisconnectClientMessage message)
        {
            var clients = WorldServer.Instance.FindClients(client => client.Account != null && client.Account.Id == message.AccountId);

            if (clients.Length > 1)
                logger.Error("Several clients connected on the same account ({0}). Disconnect them all", message.AccountId);

            bool isLogged = false;
            for (int index = 0; index < clients.Length; index++)
            {
                var client = clients[index];
                isLogged = client.Character != null;
                // dirty but whatever
                if (isLogged && index == 0) 
                    client.Character.Saved += chr => OnCharacterSaved(message);

                client.Disconnect();
            }

            if (!isLogged)
                ReplyRequest(new DisconnectedClientMessage(clients.Any()), message);
        }

        private void OnCharacterSaved(DisconnectClientMessage request)
        {
            ReplyRequest(new DisconnectedClientMessage(true), request);
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
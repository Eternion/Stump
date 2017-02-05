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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.Core.Pool;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Core.IPC
{
    public class IPCAccessor : IPCEntity
    {
        public delegate void IPCMessageHandler(IPCMessage message);

        public delegate void RequestCallbackDefaultDelegate(IPCMessage unattemptMessage);

        public delegate void RequestCallbackDelegate<in T>(T callbackMessage) where T : IPCMessage;

        public delegate void RequestCallbackErrorDelegate(IIPCErrorMessage errorMessage);

        /// <summary>
        ///     In seconds
        /// </summary>
        [Variable(DefinableRunning = true)] public static int DefaultRequestTimeout = 60;

        /// <summary>
        ///     In milliseconds
        /// </summary>
        [Variable(DefinableRunning = true)] public static int TaskPoolInterval = 150;

        /// <summary>
        ///     In milliseconds
        /// </summary>
        [Variable(DefinableRunning = true)] public static int UpdateInterval = 10000;

        [Variable] public static int BufferSize = 8192;

        [Variable] public static string RemoteHost = "localhost";

        [Variable] public static int RemotePort = 9100;

        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static IPCAccessor m_instance;

        readonly Dictionary<Type, IPCMessageHandler> m_additionalsHandlers =
            new Dictionary<Type, IPCMessageHandler>();

        readonly TimedTimerEntry m_updateTimer;
        bool m_requestingAccess;
        bool m_wasConnected;

        BufferSegment m_bufferSegment;
        IPCMessagePart m_messagePart;
        int m_writeOffset;
        int m_readOffset;
        private int m_remainingLength;
        private SocketAsyncEventArgs m_readArgs;

        public IPCAccessor()
        {
            TaskPool = new SelfRunningTaskPool(TaskPoolInterval, "IPCAccessor Task Pool");
            m_updateTimer = TaskPool.CallPeriodically(UpdateInterval, Tick);
            m_readArgs = new SocketAsyncEventArgs();
            m_bufferSegment = BufferManager.GetSegment(BufferSize);
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

        public bool IsReacheable => Socket != null && Socket.Connected;

        public bool IsConnected => IsReacheable && AccessGranted;

        protected override int RequestTimeout => DefaultRequestTimeout;

        public event Action<IPCAccessor, IPCMessage> MessageReceived;
        public event Action<IPCAccessor, IPCMessage> MessageSent;
        public event Action<IPCAccessor> Connected;
        public event Action<IPCAccessor> Disconnected;
        public event Action<IPCAccessor> Granted;

        private void OnMessageReceived(IPCMessage message)
        {
            MessageReceived?.Invoke(this, message);
        }

        private void OnMessageSended(IPCMessage message)
        {
            MessageSent?.Invoke(this, message);
        }

        private void OnClientConnected()
        {
            logger.Info("IPC connection etablished");

            Connected?.Invoke(this);
        }

        private void OnClientDisconnected()
        {
            m_wasConnected = false;
            logger.Info("IPC connection lost");

            foreach (var request in Requests.Values)
            {
                request.Cancel("world server - disconnected");
            }

            Disconnected?.Invoke(this);
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

            if (IsReacheable)
                Disconnect();

            TaskPool.Stop();
        }

        private void Connect()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(RemoteHost, RemotePort);
            OnClientConnected();

            BeginReceive();
        }

        private void OnAccessGranted(CommonOKMessage msg)
        {
            m_requestingAccess = false;
            AccessGranted = true;

            logger.Info("Access to auth. server granted");
#if DEBUG
            Console.Beep(444, 200);
#endif
            Granted?.Invoke(this);
        }

        private void OnAccessDenied(IIPCErrorMessage error)
        {
            m_requestingAccess = false;

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
                TaskPool.ExecuteInContext(() => OnClientDisconnected());
            }
        }

        private void Tick()
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
            else if (AccessGranted)
                // update server
                Send(new ServerUpdateMessage(WorldServer.Instance.ClientManager.Count));
        }


        public override void Send(IPCMessage message)
        {
            if (!IsReacheable)
                return;

            var args = new SocketAsyncEventArgs();
            args.Completed += OnSendCompleted;
            var stream = new MemoryStream();
            IPCMessageSerializer.Instance.SerializeWithLength(message, stream);

            // serialize stuff
            var data = stream.ToArray();
            args.SetBuffer(data, 0, data.Length);
            Socket.SendAsync(args);
        }

        private static void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Dispose();
        }

        protected override TimedTimerEntry RegisterTimer(Action action, int timeout)
        {
            return TaskPool.CallDelayed(timeout, action);
        }

        public void BeginReceive()
        {
            ResumeReceive();
        }

        private void ResumeReceive()
        {
            if (Socket == null || !Socket.Connected)
                return;

            m_readArgs.SetBuffer(m_bufferSegment.Buffer.Array, m_bufferSegment.Offset + m_writeOffset, m_bufferSegment.Length - m_writeOffset);
            m_readArgs.Completed += ProcessReceive;

            var willRaiseEvent = Socket.ReceiveAsync(m_readArgs);
            if (!willRaiseEvent)
            {
                ProcessReceive(this, m_readArgs);
            }
        }

        private void ProcessReceive(object sender, SocketAsyncEventArgs e)
        {
            m_readArgs.Completed -= ProcessReceive;
            if (e.BytesTransferred <= 0 || e.SocketError != SocketError.Success)
            {
                Disconnect();
                return;
            }

            m_remainingLength += e.BytesTransferred;
            try
            {
                if (BuildMessage(m_bufferSegment))
                {
                    m_writeOffset = m_readOffset = 0;
					if (m_bufferSegment.Length != BufferSize)
					{
						m_bufferSegment.DecrementUsage();
						m_bufferSegment = BufferManager.GetSegment(BufferSize);
					}
                }

                ResumeReceive();

            }
            catch (Exception ex)
            {
                logger.Error("Forced disconnection during reception : " + ex);

                Disconnect();
            }
        }

        protected virtual bool BuildMessage(BufferSegment buffer)
        {
            if (m_messagePart == null)
                m_messagePart = new IPCMessagePart();

            var reader = new FastBigEndianReader(buffer)
            {
                Position = buffer.Offset + m_readOffset,
                MaxPosition = buffer.Offset + m_readOffset + m_remainingLength,
            };

            bool built;
            try
            {
                built = m_messagePart.Build(reader);
            }
            catch
            {
                logger.Error("Cannot build message. Length={0} LengthSize={3} RemainingLength={1} Data={2}", m_messagePart.Length, m_remainingLength, m_messagePart.Data, m_messagePart.LengthBytesCount);
                throw;
            }

            // if message is complete
            if (built)
            {
                var dataPos = reader.Position;
                // prevent to read above
                reader.MaxPosition = dataPos + m_messagePart.Length.Value;

                IPCMessage message;
                try
                {
                    message = IPCMessageSerializer.Instance.Deserialize(m_messagePart.Data);
                }
                catch (Exception ex)
                {
                    reader.Seek(dataPos, SeekOrigin.Begin);
                    logger.Debug("Message = {0}", m_messagePart.Data.ToString(" "));
                    logger.Error("Error while deserializing IPC Message : " + ex);

                   return m_remainingLength <= 0 || BuildMessage(buffer);
                }

                TaskPool.AddMessage(() => ProcessMessage(message));

                m_remainingLength -= (int)(reader.Position - (buffer.Offset + m_readOffset));
                m_writeOffset = m_readOffset = (int)reader.Position - buffer.Offset;
                m_messagePart = null;

                return m_remainingLength <= 0 || BuildMessage(buffer);
            }

            m_remainingLength -= (int)(reader.Position - (buffer.Offset + m_readOffset));
            m_readOffset = (int)reader.Position - buffer.Offset;
            m_writeOffset = m_readOffset + m_remainingLength;

            EnsureBuffer(m_messagePart.Length.HasValue ? m_messagePart.Length.Value : 5);

            return false;
        }

        /// <summary>
        ///     Makes sure the underlying buffer is big enough
        /// </summary>
        protected bool EnsureBuffer(int length)
        {
            if (m_bufferSegment.Length - m_writeOffset < length + m_remainingLength)
            {
                var newSegment = BufferManager.GetSegment(Math.Max(length + m_remainingLength, BufferSize), true);

                if (newSegment is TemporaryBufferSegment)
                    logger.Warn("Extra big segment required ({0})", length + m_remainingLength);

                Array.Copy(m_bufferSegment.Buffer.Array,
                           m_bufferSegment.Offset + m_readOffset,
                           newSegment.Buffer.Array,
                           newSegment.Offset,
                           m_remainingLength);

                m_bufferSegment.DecrementUsage();
                m_bufferSegment = newSegment;
                m_writeOffset = m_remainingLength;
                m_readOffset = 0;

                return true;
            }

            return false;
        }


        protected override void ProcessAnswer(IIPCRequest request, IPCMessage answer)
        {
            if (request.TimedOut)
            {
                logger.Warn("Message {0} already timed out, message ignored", request.RequestMessage.GetType());
                return;
            }

            request.ProcessMessage(answer);
        }

        protected override void ProcessRequest(IPCMessage request)
        {
            if (request is IIPCErrorMessage)
                HandleError(request as IIPCErrorMessage);
            if (request is DisconnectClientMessage)
                HandleMessage(request as DisconnectClientMessage);

            if (m_additionalsHandlers.ContainsKey(request.GetType()))
                m_additionalsHandlers[request.GetType()](request);
            else if (!(request is IIPCErrorMessage) && !(request is DisconnectClientMessage) &&
                !(request is CommonOKMessage))
            {
                logger.Warn("IPC Message {0} not handled", request);
            }
        }

        public void AddMessageHandler(Type messageType, IPCMessageHandler handler)
        {
            m_additionalsHandlers.Add(messageType, handler);
        }

        private void HandleMessage(DisconnectClientMessage message)
        {
            var clients = WorldServer.Instance.FindClients(client => client.Account != null && client.Account.Id == message.AccountId).ToArray();

            if (clients.Length > 1)
                logger.Error("Several clients connected on the same account ({0}). Disconnect them all", message.AccountId);

            var isLogged = false;
            for (var index = 0; index < clients.Length; index++)
            {
                var client = clients[index];
                isLogged = client.Character != null;
                // dirty but whatever
                if (isLogged && index == 0)
                {
                    Action<Character> ev = null;
                    ev = chr =>
                    {
                        client.Character.Saved -= ev;
                        OnCharacterSaved(message);
                    };
                    client.Character.Saved += ev;
                }
                client.Disconnect();
            }

            if (!isLogged && AccountManager.Instance.IsAccountBlocked(message.AccountId, out Character character))
            {
                logger.Warn("Account {0} blocked, waiting release", message.AccountId);
                Action<Character> ev = null;
                ev = chr =>
                {
                    character.AccountUnblocked -= ev;
                    ReplyRequest(new DisconnectedClientMessage(true), message);
                };
                character.AccountUnblocked += ev;
            }
            else if (!isLogged)
                ReplyRequest(new DisconnectedClientMessage(clients.Any()), message);

        }

        private void OnCharacterSaved(DisconnectClientMessage request)
        {
            ReplyRequest(new DisconnectedClientMessage(true), request);
        }

        private static void HandleError(IIPCErrorMessage error)
        {
            logger.Error("Error received of type {0}. Message : {1} StackTrace : {2}",
                error.GetType(), error.Message, error.StackTrace);
        }

        private void Close()
        {
            if (Socket == null || !Socket.Connected)
                return;

            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();

            Socket = null;
        }
    }
}
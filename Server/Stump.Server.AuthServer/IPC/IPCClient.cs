#region License GNU GPL
// IPCClient.cs
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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.Core.Pool;
using Stump.Core.Timers;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Core.Threading;

namespace Stump.Server.AuthServer.IPC
{
    public class IPCClient : IPCEntity
    {
        [Variable(DefinableRunning = true)]
        public static int DefaultRequestTimeout = -1;
        //public static int DefaultRequestTimeout = 60;

        /// <summary>
        ///     In milliseconds
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static int TaskPoolInterval = 150;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private BufferSegment m_bufferSegment;
        private IPCMessagePart m_messagePart;
        private int m_writeOffset;
        private int m_readOffset;
        private int m_remainingLength;

        public IPCClient(Socket socket)
        {
            Socket = socket;
            TaskPool = new SelfRunningTaskPool(TaskPoolInterval, "IPCClient Task Pool");
            m_readArgs = new SocketAsyncEventArgs(); 
            m_readArgs.UserToken = this;
            m_bufferSegment = BufferManager.GetSegment(IPCHost.BufferSize);
        }

        private IPCOperations m_operations;
        private SocketAsyncEventArgs m_readArgs;
        public event Action<IPCClient> Disconnected;

        public WorldServer Server
        {
            get;
            private set;
        }

        #region Network Stuff
        public Socket Socket
        {
            get;
            private set;
        }

        public SelfRunningTaskPool TaskPool
        {
            get;
            private set;
        }

        public int Port
        {
            get { return ((IPEndPoint) Socket.RemoteEndPoint).Port; }
        }

        public IPAddress Address
        {
            get { return ((IPEndPoint) Socket.RemoteEndPoint).Address; }
        }

        public bool IsConnected
        {
            get { return Socket != null && Socket.Connected; }
        }

        public DateTime LastActivity
        {
            get;
            set;
        }

        protected override int RequestTimeout
        {
            get { return DefaultRequestTimeout; }
        }

        protected override TimedTimerEntry RegisterTimer(Action action, int timeout)
        {
            return TaskPool.CallDelayed(timeout, action);
        }

        public override void Send(IPCMessage message)
        {
            if (!IsConnected)
            {
                return;
            }

            var args = new SocketAsyncEventArgs();
            args.Completed += OnSendCompleted;
            var stream = new MemoryStream();
            IPCMessageSerializer.Instance.SerializeWithLength(message, stream);

            var data = stream.ToArray();
            args.SetBuffer(data, 0, data.Length);
            Socket.SendAsync(args);

            // is it necessarily ?
            LastActivity = DateTime.Now;}

        private static void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Dispose();
        }

        public void BeginReceive()
        {
            TaskPool.Start();

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
					if (m_bufferSegment.Length != IPCHost.BufferSize)
					{
						m_bufferSegment.DecrementUsage();
						m_bufferSegment = BufferManager.GetSegment(IPCHost.BufferSize);
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
            // if message is complete
            if (m_messagePart.Build(reader))
            {
                var dataPos = reader.Position;
                // prevent to read above
                reader.MaxPosition = dataPos + m_messagePart.Length.Value;

                IPCMessage message;
                try
                {
                    message = IPCMessageSerializer.Instance.Deserialize(m_messagePart.Data);
                }
                catch (Exception)
                {
                    reader.Seek(dataPos, SeekOrigin.Begin);
                    logger.Debug("Message = {0}", m_messagePart.Data.ToString(" "));
                    throw;
                }

                LastActivity = DateTime.Now;

                TaskPool.AddMessage(() => ProcessMessage(message));

                m_remainingLength -= (int)(reader.Position - (buffer.Offset + m_readOffset));
                m_writeOffset = m_readOffset = (int)reader.Position - buffer.Offset;
                m_messagePart = null;

                return m_remainingLength <= 0 || BuildMessage(buffer);
            }

            m_remainingLength -= (int)(reader.Position - (buffer.Offset + m_readOffset));
            m_readOffset = (int)reader.Position - buffer.Offset;
            m_writeOffset = m_readOffset + m_remainingLength;

            EnsureBuffer(m_messagePart.Length.HasValue ? m_messagePart.Length.Value : 3);

            return false;
        }

        /// <summary>
        ///     Makes sure the underlying buffer is big enough
        /// </summary>
        protected bool EnsureBuffer(int length)
        {
            if (m_bufferSegment.Length - m_writeOffset < length + m_remainingLength)
            {
                var newSegment = BufferManager.GetSegment(length + m_remainingLength, true);

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

        protected override void ProcessMessage(IPCMessage message)
        {
            // handshake not done yet
            if (m_operations == null)
            {
                if (!( message is HandshakeMessage ))
                {
                    SendError(string.Format("The first received packet should be a HandshakeMessage not {0}", message.GetType()), message);
                    Disconnect();
                }
                else
                {
                    // the handshake is managed by the IO thread, the other messages by an other DB connection
                    AuthServer.Instance.IOTaskPool.AddMessage(() =>
                    {
                        var handshake = message as HandshakeMessage;
                        WorldServer server;
                        try
                        {
                            server = WorldServerManager.Instance.RequestConnection(this, handshake.World);
                        }
                        catch (Exception ex)
                        {
                            SendError(ex, message);
                            Disconnect();
                            return;
                        }

                        Server = server;
                        m_operations = new IPCOperations(this);
                        // guid setted manually cause the request is not stored
                        ReplyRequest(new CommonOKMessage(), message);
                    });
                }
            }
            else
            {
                base.ProcessMessage(message);

            }
        }

        protected override void ProcessAnswer(IIPCRequest request, IPCMessage answer)
        {
            if (request.TimedOut)
            {
                logger.Warn("Message {0} already timed out, message ignored", request.RequestMessage.GetType());
                return;
            }

            if (request.TimeoutTimer != null)
            {
                request.TimeoutTimer.Stop();
                TaskPool.RemoveTimer(request.TimeoutTimer);
            }

            request.ProcessMessage(answer);
        }

        protected override void ProcessRequest(IPCMessage request)
        {
            try
            {
                m_operations.HandleMessage(request);
            }
            catch (Exception ex)
            {
                SendError(ex, request);
            }
        }

        public void SendError(Exception exception, IPCMessage request)
        {
            logger.Error("IPC error : {0}", exception);
            ReplyRequest(new IPCErrorMessage(exception.Message, exception.StackTrace), request);
        }

        public void SendError(string error, IPCMessage request)
        {
            logger.Error("IPC error : {0}", error);
            ReplyRequest(new IPCErrorMessage(error), request);
        }

        public void Disconnect()
        {
            if (Socket != null && Socket.Connected)
                Socket.Close();

            if (Server != null)
               WorldServerManager.Instance.RemoveWorld(Server);

            if (m_operations != null)
                m_operations.Dispose();

            Server = null;
            m_operations = null;

            TaskPool.ExecuteInContext(() => OnDisconnected());
        }

        protected void OnDisconnected()
        {
            foreach (var request in Requests.Values)
            {
                request.Cancel("auth server - disconnected");
            }

            TaskPool.Stop();

            var evnt = Disconnected;
            if (evnt != null)
                evnt(this);
        }

        #endregion

    }
}
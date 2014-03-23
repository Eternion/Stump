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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Pool;
using Stump.Core.Timers;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;

namespace Stump.Server.AuthServer.IPC
{
    public class IPCClient : IPCEntity
    {
        [Variable(DefinableRunning = true)]
        public static int DefaultRequestTimeout = 5;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly object m_recvLock = new object();
        private bool m_recvLockAcquired;
        private IPCMessagePart m_messagePart;

        public IPCClient(Socket socket)
        {
            Socket = socket;
        }

        private IPCOperations m_operations;

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

        protected override TimerEntry RegisterTimer(Action<int> action, int timeout)
        {
            var timer = new TimerEntry() {Action = action, InitialDelay = timeout};
            AuthServer.Instance.IOTaskPool.AddTimer(timer);

            return timer;
        }

        public override void Send(IPCMessage message)
        {
         if (!IsConnected)
                return;

            var args = new SocketAsyncEventArgs();
            var stream = BufferManager.Default.CheckOutStream();
            args.Completed += OnSendCompleted;
            IPCMessageSerializer.Instance.SerializeWithLength(message, stream);

            // serialize stuff

            args.SetBuffer(stream.Segment.Buffer.Array, stream.Segment.Offset, (int) (stream.Position));
            args.UserToken = stream;
            if (Socket.SendAsync(args))
            {
                stream.Segment.DecrementUsage();
                args.Dispose();
            }
            // is it necessarily ?
            LastActivity = DateTime.Now;}

        private static void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            ((SegmentStream)e.UserToken).Segment.DecrementUsage();
            e.Dispose();
        }

        internal void ProcessReceive(byte[] data, int offset, int count)
        {
            var reader = new BinaryReader(new MemoryStream(data, offset, count));

            while (reader.BaseStream.Length - reader.BaseStream.Position > 0)
            {
                try
                {
                    // stuff
                    LastActivity = DateTime.Now;

                    if (m_messagePart == null)
                        m_messagePart = new IPCMessagePart();

                    m_messagePart.Build(reader, reader.BaseStream.Length - reader.BaseStream.Position);

                    if (!m_messagePart.IsValid)
                        return;

                    var message = IPCMessageSerializer.Instance.Deserialize(m_messagePart.Data);

                    if (m_recvLockAcquired)
                    {
                        logger.Error("Recv lock should not be set 'cause it's mono thread !");
                    }

                    Monitor.Enter(m_recvLock, ref m_recvLockAcquired);
                    try
                    {
                        ProcessMessage(message);
                    }
                    finally
                    {
                        Monitor.Exit(m_recvLock);
                        m_recvLockAcquired = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Forced disconnection during reception : " + ex);

                    Disconnect();
                }
                finally
                {
                    m_messagePart = null;
                }
            }
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
            request.TimeoutTimer.Stop();
            AuthServer.Instance.IOTaskPool.RemoveTimer(request.TimeoutTimer);
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
            if (Server != null)
                WorldServerManager.Instance.RemoveWorld(Server);

            if (m_operations != null)
                m_operations.Dispose();

            Server = null;
            m_operations = null;
        }

        #endregion
    }
}
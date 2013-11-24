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
using ProtoBuf;
using ProtoBuf.Meta;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;

namespace Stump.Server.AuthServer.IPC
{
    public class IPCClient
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private object m_recvLock = new object();
        private bool m_recvLockAcquired = false;
        private IPCMessage m_currentRequest;

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

        public bool Connected
        {
            get { return Socket != null && Socket.Connected; }
        }

        public DateTime LastActivity
        {
            get;
            set;
        }

        public void ReplyRequest(IPCMessage message, IPCMessage request)
        {
            if (!Connected)
            {
                return;
            }

            if (request != null)
                message.RequestGuid = request.RequestGuid;

            Send(message);
        }

        public void Send(IPCMessage message)
        {
            if (!Connected)
            {
                return;
            }

            var args = new SocketAsyncEventArgs();
            args.Completed += OnSendCompleted;
            var data = IPCMessageSerializer.Instance.Serialize(message);

            args.SetBuffer(data, 0, data.Length);
            Socket.SendAsync(args);

            // is it necessarily ?
            LastActivity = DateTime.Now;
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Dispose();
        }

        internal void ProcessReceive(byte[] data, int offset, int count)
        {
            try
            {
                // stuff
                LastActivity = DateTime.Now;
                var message = IPCMessageSerializer.Instance.Deserialize(data, offset, count);

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
        }

        private void ProcessMessage(IPCMessage message)
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
                }
            }
            else
            {
                if (message.RequestGuid != Guid.Empty)
                    m_currentRequest = message;
                else
                    m_currentRequest = null;

                try
                {
                    m_operations.HandleMessage(message);
                }
                catch (Exception ex)
                {
                    SendError(ex, message);
                }
            }
        }

        public void SendError(Exception exception, IPCMessage request)
        {
            ReplyRequest(new IPCErrorMessage(exception.Message, exception.StackTrace), request);
        }

        public void SendError(string error, IPCMessage request)
        {
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
#region License GNU GPL
// SoundManager.cs
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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Uplauncher.Helpers;

namespace Uplauncher
{
    public class SoundProxy
    {
        private Socket m_clientListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                                    ProtocolType.Tcp);
        private Socket m_regListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                                    ProtocolType.Tcp);

        private Socket m_regClient;
        private List<Socket> m_clients = new List<Socket>();

        public bool Started
        {
            get;
            private set;
        }

        public void StartProxy()
        {
            if (Started)
                return;

            ClientPort = NetworkHelper.FindFreePort(50000, 51000);
            m_clientListener.Bind(new IPEndPoint(IPAddress.Loopback, ClientPort));
            m_clientListener.Listen(8);

            RegPort = NetworkHelper.FindFreePort(ClientPort + 1, 51000);
            m_regListener.Bind(new IPEndPoint(IPAddress.Loopback, RegPort));
            m_regListener.Listen(1);

            var args = new SocketAsyncEventArgs();
            args.Completed += (sender, e) => OnClientConnected(e);
            if (!m_clientListener.AcceptAsync(args))
            {
                OnClientConnected(args);
            }

            var argsReg = new SocketAsyncEventArgs();
            argsReg.Completed += (sender, e) => OnRegConnected(e);
            if (!m_regListener.AcceptAsync(argsReg))
            {
                OnRegConnected(argsReg);
            }

            Started = true;
        }

        private void OnClientConnected(SocketAsyncEventArgs e)
        {
            m_clients.Add(e.AcceptSocket);
            var args = new SocketAsyncEventArgs();
            args.Completed += (sender, arg) => OnClientReceived(arg);
            args.SetBuffer(new byte[8192], 0, 8192);
            args.UserToken = e.AcceptSocket;

            if (!e.AcceptSocket.ReceiveAsync(args))
                OnClientReceived(args);

            var listenArgs = new SocketAsyncEventArgs();
            listenArgs.Completed += (sender, x) => OnClientConnected(x);
            if (!m_clientListener.AcceptAsync(listenArgs))
            {
                OnClientConnected(listenArgs);
            }
        }

        private void RemoveClient(Socket socket)
        {
            m_clients.Remove(socket);
        }

        private void OnClientReceived(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
            {
                ((Socket)e.UserToken).Disconnect(false);
                RemoveClient((Socket)e.UserToken);
            }
            else
            {
                if (m_regClient == null || !m_regClient.Connected)
                {
                    ( (Socket)e.UserToken ).Disconnect(false);
                    RemoveClient((Socket)e.UserToken);
                }
                else
                {
                    m_regClient.Send(e.Buffer, e.Offset, e.BytesTransferred, SocketFlags.None);

                    if (!( (Socket)e.UserToken ).ReceiveAsync(e))
                        OnClientReceived(e);
                }
            }
        }
        

        private void OnRegConnected(SocketAsyncEventArgs e)
        {
            if (m_regClient == null || !m_regClient.Connected)
                m_regClient = e.AcceptSocket;

            var listenArgs = new SocketAsyncEventArgs();
            listenArgs.Completed += (sender, x) => OnRegConnected(x);
            if (!m_regListener.AcceptAsync(listenArgs))
            {
                OnRegConnected(listenArgs);
            }
        }

        public int RegPort
        {
            get;
            set;
        }

        public int ClientPort
        {
            get;
            set;
        }
    }
}
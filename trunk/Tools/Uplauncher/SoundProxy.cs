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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        private List<SoundClient> m_clients = new List<SoundClient>();
        private SoundClient m_mainClient;

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
            var client = new SoundClient(e.AcceptSocket);
            m_clients.Add(client);

            var args = new SocketAsyncEventArgs();
            args.Completed += (sender, arg) => OnClientReceived(arg);
            args.SetBuffer(new byte[8192], 0, 8192);
            args.UserToken = client;

            if (!e.AcceptSocket.ReceiveAsync(args))
                OnClientReceived(args);

            var listenArgs = new SocketAsyncEventArgs();
            listenArgs.Completed += (sender, x) => OnClientConnected(x);
            if (!m_clientListener.AcceptAsync(listenArgs))
            {
                OnClientConnected(listenArgs);
            }
        }

        private void RemoveClient(SoundClient client)
        {
            m_clients.Remove(client);
        }

        private void OnClientReceived(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
            {
                ( (SoundClient)e.UserToken ).Socket.Disconnect(false);
                RemoveClient((SoundClient)e.UserToken);
            }
            else
            {
                if (m_regClient == null || !m_regClient.Connected)
                {
                    ( (SoundClient)e.UserToken ).Socket.Disconnect(false);
                    RemoveClient((SoundClient)e.UserToken);
                }
                else
                {
                    var message = Encoding.ASCII.GetString(e.Buffer, e.Offset, e.BytesTransferred);

                    if (( (SoundClient)e.UserToken ).ID == 0 && message.Contains("sayHello"))
                    {
                        // example: 1366402807812=>sayHello(1366402807812,C:\Users\Bouh2\Desktop\Dofus\Dofus 2.10\app/config.xml)|
                        var index = message.IndexOf("sayHello") + ("sayHello").Length;
                        var idStr = message.Substring(index + 1, message.IndexOf(",", index) - (index + 1));

                        ( (SoundClient)e.UserToken ).ID = long.Parse(idStr);
                    }

                    Debug.WriteLine("CLIENT : " + message);
                    m_regClient.Send(e.Buffer, e.Offset, e.BytesTransferred, SocketFlags.None);

                    if (!( (SoundClient)e.UserToken ).Socket.ReceiveAsync(e))
                        OnClientReceived(e);
                }
            }
        }
        

        private void OnRegConnected(SocketAsyncEventArgs e)
        {
            if (m_regClient == null || !m_regClient.Connected)
            {
                m_regClient = e.AcceptSocket;

                var args = new SocketAsyncEventArgs();
                args.Completed += (sender, arg) => OnRegReceived(arg);
                args.SetBuffer(new byte[8192], 0, 8192);
                args.UserToken = e.AcceptSocket;

                if (!e.AcceptSocket.ReceiveAsync(args))
                    OnRegReceived(args);
            }

            var listenArgs = new SocketAsyncEventArgs();
            listenArgs.Completed += (sender, x) => OnRegConnected(x);
            if (!m_regListener.AcceptAsync(listenArgs))
            {
                OnRegConnected(listenArgs);
            }
        }

        private void OnRegReceived(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
            {
                ( (Socket)e.UserToken ).Disconnect(false);
            }
            else
            {
                var message = Encoding.ASCII.GetString(e.Buffer, e.Offset, e.BytesTransferred);

                Debug.WriteLine("REG : " + message);

                // example : main_client_is():1366402807812|
                if (message.Contains("main_client_is():"))
                {
                    var index = message.IndexOf("main_client_is():") + ( "main_client_is():" ).Length;
                    var idStr = message.Substring(index, message.IndexOf("|", index) - index);

                    var id = long.Parse(idStr);

                    m_mainClient = m_clients.First(x => x.ID == id);
                }

                foreach (var soundClient in m_clients)
                {
                    soundClient.Socket.Send(e.Buffer, e.Offset, e.BytesTransferred, SocketFlags.None);
                }

                if (!( (Socket)e.UserToken ).ReceiveAsync(e))
                    OnRegReceived(e);
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
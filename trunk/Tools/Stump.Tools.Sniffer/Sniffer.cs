// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using PcapDotNet.Analysis;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.DofusProtocol;
using Stump.DofusProtocol.Messages;
using Message = Stump.DofusProtocol.Messages.Message;
using Stump.BaseCore.Framework.Attributes;

namespace Stump.Tools.Sniffer
{
    public class Sniffer
    {
        /// <summary>
        /// Local ip pattern for the sniffer
        /// </summary>
        [Variable]
        public static string LocalIpPattern = "192.168.";

        /// <summary>
        /// local port for to sniff
        /// </summary>
        [Variable]
        public static int PortToSniff = 5555;

        private const string ConfigPath = "./sniffer_config.xml";
        private const string SchemaPath = "./sniffer_config.xsd";

        private readonly FormMain m_form;
        private readonly Dictionary<string, Assembly> m_loadedAssemblies;

        private readonly PacketDevice m_selectedDevice;

        private IdentifiedClient m_player = new IdentifiedClient("Player");
        private bool m_running;
        private IdentifiedClient m_server = new IdentifiedClient("Server");
        private Thread m_thread;



        /// <summary>
        ///   Initializes a new instance of the <see cref = "Sniffer" /> class.
        /// </summary>
        public Sniffer(FormMain form)
        {
            m_form = form;
            m_loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);

            ConfigFile = new XmlConfigFile(ConfigPath, SchemaPath);
            ConfigFile.DefinesVariables(ref m_loadedAssemblies);

            PcapDotNetAnalysis.OptIn = false;

            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                MessageBox.Show("No interfaces found! Make sure WinPcap is installed.");
                return;
            }

            foreach (LivePacketDevice device in allDevices)
            {
                if (
                    device.Addresses.Any(
                        adress =>
                        (adress.Address.Family.ToString() == "Internet") &&
                        adress.Address.ToString().Contains(LocalIpPattern)))
                {
                    m_selectedDevice = device;
                }
            }

            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();
            IdentifiedClient.OnNewMessage += IdentifiedClient_OnNewMessage;
        }

        public XmlConfigFile ConfigFile
        {
            get;
            private set;
        }

        public bool Running
        {
            get { return m_running; }
        }

        private System.Diagnostics.Stopwatch time  = new System.Diagnostics.Stopwatch();
        private void IdentifiedClient_OnNewMessage(Message message, string sender)
        {

            m_form.AddMessageToListView(message, sender);
        }

        /// <summary>
        ///   Starts this instance.
        /// </summary>
        public void Start()
        {
            m_thread = new Thread(DStart) {IsBackground = true};
            m_thread.Start();
            m_running = true;
        }

        /// <summary>
        ///   Resets this instance.
        /// </summary>
        public void Reset()
        {
            m_server = new IdentifiedClient("Serveur");
            m_player = new IdentifiedClient("Player");
        }

        /// <summary>
        ///   Stops this instance.
        /// </summary>
        public void Stop()
        {
            m_thread.Abort();
            m_running = false;
        }

        private void DStart()
        {
            if (m_selectedDevice == null)
                return;
            using (
                PacketCommunicator communicator = m_selectedDevice.Open(65536,
                                                                        PacketDeviceOpenAttributes.MaximumResponsiveness,
                                                                        1000))
            {
                communicator.SetFilter(communicator.CreateFilter("tcp and port " + PortToSniff));
                communicator.ReceivePackets(0, PacketHandler);
            }
        }

        /// <summary>
        ///   Handle the Packet
        /// </summary>
        /// <param name = "packet"></param>
        private void PacketHandler(Packet packet)
        {
            IpV4Datagram datagram = packet.Ethernet.IpV4;
            String ipSource = datagram.Source.ToString();
            MemoryStream stream = datagram.Tcp.Payload.ToMemoryStream();

            byte[] data = stream.ToArray();

            if (data.Length == 0)
                return;

            m_form.LbByteNumber.Text = (int.Parse(m_form.LbByteNumber.Text) + data.Length).ToString();
            m_form.LbPacketNumber.Text = (int.Parse(m_form.LbPacketNumber.Text) + 1).ToString();

            if (ipSource.Contains(LocalIpPattern))
            {
                m_player.ProcessReceive(data, 0, data.Length);
            }
            else
            {
                m_server.ProcessReceive(data, 0, data.Length);
            }
        }
    }
}
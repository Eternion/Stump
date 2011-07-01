
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using PcapDotNet.Analysis;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using Stump.Core.Attributes;
using Stump.Core.Xml;
using Stump.Core.Xml.Config;
using Stump.DofusProtocol;
using Stump.DofusProtocol.Messages;
using Message = Stump.DofusProtocol.Messages.Message;

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

        /// <summary>
        /// write all message in a log file
        /// </summary>
        [Variable]
        public static bool Logs = true;


        private const string ConfigPath = "./sniffer_config.xml";
        private const string SchemaPath = "./sniffer_config.xsd";

        private readonly FormMain m_form;
        private readonly Dictionary<string, Assembly> m_loadedAssemblies;

        private XmlWriter m_writer;

        private PacketDevice m_selectedDevice;

        private IdentifiedClient m_player = new IdentifiedClient("Player");
        private bool m_running;
        private IdentifiedClient m_server = new IdentifiedClient("Server");
        private Thread m_thread;

        private bool m_initialized;



        /// <summary>
        ///   Initializes a new instance of the <see cref = "Sniffer" /> class.
        /// </summary>
        public Sniffer(FormMain form)
        {
            m_form = form;
            m_loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);

            Config = new XmlConfig(ConfigPath, SchemaPath);
            Config.DefinesVariables(ref m_loadedAssemblies);

            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();

            if (Logs) InitLogs();
        }

        public XmlConfig Config
        {
            get;
            private set;
        }

        public bool Running
        {
            get { return m_running; }
        }

        private void IdentifiedClient_OnNewMessage(Message message, string sender)
        {
            m_form.AddMessageToListView(message, sender);

            if (Logs)
            {
                var tv = new TreeView();
                Parser.ToTreeView(tv, message);
                Parser.TreeNodeToXml(tv.Nodes[0], m_writer);
                //m_writer.Flush();
            }
        }

        private void InitLogs()
        {
            m_writer = XmlWriter.Create("logs.xml",new XmlWriterSettings { NewLineOnAttributes = true, Indent = true});
            m_writer.WriteStartDocument();
            m_writer.WriteStartElement("Messages");
        }

        public void StopLogs()
        {
            m_writer.WriteEndElement();
            m_writer.Close();
        }

        /// <summary>
        ///   Starts this instance.
        /// </summary>
        public bool Start()
        {
            if (!m_initialized)
            {
                PcapDotNetAnalysis.OptIn = false;

                IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

                if (allDevices.Count == 0)
                {
                    MessageBox.Show("No interfaces found! Make sure WinPcap is installed.");
                    return false;
                }

                Func<LivePacketDevice, string> deviceInfoProvider =
                    entry => entry.Description + " ( " + string.Join("  ", entry.Addresses.Where(a => !a.Address.ToString().Contains("Internet6")).Select(a => a.Address)) + " ) ";

                var dialog = new DialogInterfaceSelect
                                 {
                                     Interfaces = allDevices.Select(deviceInfoProvider).ToArray()
                                 };
                if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedInterface != null)
                {
                    m_selectedDevice = allDevices.Where(entry => deviceInfoProvider(entry) == dialog.SelectedInterface).First();

                    IdentifiedClient.OnNewMessage += IdentifiedClient_OnNewMessage;

                }
                else
                    return false;

                m_initialized = true;
            }

            m_thread = new Thread(StartSniffing) { IsBackground = true };
            m_thread.Start();
            m_running = true;

            return true;
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

        private void StartSniffing()
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                m_writer.Flush();
            }
        }
    }
}
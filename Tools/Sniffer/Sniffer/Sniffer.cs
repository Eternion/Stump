using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using PcapDotNet.Analysis;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using Sniffer.Modules;
using Stump.DofusProtocol.Messages;
using System.Diagnostics;
using Stump.DofusProtocol.Types;
using Message = Stump.DofusProtocol.Messages.Message;

namespace Stump.Tools.Sniffer
{
    public class Sniffer
    {
        /// <summary>
        /// local port for to sniff
        /// </summary>
        public static int DefaultPortToSniff = 5555;

        /// <summary>
        /// Folder containing modules
        /// </summary>
        public static string ModulesFolder = "./modules";

        private readonly FormMain m_form;

        private readonly ModuleProvider<PacketHandlerModule> _moduleProvider = new ModuleProvider<PacketHandlerModule>(ModulesFolder);

        private PacketDevice m_selectedDevice;
        private PacketCommunicator m_communicator;

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

            m_form.cBSendTo.DataSource = new List<IdentifiedClient> { m_player, m_server };
            m_form.cBSendTo.DisplayMember = "Name";

            //m_form.cbMessageType.DataSource = MessageReceiver.GetMessages().ToList();

            _moduleProvider.LoadModules();
            AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);

            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();

            Port = DefaultPortToSniff;
            form.toolStripTextBoxPort.Text = Port.ToString();
            form.toolStripTextBoxPort.TextChanged += (sender, e) =>
            {
                int port;
                if (int.TryParse(form.toolStripTextBoxPort.Text, out port))
                    Port = port;
                else
                    form.toolStripTextBoxPort.Text = Port.ToString();
            };
        }

        public void OnClose()
        {
            _moduleProvider.RemoveAllModules();
        }

        public int Port
        {
            get;
            set;
        }

        public bool Running
        {
            get { return m_running; }
        }

        private void IdentifiedClient_OnNewMessage(Message message, string sender)
        {
            Debug.Print("Message {0} - sender {1}", message, sender);

            _moduleProvider.Dispatch(m => m.Handle(message, sender));
         
            m_form.AddMessageToListView(message, sender);
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
                    MessageBox.Show(@"No interfaces found! Make sure WinPcap is installed.");
                    return false;
                }

                var dialog = new DialogInterfaceSelect
                                 {
                                     Interfaces = allDevices.Select(entry => entry.Name + ":" + (entry.Description ?? "")).ToArray()
                                 };
                if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedInterface != null)
                {
                    m_selectedDevice = allDevices.First(entry => entry.Name + ":" + (entry.Description ?? "") == dialog.SelectedInterface);

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
            m_initialized = false;
            IdentifiedClient.OnNewMessage -= IdentifiedClient_OnNewMessage;
        }

        private void StartSniffing()
        {
            if (m_selectedDevice == null)
                return;
            using (
                 m_communicator = m_selectedDevice.Open(65536,
                                                                        PacketDeviceOpenAttributes.MaximumResponsiveness,
                                                                        1000))
            {
                m_communicator.SetFilter(m_communicator.CreateFilter("tcp and port " + Port));
                m_communicator.ReceivePackets(0, PacketHandler);
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

            var sender = datagram.Tcp.SourcePort == Port ? m_server : m_player;
            var dest = sender == m_player ? m_server : m_player;

            var ethernetLayer = packet.Ethernet.ExtractLayer() as EthernetLayer;
            var ipv4Layer = packet.Ethernet.IpV4.ExtractLayer() as IpV4Layer;
            var tcpLayer = packet.Ethernet.IpV4.Tcp.ExtractLayer() as TcpLayer;
            var payloadLayer = packet.Ethernet.IpV4.Tcp.Payload.ExtractLayer() as PayloadLayer;

            //set mac
            sender.Mac = ethernetLayer.Source;
            dest.Mac = ethernetLayer.Destination;

            //set ip
            sender.Ip = ipv4Layer.Source;
            dest.Ip = ipv4Layer.Destination;

            //set ipv4 id
            sender.Ipv4Id = ipv4Layer.Identification;

            //set port
            sender.Port = tcpLayer.SourcePort;
            dest.Port = tcpLayer.DestinationPort;

            //set tcp Id
            sender.TcpId = (uint)(tcpLayer.SequenceNumber + payloadLayer.Length);
            sender.AckId = tcpLayer.AcknowledgmentNumber;

            //set AckId
            dest.AckId = sender.TcpId;

            //Process
            sender.ProcessReceive(data, 0, data.Length);

            if (sender.Sended.Contains(tcpLayer.SequenceNumber))
            {
                ConfirmReception(dest, sender);
                dest.Sended.Remove(tcpLayer.SequenceNumber);
            }
        }

        public void Send(IdentifiedClient dest, Message message)
        {
            /*message = new CharacterNameSuggestionRequestMessage();
            var src = dest == m_player ? m_server : m_player;

            using (var writer = new BigEndianStream.BigEndianWriter())
            {
                // We need to create all the packet layers.

                // LinkLayer -> Ethernet
                var ethernetLayer = new EthernetLayer()
                                        {
                                            Source = src.Mac,
                                            Destination = dest.Mac
                                        };

                // InternetLayer -> IPv4
                src.Ipv4Id += 2;
                var ipv4Layer = new IpV4Layer()
                                    {
                                        Source = src.Ip,
                                        Destination = dest.Ip,
                                        Ttl = 128,
                                        Identification = src.Ipv4Id
                                    };

                // TransportLayer -> TCP
                var tcpLayer = new TcpLayer()
                                   {
                                       SourcePort = src.Port,
                                       DestinationPort = dest.Port,
                                       SequenceNumber = src.TcpId,
                                       AcknowledgmentNumber = src.AckId,
                                       ControlBits = TcpControlBits.Push | TcpControlBits.Acknowledgment,
                                       Window = 4140
                                   };

                //Write Message in Buffer
                message.Pack(writer);

                // PayloadLayer
                var payloadLayer = new PayloadLayer()
                                       {
                                           Data = new Datagram(writer.Data)
                                       };

                // Build Packet
                var builder = new PacketBuilder(ethernetLayer, ipv4Layer, tcpLayer, payloadLayer);
                var packet = builder.Build(DateTime.Now);

                //Add to Sended
                dest.Sended.Add(src.AckId);

                m_communicator.SendPacket(packet);
            }*/
        }

        private void ConfirmReception(IdentifiedClient src, IdentifiedClient dest)
        {
            // LinkLayer -> Ethernet
            var ethernetLayer = new EthernetLayer()
            {
                Source = src.Mac,
                Destination = dest.Mac
            };

            // InternetLayer -> IPv4
            src.Ipv4Id += 2;
            var ipv4Layer = new IpV4Layer()
            {
                Source = src.Ip,
                Destination = dest.Ip,
                Ttl = 128,
                Identification = src.Ipv4Id
            };

            // TransportLayer -> TCP
            var tcpLayer = new TcpLayer()
            {
                SourcePort = src.Port,
                DestinationPort = dest.Port,
                SequenceNumber = src.TcpId,
                AcknowledgmentNumber = src.AckId,
                ControlBits = TcpControlBits.Acknowledgment,
                Window = 4140
            };

            // Build Packet
            var builder = new PacketBuilder(ethernetLayer, ipv4Layer, tcpLayer);
            var packet = builder.Build(DateTime.Now);
           
            m_communicator.SendPacket(packet);
        }
    }
}
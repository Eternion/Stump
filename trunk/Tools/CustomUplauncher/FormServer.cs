using System;
using System.Windows.Forms;

namespace CustomUplauncher
{
    public partial class FormServer : Form
    {
        private ServerInformations m_serverInformations;

        public FormServer()
        {
            InitializeComponent();
            ServerInformations = new ServerInformations();
        }

        public ServerInformations ServerInformations
        {
            get { return m_serverInformations; }
            set
            {
                m_serverInformations = value;
                RefreshInformationsValues();
            }
        }

        public void RefreshInformationsValues()
        {
            textBoxName.Text = ServerInformations.Name;
            textBoxIp.Text = ServerInformations.Ip;
            textBoxPorts.Text = ServerInformations.Ports;
            textBoxPatchUrl.Text = ServerInformations.PatchUrl;
            textBoxAddress.Text = ServerInformations.Address;
        }

        private void TextBoxNameTextChanged(object sender, EventArgs e)
        {
            ServerInformations.Name = textBoxName.Text;
        }

        private void TextBoxIpTextChanged(object sender, EventArgs e)
        {
            ServerInformations.Ip = textBoxIp.Text;
        }

        private void TextBoxPortsTextChanged(object sender, EventArgs e)
        {
            ServerInformations.Ports = textBoxPorts.Text;
        }

        private void TextBoxPatchUrlTextChanged(object sender, EventArgs e)
        {
            ServerInformations.PatchUrl = textBoxPatchUrl.Text;
        }

        private void TextBoxAddressTextChanged(object sender, EventArgs e)
        {
            ServerInformations.Address = textBoxAddress.Text;
        }
    }
}
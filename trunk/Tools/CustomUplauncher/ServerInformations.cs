using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CustomUplauncher
{
    public class ServerInformations
    {
        public ServerInformations()
        {
        }

        public ServerInformations(ServerInformations server)
            : this (server.Name, server.Ip, server.Ports, server.PatchUrl, server.Address)
        {
        }

        public ServerInformations(string name, string ip, string ports, string patchUrl, string address)
        {
            Name = name;
            Ip = ip;
            Ports = ports;
            PatchUrl = patchUrl;
            Address = address;
        }

        public string Name
        {
            get;
            set;
        }

        public string Ip
        {
            get;
            set;
        }

        public string Ports
        {
            get;
            set;
        }

        public string PatchUrl
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Concat(Name, " - ", Ip);
        }
    }
}
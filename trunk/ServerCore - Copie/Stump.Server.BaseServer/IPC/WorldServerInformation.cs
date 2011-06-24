
using System;
using System.Xml.Serialization;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.IPC
{
    [Serializable]
    public class WorldServerInformation
    {
        #region Properties

        /// <summary>
        ///   World address.
        /// </summary>
        public string Address;

        /// <summary>
        ///   Internally assigned unique Id of this World.
        /// </summary>
        public int Id;

        [XmlIgnore]
        public DateTime LastPing;

        [XmlIgnore]
        public DateTime LastUpdate;

        /// <summary>
        ///   World name.
        /// </summary>
        public string Name;

        public ushort Port;

        public string AddressString
        {
            get { return Address + ":" + Port; }
        }

        #endregion
    }
}
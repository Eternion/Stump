using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Stump.Server.BaseServer.IPC.Objects
{
    /// <summary>
    /// Reprensents a serialized WorldServer
    /// </summary>
    [Serializable]
    [DataContract]
    public class WorldServerData
    {
        #region Properties

        /// <summary>
        ///   Internally assigned unique Id of this World.
        /// </summary>
        [DataMember]
        public int Id;

        /// <summary>
        ///   World address.
        /// </summary>
        [DataMember]
        public string Address;

        [DataMember]
        public ushort Port;

        /// <summary>
        ///   World name.
        /// </summary>
        [DataMember]
        public string Name;


        public string AddressString
        {
            get { return Address + ":" + Port; }
        }

        #endregion
    }
}
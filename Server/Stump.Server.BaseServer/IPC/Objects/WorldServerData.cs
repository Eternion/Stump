using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ProtoBuf;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.IPC.Objects
{
    /// <summary>
    /// Reprensents a serialized WorldServer
    /// </summary>
    [ProtoContract]
    public class WorldServerData
    {
        #region Properties

        /// <summary>
        ///   Internally assigned unique Id of this World.
        /// </summary>
        [ProtoMember(1)]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        ///   World address.
        /// </summary>
        [ProtoMember(2)]
        public string Address
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public ushort Port
        {
            get;
            set;
        }

        /// <summary>
        ///   World name.
        /// </summary>
        [ProtoMember(4)]
        public string Name
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public int Capacity
        {
            get;
            set;
        }
        [ProtoMember(6)]
        public RoleEnum RequiredRole
        {
            get;
            set;
        }

        [ProtoMember(7)]
        public bool RequireSubscription
        {
            get;
            set;
        }

        public string AddressString
        {
            get { return Address + ":" + Port; }
        }

        #endregion
    }
}
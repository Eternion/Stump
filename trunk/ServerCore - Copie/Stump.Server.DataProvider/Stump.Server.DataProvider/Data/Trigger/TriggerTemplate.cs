
using System;
using ProtoBuf;

namespace Stump.Server.DataProvider.Data.Trigger
{
    [Serializable,ProtoContract]
    public class TriggerTemplate
    {
        [ProtoMember(1)]
        public uint MapId { get; set; }     

        [ProtoMember(2)]
        public ushort CellId { get; set; }

        [ProtoMember(3)]
        public ushort Action { get; set; }

        [ProtoMember(4)]
        public ushort Condition { get; set; }
    }
}
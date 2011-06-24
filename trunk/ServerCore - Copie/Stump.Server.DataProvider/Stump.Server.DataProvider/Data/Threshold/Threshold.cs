
using System;
using ProtoBuf;

namespace Stump.Server.DataProvider.Data.Threshold
{
    [Serializable,ProtoContract]
    public class Threshold
    {
        [ProtoMember(1)]
        public uint Level
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public long Value
        {
            get;
            set;
        }

        public Threshold()
        {
            
        }
    }
}
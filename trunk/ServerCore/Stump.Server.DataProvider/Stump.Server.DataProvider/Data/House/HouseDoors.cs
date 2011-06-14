
using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Stump.Server.DataProvider.Data.House
{
    [Serializable, ProtoContract]
    public class HouseDoors
    {
        [ProtoMember(1)]
        public int HouseId { get; set; }

        [ProtoMember(2)]
        public List<short> Doors { get; set; }

        public HouseDoors()
        {
        }
    }
}
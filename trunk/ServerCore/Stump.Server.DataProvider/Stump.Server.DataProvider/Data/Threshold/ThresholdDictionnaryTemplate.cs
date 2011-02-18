using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace Stump.Server.DataProvider.Data.Threshold
{
    [ProtoContract]
    public class ThresholdDictionnaryTemplate
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public List<Threshold> Thresholds { get; set; }
    }
}
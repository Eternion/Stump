
using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Stump.Server.DataProvider.Data.Threshold
{
    [Serializable,ProtoContract]
    public class ThresholdDictionnaryTemplate
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public List<Threshold> Thresholds { get; set; }

        public ThresholdDictionnaryTemplate()
        {          
        }
    }
}

using System;
using ProtoBuf;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.DataProvider.Data.Mount
{
    [Serializable,ProtoContract]
    public class MountTemplate
    {
        [ProtoMember(1)]
        public uint MountId { get; set; }

        [ProtoMember(2)]
        public string LookStr { get; set; }

        public EntityLook Look { get; set; }

        [ProtoMember(3)]
        public string PodFormula { get; set; }

        [ProtoMember(4)]
        public string EnergyFormula { get; set; }

        [ProtoMember(5)]
        public uint MaxMaturity { get; set; }

        [ProtoMember(6)]
        public string GestationDuration { get; set; }

        [ProtoMember(7)]
        public string LearningMalus { get; set; }

        public MountTemplate()
        {
        }
    }
}
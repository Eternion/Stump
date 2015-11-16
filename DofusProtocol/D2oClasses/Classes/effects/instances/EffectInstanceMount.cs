

// Generated on 11/16/2015 14:26:30
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceMount", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceMount : EffectInstance
    {
        public double date;
        public uint modelId;
        public uint mountId;
        [D2OIgnore]
        public double Date
        {
            get { return this.date; }
            set { this.date = value; }
        }
        [D2OIgnore]
        public uint ModelId
        {
            get { return this.modelId; }
            set { this.modelId = value; }
        }
        [D2OIgnore]
        public uint MountId
        {
            get { return this.mountId; }
            set { this.mountId = value; }
        }
    }
}
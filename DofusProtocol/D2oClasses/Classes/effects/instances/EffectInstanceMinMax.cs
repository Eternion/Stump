

// Generated on 04/19/2016 10:17:57
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceMinMax", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceMinMax : EffectInstance
    {
        public uint min;
        public uint max;
        [D2OIgnore]
        public uint Min
        {
            get { return this.min; }
            set { this.min = value; }
        }
        [D2OIgnore]
        public uint Max
        {
            get { return this.max; }
            set { this.max = value; }
        }
    }
}
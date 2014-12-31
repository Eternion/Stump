

// Generated on 12/29/2014 16:35:09
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceInteger", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceInteger : EffectInstance
    {
        public int value;
        [D2OIgnore]
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
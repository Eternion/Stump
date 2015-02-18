

// Generated on 02/18/2015 11:05:57
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceString", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceString : EffectInstance
    {
        public String text;
        [D2OIgnore]
        public String Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
    }
}
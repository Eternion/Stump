

// Generated on 08/13/2015 17:13:45
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
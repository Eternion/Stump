
// Generated on 03/25/2013 19:24:33
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceInteger")]
    [Serializable]
    public class EffectInstanceInteger : EffectInstance, IIndexedData
    {
        public int value;

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

    }
}
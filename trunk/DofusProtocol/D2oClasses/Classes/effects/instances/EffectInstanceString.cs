
// Generated on 03/25/2013 19:24:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceString")]
    [Serializable]
    public class EffectInstanceString : EffectInstance
    {
        public String text;

        public String Text
        {
            get { return text; }
            set { text = value; }
        }

    }
}

// Generated on 03/25/2013 19:24:33
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceDuration")]
    [Serializable]
    public class EffectInstanceDuration : EffectInstance, IIndexedData
    {
        public uint days;
        public uint hours;
        public uint minutes;

        public uint Days
        {
            get { return days; }
            set { days = value; }
        }

        public uint Hours
        {
            get { return hours; }
            set { hours = value; }
        }

        public uint Minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }

    }
}


// Generated on 10/28/2013 14:03:17
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceDuration", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceDuration : EffectInstance
    {
        public uint days;
        public uint hours;
        public uint minutes;
        [D2OIgnore]
        public uint Days
        {
            get { return days; }
            set { days = value; }
        }
        [D2OIgnore]
        public uint Hours
        {
            get { return hours; }
            set { hours = value; }
        }
        [D2OIgnore]
        public uint Minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }
    }
}
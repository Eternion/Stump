

// Generated on 02/18/2015 11:05:57
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
            get { return this.days; }
            set { this.days = value; }
        }
        [D2OIgnore]
        public uint Hours
        {
            get { return this.hours; }
            set { this.hours = value; }
        }
        [D2OIgnore]
        public uint Minutes
        {
            get { return this.minutes; }
            set { this.minutes = value; }
        }
    }
}
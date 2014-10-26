

// Generated on 10/26/2014 23:27:35
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceDate", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceDate : EffectInstance
    {
        public uint year;
        public uint month;
        public uint day;
        public uint hour;
        public uint minute;
        [D2OIgnore]
        public uint Year
        {
            get { return this.year; }
            set { this.year = value; }
        }
        [D2OIgnore]
        public uint Month
        {
            get { return this.month; }
            set { this.month = value; }
        }
        [D2OIgnore]
        public uint Day
        {
            get { return this.day; }
            set { this.day = value; }
        }
        [D2OIgnore]
        public uint Hour
        {
            get { return this.hour; }
            set { this.hour = value; }
        }
        [D2OIgnore]
        public uint Minute
        {
            get { return this.minute; }
            set { this.minute = value; }
        }
    }
}
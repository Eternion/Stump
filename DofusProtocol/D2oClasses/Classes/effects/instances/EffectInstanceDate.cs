

// Generated on 10/28/2013 14:03:17
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
            get { return year; }
            set { year = value; }
        }
        [D2OIgnore]
        public uint Month
        {
            get { return month; }
            set { month = value; }
        }
        [D2OIgnore]
        public uint Day
        {
            get { return day; }
            set { day = value; }
        }
        [D2OIgnore]
        public uint Hour
        {
            get { return hour; }
            set { hour = value; }
        }
        [D2OIgnore]
        public uint Minute
        {
            get { return minute; }
            set { minute = value; }
        }
    }
}
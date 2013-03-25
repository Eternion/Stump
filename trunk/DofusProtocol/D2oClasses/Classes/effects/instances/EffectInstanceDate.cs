
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceDate")]
    [Serializable]
    public class EffectInstanceDate : EffectInstance, IIndexedData
    {
        public uint year;
        public uint month;
        public uint day;
        public uint hour;
        public uint minute;

        public uint Year
        {
            get { return year; }
            set { year = value; }
        }

        public uint Month
        {
            get { return month; }
            set { month = value; }
        }

        public uint Day
        {
            get { return day; }
            set { day = value; }
        }

        public uint Hour
        {
            get { return hour; }
            set { hour = value; }
        }

        public uint Minute
        {
            get { return minute; }
            set { minute = value; }
        }

    }
}
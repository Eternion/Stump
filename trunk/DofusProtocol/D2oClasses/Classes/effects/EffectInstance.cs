
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstance")]
    [Serializable]
    public class EffectInstance : IDataObject, IIndexedData
    {
        public uint effectId;
        public int targetId;
        public int duration;
        public int delay;
        public int random;
        public int group;
        public int modificator;
        public Boolean trigger;
        public Boolean hidden;
        public uint zoneSize;
        public uint zoneShape;
        public uint zoneMinSize;
        public Boolean rawZoneInit;
        public String rawZone;

        int IIndexedData.Id
        {
            get { return (int)effectId; }
        }

        public uint EffectId
        {
            get { return effectId; }
            set { effectId = value; }
        }

        public int TargetId
        {
            get { return targetId; }
            set { targetId = value; }
        }

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public int Random
        {
            get { return random; }
            set { random = value; }
        }

        public int Group
        {
            get { return group; }
            set { group = value; }
        }

        public int Modificator
        {
            get { return modificator; }
            set { modificator = value; }
        }

        public Boolean Trigger
        {
            get { return trigger; }
            set { trigger = value; }
        }

        public Boolean Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }

        public uint ZoneSize
        {
            get { return zoneSize; }
            set { zoneSize = value; }
        }

        public uint ZoneShape
        {
            get { return zoneShape; }
            set { zoneShape = value; }
        }

        public uint ZoneMinSize
        {
            get { return zoneMinSize; }
            set { zoneMinSize = value; }
        }

        public Boolean RawZoneInit
        {
            get { return rawZoneInit; }
            set { rawZoneInit = value; }
        }

        public String RawZone
        {
            get { return rawZone; }
            set { rawZone = value; }
        }

    }
}


// Generated on 09/01/2015 11:16:31
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstance", "com.ankamagames.dofus.datacenter.effects")]
    [Serializable]
    public class EffectInstance : IDataObject, IIndexedData
    {
        public uint effectUid;
        public uint effectId;
        public int targetId;
        public String targetMask;
        public int duration;
        public int delay;
        public int random;
        public int group;
        public int modificator;
        public Boolean trigger;
        public String triggers;
        public Boolean visibleInTooltip = true;
        public Boolean visibleInBuffUi = true;
        public Boolean visibleInFightLog = true;
        public Object zoneSize;
        public uint zoneShape;
        public Object zoneMinSize;
        public Object zoneEfficiencyPercent;
        public Object zoneMaxEfficiency;
        public Object zoneStopAtTarget;
        public String rawZone;
        int IIndexedData.Id
        {
            get { return (int)effectUid; }
        }
        [D2OIgnore]
        public uint EffectUid
        {
            get { return this.effectUid; }
            set { this.effectUid = value; }
        }
        [D2OIgnore]
        public uint EffectId
        {
            get { return this.effectId; }
            set { this.effectId = value; }
        }
        [D2OIgnore]
        public int TargetId
        {
            get { return this.targetId; }
            set { this.targetId = value; }
        }
        [D2OIgnore]
        public String TargetMask
        {
            get { return this.targetMask; }
            set { this.targetMask = value; }
        }
        [D2OIgnore]
        public int Duration
        {
            get { return this.duration; }
            set { this.duration = value; }
        }
        [D2OIgnore]
        public int Delay
        {
            get { return this.delay; }
            set { this.delay = value; }
        }
        [D2OIgnore]
        public int Random
        {
            get { return this.random; }
            set { this.random = value; }
        }
        [D2OIgnore]
        public int Group
        {
            get { return this.group; }
            set { this.group = value; }
        }
        [D2OIgnore]
        public int Modificator
        {
            get { return this.modificator; }
            set { this.modificator = value; }
        }
        [D2OIgnore]
        public Boolean Trigger
        {
            get { return this.trigger; }
            set { this.trigger = value; }
        }
        [D2OIgnore]
        public String Triggers
        {
            get { return this.triggers; }
            set { this.triggers = value; }
        }
        [D2OIgnore]
        public Boolean VisibleInTooltip
        {
            get { return this.visibleInTooltip; }
            set { this.visibleInTooltip = value; }
        }
        [D2OIgnore]
        public Boolean VisibleInBuffUi
        {
            get { return this.visibleInBuffUi; }
            set { this.visibleInBuffUi = value; }
        }
        [D2OIgnore]
        public Boolean VisibleInFightLog
        {
            get { return this.visibleInFightLog; }
            set { this.visibleInFightLog = value; }
        }
        [D2OIgnore]
        public Object ZoneSize
        {
            get { return this.zoneSize; }
            set { this.zoneSize = value; }
        }
        [D2OIgnore]
        public uint ZoneShape
        {
            get { return this.zoneShape; }
            set { this.zoneShape = value; }
        }
        [D2OIgnore]
        public Object ZoneMinSize
        {
            get { return this.zoneMinSize; }
            set { this.zoneMinSize = value; }
        }
        [D2OIgnore]
        public Object ZoneEfficiencyPercent
        {
            get { return this.zoneEfficiencyPercent; }
            set { this.zoneEfficiencyPercent = value; }
        }
        [D2OIgnore]
        public Object ZoneMaxEfficiency
        {
            get { return this.zoneMaxEfficiency; }
            set { this.zoneMaxEfficiency = value; }
        }
        [D2OIgnore]
        public Object ZoneStopAtTarget
        {
            get { return this.zoneStopAtTarget; }
            set { this.zoneStopAtTarget = value; }
        }
        [D2OIgnore]
        public String RawZone
        {
            get { return this.rawZone; }
            set { this.rawZone = value; }
        }
    }
}
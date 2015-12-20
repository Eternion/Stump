

// Generated on 12/20/2015 18:01:11
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Effect", "com.ankamagames.dofus.datacenter.effects")]
    [Serializable]
    public class Effect : IDataObject, IIndexedData
    {
        public const String MODULE = "Effects";
        public int id;
        [I18NField]
        public uint descriptionId;
        public int iconId;
        public int characteristic;
        public uint category;
        public String @operator;
        public Boolean showInTooltip;
        public Boolean useDice;
        public Boolean forceMinMax;
        public Boolean boost;
        public Boolean active;
        public int oppositeId;
        [I18NField]
        public uint theoreticalDescriptionId;
        public uint theoreticalPattern;
        public Boolean showInSet;
        public int bonusType;
        public Boolean useInFight;
        public uint effectPriority;
        public int elementId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
        [D2OIgnore]
        public int Characteristic
        {
            get { return this.characteristic; }
            set { this.characteristic = value; }
        }
        [D2OIgnore]
        public uint Category
        {
            get { return this.category; }
            set { this.category = value; }
        }
        [D2OIgnore]
        public String Operator
        {
            get { return this.@operator; }
            set { this.@operator = value; }
        }
        [D2OIgnore]
        public Boolean ShowInTooltip
        {
            get { return this.showInTooltip; }
            set { this.showInTooltip = value; }
        }
        [D2OIgnore]
        public Boolean UseDice
        {
            get { return this.useDice; }
            set { this.useDice = value; }
        }
        [D2OIgnore]
        public Boolean ForceMinMax
        {
            get { return this.forceMinMax; }
            set { this.forceMinMax = value; }
        }
        [D2OIgnore]
        public Boolean Boost
        {
            get { return this.boost; }
            set { this.boost = value; }
        }
        [D2OIgnore]
        public Boolean Active
        {
            get { return this.active; }
            set { this.active = value; }
        }
        [D2OIgnore]
        public int OppositeId
        {
            get { return this.oppositeId; }
            set { this.oppositeId = value; }
        }
        [D2OIgnore]
        public uint TheoreticalDescriptionId
        {
            get { return this.theoreticalDescriptionId; }
            set { this.theoreticalDescriptionId = value; }
        }
        [D2OIgnore]
        public uint TheoreticalPattern
        {
            get { return this.theoreticalPattern; }
            set { this.theoreticalPattern = value; }
        }
        [D2OIgnore]
        public Boolean ShowInSet
        {
            get { return this.showInSet; }
            set { this.showInSet = value; }
        }
        [D2OIgnore]
        public int BonusType
        {
            get { return this.bonusType; }
            set { this.bonusType = value; }
        }
        [D2OIgnore]
        public Boolean UseInFight
        {
            get { return this.useInFight; }
            set { this.useInFight = value; }
        }
        [D2OIgnore]
        public uint EffectPriority
        {
            get { return this.effectPriority; }
            set { this.effectPriority = value; }
        }
        [D2OIgnore]
        public int ElementId
        {
            get { return this.elementId; }
            set { this.elementId = value; }
        }
    }
}
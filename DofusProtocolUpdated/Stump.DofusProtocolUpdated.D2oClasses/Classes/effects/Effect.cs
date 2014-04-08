

// Generated on 12/12/2013 16:57:37
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
        [I18NField]
        public uint theoreticalDescriptionId;
        public uint theoreticalPattern;
        public Boolean showInSet;
        public int bonusType;
        public Boolean useInFight;
        public uint effectPriority;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }
        [D2OIgnore]
        public int Characteristic
        {
            get { return characteristic; }
            set { characteristic = value; }
        }
        [D2OIgnore]
        public uint Category
        {
            get { return category; }
            set { category = value; }
        }
        [D2OIgnore]
        public String Operator
        {
            get { return @operator; }
            set { @operator = value; }
        }
        [D2OIgnore]
        public Boolean ShowInTooltip
        {
            get { return showInTooltip; }
            set { showInTooltip = value; }
        }
        [D2OIgnore]
        public Boolean UseDice
        {
            get { return useDice; }
            set { useDice = value; }
        }
        [D2OIgnore]
        public Boolean ForceMinMax
        {
            get { return forceMinMax; }
            set { forceMinMax = value; }
        }
        [D2OIgnore]
        public Boolean Boost
        {
            get { return boost; }
            set { boost = value; }
        }
        [D2OIgnore]
        public Boolean Active
        {
            get { return active; }
            set { active = value; }
        }
        [D2OIgnore]
        public uint TheoreticalDescriptionId
        {
            get { return theoreticalDescriptionId; }
            set { theoreticalDescriptionId = value; }
        }
        [D2OIgnore]
        public uint TheoreticalPattern
        {
            get { return theoreticalPattern; }
            set { theoreticalPattern = value; }
        }
        [D2OIgnore]
        public Boolean ShowInSet
        {
            get { return showInSet; }
            set { showInSet = value; }
        }
        [D2OIgnore]
        public int BonusType
        {
            get { return bonusType; }
            set { bonusType = value; }
        }
        [D2OIgnore]
        public Boolean UseInFight
        {
            get { return useInFight; }
            set { useInFight = value; }
        }
        [D2OIgnore]
        public uint EffectPriority
        {
            get { return effectPriority; }
            set { effectPriority = value; }
        }
    }
}
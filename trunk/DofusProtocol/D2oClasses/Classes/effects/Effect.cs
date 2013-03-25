
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Effects")]
    [Serializable]
    public class Effect : IDataObject, IIndexedData
    {
        private const String MODULE = "Effects";
        public int id;
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
        public Boolean showInSet;
        public int bonusType;
        public Boolean useInFight;
        public uint effectPriority;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public int Characteristic
        {
            get { return characteristic; }
            set { characteristic = value; }
        }

        public uint Category
        {
            get { return category; }
            set { category = value; }
        }

        public String Operator
        {
            get { return @operator; }
            set { @operator = value; }
        }

        public Boolean ShowInTooltip
        {
            get { return showInTooltip; }
            set { showInTooltip = value; }
        }

        public Boolean UseDice
        {
            get { return useDice; }
            set { useDice = value; }
        }

        public Boolean ForceMinMax
        {
            get { return forceMinMax; }
            set { forceMinMax = value; }
        }

        public Boolean Boost
        {
            get { return boost; }
            set { boost = value; }
        }

        public Boolean Active
        {
            get { return active; }
            set { active = value; }
        }

        public Boolean ShowInSet
        {
            get { return showInSet; }
            set { showInSet = value; }
        }

        public int BonusType
        {
            get { return bonusType; }
            set { bonusType = value; }
        }

        public Boolean UseInFight
        {
            get { return useInFight; }
            set { useInFight = value; }
        }

        public uint EffectPriority
        {
            get { return effectPriority; }
            set { effectPriority = value; }
        }

    }
}
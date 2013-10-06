 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Effects")]
    [D2OClass("Effect")]
    public class EffectRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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

        [NullString]
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Effect)obj;
            
            Id = castedObj.id;
            DescriptionId = castedObj.descriptionId;
            IconId = castedObj.iconId;
            Characteristic = castedObj.characteristic;
            Category = castedObj.category;
            Operator = castedObj.@operator;
            ShowInTooltip = castedObj.showInTooltip;
            UseDice = castedObj.useDice;
            ForceMinMax = castedObj.forceMinMax;
            Boost = castedObj.boost;
            Active = castedObj.active;
            ShowInSet = castedObj.showInSet;
            BonusType = castedObj.bonusType;
            UseInFight = castedObj.useInFight;
            EffectPriority = castedObj.effectPriority;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new Effect();
            obj.id = Id;
            obj.descriptionId = DescriptionId;
            obj.iconId = IconId;
            obj.characteristic = Characteristic;
            obj.category = Category;
            obj.@operator = Operator;
            obj.showInTooltip = ShowInTooltip;
            obj.useDice = UseDice;
            obj.forceMinMax = ForceMinMax;
            obj.boost = Boost;
            obj.active = Active;
            obj.showInSet = ShowInSet;
            obj.bonusType = BonusType;
            obj.useInFight = UseInFight;
            obj.effectPriority = EffectPriority;
            return obj;
        
        }
    }
}
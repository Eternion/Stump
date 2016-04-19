 


// Generated on 04/19/2016 10:18:06
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Effects")]
    [D2OClass("Effect", "com.ankamagames.dofus.datacenter.effects")]
    public class EffectRecord : ID2ORecord, ISaveIntercepter
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

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
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
        [NullString]
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
        public int OppositeId
        {
            get { return oppositeId; }
            set { oppositeId = value; }
        }

        [D2OIgnore]
        [I18NField]
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

        [D2OIgnore]
        public int ElementId
        {
            get { return elementId; }
            set { elementId = value; }
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
            OppositeId = castedObj.oppositeId;
            TheoreticalDescriptionId = castedObj.theoreticalDescriptionId;
            TheoreticalPattern = castedObj.theoreticalPattern;
            ShowInSet = castedObj.showInSet;
            BonusType = castedObj.bonusType;
            UseInFight = castedObj.useInFight;
            EffectPriority = castedObj.effectPriority;
            ElementId = castedObj.elementId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Effect)parent : new Effect();
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
            obj.oppositeId = OppositeId;
            obj.theoreticalDescriptionId = TheoreticalDescriptionId;
            obj.theoreticalPattern = TheoreticalPattern;
            obj.showInSet = ShowInSet;
            obj.bonusType = BonusType;
            obj.useInFight = UseInFight;
            obj.effectPriority = EffectPriority;
            obj.elementId = ElementId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}
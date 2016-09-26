 


// Generated on 09/26/2016 01:50:45
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
    [TableName("ActionDescriptions")]
    [D2OClass("ActionDescription", "com.ankamagames.dofus.datacenter.misc")]
    public class ActionDescriptionRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "ActionDescriptions";
        public uint id;
        public uint typeId;
        public String name;
        [I18NField]
        public uint descriptionId;
        public Boolean trusted;
        public Boolean needInteraction;
        public uint maxUsePerFrame;
        public uint minimalUseInterval;
        public Boolean needConfirmation;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public Boolean Trusted
        {
            get { return trusted; }
            set { trusted = value; }
        }

        [D2OIgnore]
        public Boolean NeedInteraction
        {
            get { return needInteraction; }
            set { needInteraction = value; }
        }

        [D2OIgnore]
        public uint MaxUsePerFrame
        {
            get { return maxUsePerFrame; }
            set { maxUsePerFrame = value; }
        }

        [D2OIgnore]
        public uint MinimalUseInterval
        {
            get { return minimalUseInterval; }
            set { minimalUseInterval = value; }
        }

        [D2OIgnore]
        public Boolean NeedConfirmation
        {
            get { return needConfirmation; }
            set { needConfirmation = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (ActionDescription)obj;
            
            Id = castedObj.id;
            TypeId = castedObj.typeId;
            Name = castedObj.name;
            DescriptionId = castedObj.descriptionId;
            Trusted = castedObj.trusted;
            NeedInteraction = castedObj.needInteraction;
            MaxUsePerFrame = castedObj.maxUsePerFrame;
            MinimalUseInterval = castedObj.minimalUseInterval;
            NeedConfirmation = castedObj.needConfirmation;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (ActionDescription)parent : new ActionDescription();
            obj.id = Id;
            obj.typeId = TypeId;
            obj.name = Name;
            obj.descriptionId = DescriptionId;
            obj.trusted = Trusted;
            obj.needInteraction = NeedInteraction;
            obj.maxUsePerFrame = MaxUsePerFrame;
            obj.minimalUseInterval = MinimalUseInterval;
            obj.needConfirmation = NeedConfirmation;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}
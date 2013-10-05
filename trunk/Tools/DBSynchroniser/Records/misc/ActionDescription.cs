 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("ActionDescriptions")]
    public class ActionDescriptionRecord : ID2ORecord
    {
        public const String MODULE = "ActionDescriptions";
        public uint id;
        public uint typeId;
        public String name;
        public uint descriptionId;
        public Boolean trusted;
        public Boolean needInteraction;
        public uint maxUsePerFrame;
        public uint minimalUseInterval;
        public Boolean needConfirmation;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public Boolean Trusted
        {
            get { return trusted; }
            set { trusted = value; }
        }

        public Boolean NeedInteraction
        {
            get { return needInteraction; }
            set { needInteraction = value; }
        }

        public uint MaxUsePerFrame
        {
            get { return maxUsePerFrame; }
            set { maxUsePerFrame = value; }
        }

        public uint MinimalUseInterval
        {
            get { return minimalUseInterval; }
            set { minimalUseInterval = value; }
        }

        public Boolean NeedConfirmation
        {
            get { return needConfirmation; }
            set { needConfirmation = value; }
        }

        public void AssignFields(object obj)
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
        
        public object CreateObject()
        {
            var obj = new ActionDescription();
            
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
    }
}
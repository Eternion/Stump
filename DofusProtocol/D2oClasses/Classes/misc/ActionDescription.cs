

// Generated on 09/01/2015 11:16:33
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ActionDescription", "com.ankamagames.dofus.datacenter.misc")]
    [Serializable]
    public class ActionDescription : IDataObject, IIndexedData
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
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public Boolean Trusted
        {
            get { return this.trusted; }
            set { this.trusted = value; }
        }
        [D2OIgnore]
        public Boolean NeedInteraction
        {
            get { return this.needInteraction; }
            set { this.needInteraction = value; }
        }
        [D2OIgnore]
        public uint MaxUsePerFrame
        {
            get { return this.maxUsePerFrame; }
            set { this.maxUsePerFrame = value; }
        }
        [D2OIgnore]
        public uint MinimalUseInterval
        {
            get { return this.minimalUseInterval; }
            set { this.minimalUseInterval = value; }
        }
        [D2OIgnore]
        public Boolean NeedConfirmation
        {
            get { return this.needConfirmation; }
            set { this.needConfirmation = value; }
        }
    }
}
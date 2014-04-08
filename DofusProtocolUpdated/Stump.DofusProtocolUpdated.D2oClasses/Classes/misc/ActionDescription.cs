

// Generated on 12/12/2013 16:57:41
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
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        [D2OIgnore]
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
    }
}
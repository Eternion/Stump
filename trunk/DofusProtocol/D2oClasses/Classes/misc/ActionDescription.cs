
// Generated on 03/25/2013 19:24:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ActionDescriptions")]
    [Serializable]
    public class ActionDescription : IDataObject, IIndexedData
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

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

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

    }
}
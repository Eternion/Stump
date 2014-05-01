

// Generated on 10/28/2013 14:03:19
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("TypeAction", "com.ankamagames.dofus.datacenter.misc")]
    [Serializable]
    public class TypeAction : IDataObject, IIndexedData
    {
        public const String MODULE = "TypeActions";
        public int id;
        public String elementName;
        public int elementId;
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
        public String ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }
        [D2OIgnore]
        public int ElementId
        {
            get { return elementId; }
            set { elementId = value; }
        }
    }
}
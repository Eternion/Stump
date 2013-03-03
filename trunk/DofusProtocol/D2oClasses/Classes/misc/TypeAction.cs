
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("TypeActions")]
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

        public int ElementId
        {
            get { return elementId; }
            set { elementId = value; }
        }

    }
}
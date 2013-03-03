
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Pack")]
    [Serializable]
    public class Pack : IDataObject, IIndexedData
    {
        private const String MODULE = "Pack";
        public int id;
        public String name;
        public Boolean hasSubAreas;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Boolean HasSubAreas
        {
            get { return hasSubAreas; }
            set { hasSubAreas = value; }
        }

    }
}


// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Pack", "com.ankamagames.dofus.datacenter.misc")]
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
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        [D2OIgnore]
        public Boolean HasSubAreas
        {
            get { return hasSubAreas; }
            set { hasSubAreas = value; }
        }
    }
}


// Generated on 09/26/2016 01:50:30
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
        public const String MODULE = "Pack";
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        [D2OIgnore]
        public Boolean HasSubAreas
        {
            get { return this.hasSubAreas; }
            set { this.hasSubAreas = value; }
        }
    }
}
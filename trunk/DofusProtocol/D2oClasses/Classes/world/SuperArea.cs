

// Generated on 10/06/2013 17:58:57
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SuperArea", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class SuperArea : IDataObject, IIndexedData
    {
        private const String MODULE = "SuperAreas";
        public int id;
        public uint nameId;
        public uint worldmapId;
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
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint WorldmapId
        {
            get { return worldmapId; }
            set { worldmapId = value; }
        }
    }
}
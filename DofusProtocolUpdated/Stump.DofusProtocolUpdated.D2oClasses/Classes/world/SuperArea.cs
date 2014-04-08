

// Generated on 12/12/2013 16:57:43
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
        public const String MODULE = "SuperAreas";
        public int id;
        [I18NField]
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
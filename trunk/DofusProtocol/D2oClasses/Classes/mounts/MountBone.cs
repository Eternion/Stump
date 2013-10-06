

// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MountBone", "com.ankamagames.dofus.datacenter.mounts")]
    [Serializable]
    public class MountBone : IDataObject, IIndexedData
    {
        public uint id;
        private String MODULE = "MountBones";
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
    }
}
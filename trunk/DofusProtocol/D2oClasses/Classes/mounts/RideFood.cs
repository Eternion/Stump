

// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("RideFood", "com.ankamagames.dofus.datacenter.mounts")]
    [Serializable]
    public class RideFood : IDataObject
    {
        public uint gid;
        public uint typeId;
        public String MODULE = "RideFood";
        [D2OIgnore]
        public uint Gid
        {
            get { return gid; }
            set { gid = value; }
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }
    }
}
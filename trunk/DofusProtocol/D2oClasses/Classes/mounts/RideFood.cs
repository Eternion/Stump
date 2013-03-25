
// Generated on 03/25/2013 19:24:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("RideFood")]
    [Serializable]
    public class RideFood : IDataObject, IIndexedData
    {
        public uint gid;
        public uint typeId;
        public String MODULE = "RideFood";

        int IIndexedData.Id
        {
            get { return (int)gid; }
        }

        public uint Gid
        {
            get { return gid; }
            set { gid = value; }
        }

        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

    }
}
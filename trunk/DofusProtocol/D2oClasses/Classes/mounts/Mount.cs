
// Generated on 03/25/2013 19:24:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Mounts")]
    [Serializable]
    public class Mount : IDataObject, IIndexedData
    {
        public uint id;
        public uint nameId;
        public String look;
        private String MODULE = "Mounts";

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public String Look
        {
            get { return look; }
            set { look = value; }
        }

    }
}
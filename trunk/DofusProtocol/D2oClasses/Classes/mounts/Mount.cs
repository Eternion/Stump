

// Generated on 10/28/2013 14:03:20
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Mount", "com.ankamagames.dofus.datacenter.mounts")]
    [Serializable]
    public class Mount : IDataObject, IIndexedData
    {
        public uint id;
        [I18NField]
        public uint nameId;
        public String look;
        private String MODULE = "Mounts";
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
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public String Look
        {
            get { return look; }
            set { look = value; }
        }
    }
}
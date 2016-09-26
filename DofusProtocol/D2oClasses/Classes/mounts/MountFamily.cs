

// Generated on 09/26/2016 01:50:31
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MountFamily", "com.ankamagames.dofus.datacenter.mounts")]
    [Serializable]
    public class MountFamily : IDataObject, IIndexedData
    {
        private String MODULE = "MountFamily";
        public uint id;
        [I18NField]
        public uint nameId;
        public String headUri;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public String HeadUri
        {
            get { return this.headUri; }
            set { this.headUri = value; }
        }
    }
}
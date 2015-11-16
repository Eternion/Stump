

// Generated on 11/16/2015 14:26:29
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SkinMapping", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class SkinMapping : IDataObject, IIndexedData
    {
        public const String MODULE = "SkinMappings";
        public int id;
        public int lowDefId;
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
        public int LowDefId
        {
            get { return this.lowDefId; }
            set { this.lowDefId = value; }
        }
    }
}
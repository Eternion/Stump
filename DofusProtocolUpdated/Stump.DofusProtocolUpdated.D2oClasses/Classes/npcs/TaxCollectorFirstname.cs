

// Generated on 12/12/2013 16:57:41
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("TaxCollectorFirstname", "com.ankamagames.dofus.datacenter.npcs")]
    [Serializable]
    public class TaxCollectorFirstname : IDataObject, IIndexedData
    {
        public const String MODULE = "TaxCollectorFirstnames";
        public int id;
        [I18NField]
        public uint firstnameId;
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
        public uint FirstnameId
        {
            get { return firstnameId; }
            set { firstnameId = value; }
        }
    }
}
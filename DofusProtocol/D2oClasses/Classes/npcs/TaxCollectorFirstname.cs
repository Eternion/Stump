

// Generated on 02/18/2015 11:06:01
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint FirstnameId
        {
            get { return this.firstnameId; }
            set { this.firstnameId = value; }
        }
    }
}
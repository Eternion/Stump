

// Generated on 11/16/2015 14:26:30
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("RankName", "com.ankamagames.dofus.datacenter.guild")]
    [Serializable]
    public class RankName : IDataObject, IIndexedData
    {
        public const String MODULE = "RankNames";
        public int id;
        [I18NField]
        public uint nameId;
        public int order;
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}
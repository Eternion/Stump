

// Generated on 11/16/2015 14:26:30
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SmileyPack", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class SmileyPack : IDataObject, IIndexedData
    {
        public const String MODULE = "SmileyPacks";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint order;
        public List<uint> smileys;
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
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public List<uint> Smileys
        {
            get { return this.smileys; }
            set { this.smileys = value; }
        }
    }
}
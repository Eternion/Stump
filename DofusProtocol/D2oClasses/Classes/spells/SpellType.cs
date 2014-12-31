

// Generated on 12/29/2014 16:35:24
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpellType", "com.ankamagames.dofus.datacenter.spells")]
    [Serializable]
    public class SpellType : IDataObject, IIndexedData
    {
        public const String MODULE = "SpellTypes";
        public int id;
        [I18NField]
        public uint longNameId;
        [I18NField]
        public uint shortNameId;
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
        public uint LongNameId
        {
            get { return this.longNameId; }
            set { this.longNameId = value; }
        }
        [D2OIgnore]
        public uint ShortNameId
        {
            get { return this.shortNameId; }
            set { this.shortNameId = value; }
        }
    }
}
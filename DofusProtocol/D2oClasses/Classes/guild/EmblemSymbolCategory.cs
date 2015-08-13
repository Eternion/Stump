

// Generated on 08/13/2015 17:13:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EmblemSymbolCategory", "com.ankamagames.dofus.datacenter.guild")]
    [Serializable]
    public class EmblemSymbolCategory : IDataObject, IIndexedData
    {
        public const String MODULE = "EmblemSymbolCategories";
        public int id;
        [I18NField]
        public uint nameId;
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
    }
}
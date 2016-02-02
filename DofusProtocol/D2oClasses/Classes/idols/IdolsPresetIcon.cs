

// Generated on 02/02/2016 14:15:01
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("IdolsPresetIcon", "com.ankamagames.dofus.datacenter.idols")]
    [Serializable]
    public class IdolsPresetIcon : IDataObject, IIndexedData
    {
        public const String MODULE = "IdolsPresetIcons";
        public int id;
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
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}
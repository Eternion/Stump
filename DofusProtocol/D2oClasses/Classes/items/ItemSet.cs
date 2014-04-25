

// Generated on 10/28/2013 14:03:18
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ItemSet", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class ItemSet : IDataObject, IIndexedData
    {
        private const String MODULE = "ItemSets";
        public uint id;
        public List<uint> items;
        [I18NField]
        public uint nameId;
        public List<List<EffectInstance>> effects;
        public Boolean bonusIsSecret;
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
        public List<uint> Items
        {
            get { return items; }
            set { items = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public List<List<EffectInstance>> Effects
        {
            get { return effects; }
            set { effects = value; }
        }
        [D2OIgnore]
        public Boolean BonusIsSecret
        {
            get { return bonusIsSecret; }
            set { bonusIsSecret = value; }
        }
    }
}
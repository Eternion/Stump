
// Generated on 03/25/2013 19:24:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ItemSets")]
    [Serializable]
    public class ItemSet : IDataObject, IIndexedData
    {
        private const String MODULE = "ItemSets";
        public uint id;
        public List<uint> items;
        public uint nameId;
        public List<List<EffectInstance>> effects;
        public Boolean bonusIsSecret;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public List<uint> Items
        {
            get { return items; }
            set { items = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public List<List<EffectInstance>> Effects
        {
            get { return effects; }
            set { effects = value; }
        }

        public Boolean BonusIsSecret
        {
            get { return bonusIsSecret; }
            set { bonusIsSecret = value; }
        }

    }
}
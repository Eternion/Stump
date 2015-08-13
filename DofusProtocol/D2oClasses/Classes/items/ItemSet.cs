

// Generated on 08/13/2015 17:13:47
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
        public const String MODULE = "ItemSets";
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public List<uint> Items
        {
            get { return this.items; }
            set { this.items = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public List<List<EffectInstance>> Effects
        {
            get { return this.effects; }
            set { this.effects = value; }
        }
        [D2OIgnore]
        public Boolean BonusIsSecret
        {
            get { return this.bonusIsSecret; }
            set { this.bonusIsSecret = value; }
        }
    }
}
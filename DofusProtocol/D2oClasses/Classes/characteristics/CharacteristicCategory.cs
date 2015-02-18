

// Generated on 02/18/2015 11:05:57
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CharacteristicCategory", "com.ankamagames.dofus.datacenter.characteristics")]
    [Serializable]
    public class CharacteristicCategory : IDataObject, IIndexedData
    {
        public const String MODULE = "CharacteristicCategories";
        public int id;
        [I18NField]
        public uint nameId;
        public int order;
        public List<uint> characteristicIds;
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
        [D2OIgnore]
        public List<uint> CharacteristicIds
        {
            get { return this.characteristicIds; }
            set { this.characteristicIds = value; }
        }
    }
}
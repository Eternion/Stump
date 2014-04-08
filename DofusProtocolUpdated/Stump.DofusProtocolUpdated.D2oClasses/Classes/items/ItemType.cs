

// Generated on 12/12/2013 16:57:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ItemType", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class ItemType : IDataObject, IIndexedData
    {
        public const String MODULE = "ItemTypes";
        public int id;
        [I18NField]
        public uint nameId;
        public uint superTypeId;
        public Boolean plural;
        public uint gender;
        public String rawZone;
        public Boolean needUseConfirm;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint SuperTypeId
        {
            get { return superTypeId; }
            set { superTypeId = value; }
        }
        [D2OIgnore]
        public Boolean Plural
        {
            get { return plural; }
            set { plural = value; }
        }
        [D2OIgnore]
        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        [D2OIgnore]
        public String RawZone
        {
            get { return rawZone; }
            set { rawZone = value; }
        }
        [D2OIgnore]
        public Boolean NeedUseConfirm
        {
            get { return needUseConfirm; }
            set { needUseConfirm = value; }
        }
    }
}
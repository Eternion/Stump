
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ItemTypes")]
    [Serializable]
    public class ItemType : IDataObject, IIndexedData
    {
        private const String MODULE = "ItemTypes";
        public int id;
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint SuperTypeId
        {
            get { return superTypeId; }
            set { superTypeId = value; }
        }

        public Boolean Plural
        {
            get { return plural; }
            set { plural = value; }
        }

        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public String RawZone
        {
            get { return rawZone; }
            set { rawZone = value; }
        }

        public Boolean NeedUseConfirm
        {
            get { return needUseConfirm; }
            set { needUseConfirm = value; }
        }

    }
}
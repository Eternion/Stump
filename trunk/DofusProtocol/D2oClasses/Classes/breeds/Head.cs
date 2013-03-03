
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Heads")]
    [Serializable]
    public class Head : IDataObject, IIndexedData
    {
        private const String MODULE = "Heads";
        public int id;
        public String skins;
        public String assetId;
        public uint breed;
        public uint gender;
        public uint order;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Skins
        {
            get { return skins; }
            set { skins = value; }
        }

        public String AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }

        public uint Breed
        {
            get { return breed; }
            set { breed = value; }
        }

        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

    }
}
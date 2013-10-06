

// Generated on 10/06/2013 17:58:56
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Area", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class Area : IDataObject, IIndexedData
    {
        private const String MODULE = "Areas";
        public int id;
        public uint nameId;
        public int superAreaId;
        public Boolean containHouses;
        public Boolean containPaddocks;
        public Rectangle bounds;
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
        public int SuperAreaId
        {
            get { return superAreaId; }
            set { superAreaId = value; }
        }
        [D2OIgnore]
        public Boolean ContainHouses
        {
            get { return containHouses; }
            set { containHouses = value; }
        }
        [D2OIgnore]
        public Boolean ContainPaddocks
        {
            get { return containPaddocks; }
            set { containPaddocks = value; }
        }
        [D2OIgnore]
        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }
    }
}
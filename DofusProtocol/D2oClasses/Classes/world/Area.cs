

// Generated on 08/13/2015 17:13:55
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
        public const String MODULE = "Areas";
        public int id;
        [I18NField]
        public uint nameId;
        public int superAreaId;
        public Boolean containHouses;
        public Boolean containPaddocks;
        public Rectangle bounds;
        public uint worldmapId;
        public Boolean hasWorldMap;
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
        public int SuperAreaId
        {
            get { return this.superAreaId; }
            set { this.superAreaId = value; }
        }
        [D2OIgnore]
        public Boolean ContainHouses
        {
            get { return this.containHouses; }
            set { this.containHouses = value; }
        }
        [D2OIgnore]
        public Boolean ContainPaddocks
        {
            get { return this.containPaddocks; }
            set { this.containPaddocks = value; }
        }
        [D2OIgnore]
        public Rectangle Bounds
        {
            get { return this.bounds; }
            set { this.bounds = value; }
        }
        [D2OIgnore]
        public uint WorldmapId
        {
            get { return this.worldmapId; }
            set { this.worldmapId = value; }
        }
        [D2OIgnore]
        public Boolean HasWorldMap
        {
            get { return this.hasWorldMap; }
            set { this.hasWorldMap = value; }
        }
    }
}
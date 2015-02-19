

// Generated on 02/18/2015 11:06:02
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SuperArea", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class SuperArea : IDataObject, IIndexedData
    {
        public const String MODULE = "SuperAreas";
        public int id;
        [I18NField]
        public uint nameId;
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


// Generated on 12/12/2013 16:57:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Dungeon", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class Dungeon : IDataObject, IIndexedData
    {
        public const String MODULE = "Dungeons";
        public int id;
        [I18NField]
        public uint nameId;
        public int optimalPlayerLevel;
        public List<int> mapIds;
        public int entranceMapId;
        public int exitMapId;
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
        public int OptimalPlayerLevel
        {
            get { return optimalPlayerLevel; }
            set { optimalPlayerLevel = value; }
        }
        [D2OIgnore]
        public List<int> MapIds
        {
            get { return mapIds; }
            set { mapIds = value; }
        }
        [D2OIgnore]
        public int EntranceMapId
        {
            get { return entranceMapId; }
            set { entranceMapId = value; }
        }
        [D2OIgnore]
        public int ExitMapId
        {
            get { return exitMapId; }
            set { exitMapId = value; }
        }
    }
}


// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MonsterMiniBoss", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class MonsterMiniBoss : IDataObject, IIndexedData
    {
        private const String MODULE = "MonsterMiniBoss";
        public int id;
        public int monsterReplacingId;
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
        public int MonsterReplacingId
        {
            get { return monsterReplacingId; }
            set { monsterReplacingId = value; }
        }
    }
}


// Generated on 12/12/2013 16:57:41
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
        public const String MODULE = "MonsterMiniBoss";
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


// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AchievementObjective", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class AchievementObjective : IDataObject, IIndexedData
    {
        private const String MODULE = "AchievementObjectives";
        public uint id;
        public uint achievementId;
        public uint nameId;
        public String criterion;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint AchievementId
        {
            get { return achievementId; }
            set { achievementId = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public String Criterion
        {
            get { return criterion; }
            set { criterion = value; }
        }
    }
}
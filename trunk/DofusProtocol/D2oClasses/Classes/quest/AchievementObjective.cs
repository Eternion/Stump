
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AchievementObjectives")]
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

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint AchievementId
        {
            get { return achievementId; }
            set { achievementId = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public String Criterion
        {
            get { return criterion; }
            set { criterion = value; }
        }

    }
}
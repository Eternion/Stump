

// Generated on 12/12/2013 16:57:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("IncarnationLevel", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class IncarnationLevel : IDataObject, IIndexedData
    {
        public const String MODULE = "IncarnationLevels";
        public int id;
        public int incarnationId;
        public int level;
        public uint requiredXp;
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
        public int IncarnationId
        {
            get { return incarnationId; }
            set { incarnationId = value; }
        }
        [D2OIgnore]
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        [D2OIgnore]
        public uint RequiredXp
        {
            get { return requiredXp; }
            set { requiredXp = value; }
        }
    }
}
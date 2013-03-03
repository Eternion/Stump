
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("IncarnationLevels")]
    [Serializable]
    public class IncarnationLevel : IDataObject, IIndexedData
    {
        private const String MODULE = "IncarnationLevels";
        public int id;
        public int incarnationId;
        public int level;
        public uint requiredXp;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int IncarnationId
        {
            get { return incarnationId; }
            set { incarnationId = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public uint RequiredXp
        {
            get { return requiredXp; }
            set { requiredXp = value; }
        }

    }
}
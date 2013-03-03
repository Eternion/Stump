
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SkinMappings")]
    [Serializable]
    public class SkinMapping : IDataObject, IIndexedData
    {
        private const String MODULE = "SkinMappings";
        public int id;
        public int lowDefId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int LowDefId
        {
            get { return lowDefId; }
            set { lowDefId = value; }
        }

    }
}
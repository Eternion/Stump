
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentSides")]
    [Serializable]
    public class AlignmentSide : IDataObject, IIndexedData
    {
        private const String MODULE = "AlignmentSides";
        public int id;
        public uint nameId;
        public Boolean canConquest;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public Boolean CanConquest
        {
            get { return canConquest; }
            set { canConquest = value; }
        }

    }
}
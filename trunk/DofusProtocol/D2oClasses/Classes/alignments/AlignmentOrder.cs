
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentOrder")]
    [Serializable]
    public class AlignmentOrder : IDataObject, IIndexedData
    {
        private const String MODULE = "AlignmentOrder";
        public int id;
        public uint nameId;
        public uint sideId;

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

        public uint SideId
        {
            get { return sideId; }
            set { sideId = value; }
        }

    }
}

// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Tips")]
    [Serializable]
    public class Tips : IDataObject, IIndexedData
    {
        private const String MODULE = "Tips";
        public int id;
        public uint descId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }

    }
}
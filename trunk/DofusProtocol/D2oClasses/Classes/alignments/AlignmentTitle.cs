
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentTitles")]
    [Serializable]
    public class AlignmentTitle : IDataObject, IIndexedData
    {
        private const String MODULE = "AlignmentTitles";
        public int sideId;
        public List<int> namesId;
        public List<int> shortsId;

        int IIndexedData.Id
        {
            get { return (int)sideId; }
        }

        public int SideId
        {
            get { return sideId; }
            set { sideId = value; }
        }

        public List<int> NamesId
        {
            get { return namesId; }
            set { namesId = value; }
        }

        public List<int> ShortsId
        {
            get { return shortsId; }
            set { shortsId = value; }
        }

    }
}
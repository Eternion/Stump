

// Generated on 12/12/2013 16:57:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentTitle", "com.ankamagames.dofus.datacenter.alignments")]
    [Serializable]
    public class AlignmentTitle : IDataObject, IIndexedData
    {
        public const String MODULE = "AlignmentTitles";
        public int sideId;
        public List<int> namesId;
        public List<int> shortsId;
        int IIndexedData.Id
        {
            get { return (int)sideId; }
        }
        [D2OIgnore]
        public int SideId
        {
            get { return sideId; }
            set { sideId = value; }
        }
        [D2OIgnore]
        public List<int> NamesId
        {
            get { return namesId; }
            set { namesId = value; }
        }
        [D2OIgnore]
        public List<int> ShortsId
        {
            get { return shortsId; }
            set { shortsId = value; }
        }
    }
}
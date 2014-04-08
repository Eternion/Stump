

// Generated on 12/12/2013 16:57:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentRankJntGift", "com.ankamagames.dofus.datacenter.alignments")]
    [Serializable]
    public class AlignmentRankJntGift : IDataObject, IIndexedData
    {
        public const String MODULE = "AlignmentRankJntGift";
        public int id;
        public List<int> gifts;
        public List<int> parameters;
        public List<int> levels;
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
        public List<int> Gifts
        {
            get { return gifts; }
            set { gifts = value; }
        }
        [D2OIgnore]
        public List<int> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
        [D2OIgnore]
        public List<int> Levels
        {
            get { return levels; }
            set { levels = value; }
        }
    }
}


// Generated on 02/18/2015 11:05:56
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public List<int> Gifts
        {
            get { return this.gifts; }
            set { this.gifts = value; }
        }
        [D2OIgnore]
        public List<int> Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }
        [D2OIgnore]
        public List<int> Levels
        {
            get { return this.levels; }
            set { this.levels = value; }
        }
    }
}
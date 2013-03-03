
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentRankJntGift")]
    [Serializable]
    public class AlignmentRankJntGift : IDataObject, IIndexedData
    {
        private const String MODULE = "AlignmentRankJntGift";
        public int id;
        public List<int> gifts;
        public List<int> parameters;
        public List<int> levels;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public List<int> Gifts
        {
            get { return gifts; }
            set { gifts = value; }
        }

        public List<int> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public List<int> Levels
        {
            get { return levels; }
            set { levels = value; }
        }

    }
}
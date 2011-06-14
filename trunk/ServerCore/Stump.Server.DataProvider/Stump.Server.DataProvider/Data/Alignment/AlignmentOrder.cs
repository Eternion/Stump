
using System.Collections.Generic;

namespace Stump.Server.DataProvider.Data.Alignment
{
    public class AlignmentOrder
    {
        public int Id { get; set; }

        public AlignmentSide AlignmentSide { get; set; }

        public List<AlignmentRank> Ranks;
    }
}
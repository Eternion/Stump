
using System.Collections.Generic;

namespace Stump.Server.DataProvider.Data.Alignment
{
    public class AlignmentRank
    {
        public int Id { get; set; }

        public AlignmentOrder AlignmentOrder { get; set; }

        public int MinLevelAlignment { get; set; }

        public List<AlignmentGift> Gifts { get; set; }
    }
}
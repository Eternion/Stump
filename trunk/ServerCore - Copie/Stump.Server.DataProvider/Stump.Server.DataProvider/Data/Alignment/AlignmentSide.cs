
using System.Collections.Generic;

namespace Stump.Server.DataProvider.Data.Alignment
{
    public class AlignmentSide
    {
        public int Id { get; set; }

        public bool CanConquest { get; set; }

        public List<AlignmentOrder> Orders { get; set; }
    }
}
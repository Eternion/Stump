
using System;
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.Alignment
{
    [ActiveRecord("subareas_prism")]
    public sealed class PrismRecord : WorldBaseRecord<PrismRecord>
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "SubAreaId")]
        public uint SubAreaId { get; set; }

        [OneToOne]
        public SubAreaRecord SubArea { get; set; }

        [Property("MapId", NotNull = true)]
        public uint MapId { get; set; }

        [Property("PlaceDate", NotNull = true)]
        public DateTime PlaceDate { get; set; }
    }
}

using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.House
{
    [ActiveRecord("accounts_house"), JoinedBase]
    public class HouseRecord : WorldBaseRecord<HouseRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "HouseId")]
        public uint HouseId { get; set; }

        [BelongsTo("AccountId", NotNull = true)]
        public WorldAccountRecord Account { get; set; }

        [Property("Price", NotNull = false)]
        public uint Price { get; set; }

        [Property("Password", NotNull = false)]
        public uint Password { get; set; }
    }
}
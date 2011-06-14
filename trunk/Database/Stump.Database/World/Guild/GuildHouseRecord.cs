
using Castle.ActiveRecord;
using Stump.Database.WorldServer.House;

namespace Stump.Database.WorldServer.Guild
{
    [ActiveRecord("guilds_houses")]
    public sealed class GuildHouseRecord : HouseRecord
    {
        [JoinedKey("HouseId")]
        public uint HouseId { get; set; }

        [BelongsTo("GuildId", NotNull = true)]
        public GuildRecord Guild { get; set; }
    }
}
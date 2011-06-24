
using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Character;
using Stump.Database.WorldServer.Storage;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.WorldServer.Guild
{
    [ActiveRecord("guilds_collectors")]
    public sealed class CollectorRecord : WorldBaseRecord<CollectorRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id { get; set; }

        [BelongsTo("GuildId", NotNull = true)]
        public GuildRecord Guild { get; set; }

        [BelongsTo("OwnerId", NotNull = true)]
        public CharacterRecord Owner { get; set; }

        [Property("LastNameId", NotNull = true)]
        public uint LastNameId { get; set; }

        [Property("FirstNameId", NotNull = true)]
        public uint FirstNameId { get; set; }

        [Property("MapId", NotNull = true)]
        public int MapId { get; set; }

        [Property("CellId", NotNull = true)]
        public ushort CellId { get; set; }

        [Property("Direction", NotNull = true)]
        public DirectionsEnum Direction { get; set; }

        [BelongsTo("InventoryId", NotNull = true, Cascade = CascadeEnum.Delete)]
        public InventoryRecord Inventory { get; set; }

        [Property("SetDate", NotNull = true)]
        public DateTime SetDate { get; set; }
    }
}
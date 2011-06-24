
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.Guild
{
    [ActiveRecord("guilds_emblem")]
    public sealed class GuildEmblemRecord : WorldBaseRecord<GuildEmblemRecord>
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "GuildId")]
        public uint GuildId { get; set; }

        [OneToOne(Cascade = CascadeEnum.Delete)]
        public GuildRecord Guild { get; set; }

        [Property("SymbolShape", NotNull = true, Default = "0")]
        public int SymbolShape { get; set; }

        [Property("SymbolColor", NotNull = true, Default = "0")]
        public int SymbolColor { get; set; }

        [Property("BackgroundShape", NotNull = true, Default = "0")]
        public int BackgroundShape { get; set; }

        [Property("BackgroundColor", NotNull = true, Default = "0")]
        public int BackgroundColor { get; set; }
    }
}
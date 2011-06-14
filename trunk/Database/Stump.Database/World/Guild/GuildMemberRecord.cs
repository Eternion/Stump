
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Character;

namespace Stump.Database.WorldServer.Guild
{
    [ActiveRecord("guilds_members")]
    public sealed class GuildMemberRecord : WorldBaseRecord<GuildMemberRecord>
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "CharacterId")]
        public uint CharacterId { get; set; }

        [OneToOne]
        public CharacterRecord Character { get; set; }


        [BelongsTo("GuildId", NotNull = true)]
        public GuildRecord Guild { get; set; }

        [Property("Rank", NotNull = true)]
        public uint Rank { get; set; }

        [Property("Rights", NotNull = true)]
        public uint Rights { get; set; }

        [Property("GivenExperience", NotNull = true, Default = "0")]
        public uint GivenExperience { get; set; }

        [Property("GivenPercent", NotNull = true, Default = "0")]
        public byte GivenPercent { get; set; }
    }
}
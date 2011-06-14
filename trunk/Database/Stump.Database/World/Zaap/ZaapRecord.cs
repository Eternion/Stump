
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Character;

namespace Stump.Database.WorldServer.Zaap
{
    [ActiveRecord("characters_zaaps")]
    public class ZaapRecord : WorldBaseRecord<ZaapRecord>
    {
        [PrimaryKey(PrimaryKeyType.Identity)]
        public long Id { get; set; }

        [BelongsTo("CharacterId", NotNull = true)]
        public CharacterRecord Character { get; set; }

        [Property("ZaapId", NotNull = true)]
        public int ZaapId { get; set; }
    }
}
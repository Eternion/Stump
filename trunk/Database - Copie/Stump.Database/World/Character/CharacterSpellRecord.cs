
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.WorldServer.Character
{
    /// <summary>
    /// A Spell learned by a Character with a position and a level
    /// </summary>
    [ActiveRecord("characters_spells")]
    public class CharacterSpellRecord : WorldBaseRecord<CharacterSpellRecord>
    {
        [PrimaryKey(PrimaryKeyType.Identity)]
        public long Id { get; set; }

        [BelongsTo("CharacterId", NotNull = true)]
        public CharacterRecord Character { get; set; }

        [Property("SpellId", NotNull = true)]
        public uint SpellId { get; set; }

        [Property("Level", NotNull = true, Default = "1")]
        public int Level { get; set; }

        [Property("Position", NotNull = true, Default = "0")]
        public int Position { get; set; }

        public override string ToString()
        {
            return (SpellIdEnum) SpellId + " (" + SpellId + ")";
        }
    }
}
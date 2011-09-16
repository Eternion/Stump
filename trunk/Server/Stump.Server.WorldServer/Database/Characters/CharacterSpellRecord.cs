using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Database.Characters
{
    /// <summary>
    /// A Spell learned by a Character with a position and a level
    /// </summary>
    [ActiveRecord("characters_spells")]
    public class CharacterSpellRecord : WorldBaseRecord<CharacterSpellRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [BelongsTo("CharacterId")]
        public CharacterRecord Character
        {
            get;
            set;
        }

        [Property("SpellId", NotNull = true)]
        public int SpellId
        {
            get;
            set;
        }

        [Property("Level", NotNull = true, Default = "1")]
        public sbyte Level
        {
            get;
            set;
        }

        [Property("Position", NotNull = true, Default = "0")]
        public byte Position
        {
            get;
            set;
        }

        public override string ToString()
        {
            return (SpellIdEnum) SpellId + " (" + SpellId + ")";
        }
    }
}
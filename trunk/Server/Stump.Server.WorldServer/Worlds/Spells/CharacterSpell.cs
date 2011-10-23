using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Worlds.Spells
{
    public class CharacterSpell : Spell
    {
        public CharacterSpell(CharacterSpellRecord record)
            : base(record)
        {
            Record = record;
        }

        public CharacterSpellRecord Record
        {
            get;
            private set;
        }
    }
}
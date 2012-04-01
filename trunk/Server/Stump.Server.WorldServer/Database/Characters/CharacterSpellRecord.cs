using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Spells;

namespace Stump.Server.WorldServer.Database.Characters
{
    /// <summary>
    /// A Spell learned by a Character with a position and a level
    /// </summary>
    [ActiveRecord("characters_spells")]
    public class CharacterSpellRecord : WorldBaseRecord<CharacterSpellRecord>, ISpellRecord
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property("Owner")]
        public int OwnerId
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

        public static CharacterSpellRecord[] FindAllByOwner(int ownerId)
        {
            return FindAll(Restrictions.Eq("OwnerId", ownerId));
        }

        public override string ToString()
        {
            return (SpellIdEnum) SpellId + " (" + SpellId + ")";
        }
    }
}
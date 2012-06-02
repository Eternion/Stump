using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Database.Breeds
{
    [ActiveRecord("breeds_spells")]
    public class LearnableSpell : WorldBaseRecord<LearnableSpell>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [BelongsTo("Breed")]
        public Breed Breed
        {
            get;
            set;
        }

        [Property("Spell")]
        public int SpellId
        {
            get;
            set;
        }

        [Property("ObtainLevel")]
        public ushort ObtainLevel
        {
            get;
            set;
        }
    }
}
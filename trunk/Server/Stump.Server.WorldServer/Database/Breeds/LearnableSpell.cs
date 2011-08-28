using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Database.Breeds
{
    [ActiveRecord("breed_spells")]
    public class LearnableSpell : WorldBaseRecord<LearnableSpell>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [BelongsTo("BreedId")]
        public Breed Breed
        {
            get;
            set;
        }

        [Property("SpellId")]
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
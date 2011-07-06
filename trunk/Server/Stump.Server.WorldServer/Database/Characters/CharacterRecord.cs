using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;

namespace Stump.Server.WorldServer.Database.Characters
{
    [ActiveRecord("characters")]
    public class CharacterRecord : WorldBaseRecord<CharacterRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("Name", Length = 18, NotNull = true)]
        public string Name
        {
            get;
            set;
        }

        [Property("Breed", NotNull = true)]
        public PlayableBreedEnum Breed
        {
            get;
            set;
        }

        [Property("Sex", NotNull = true)]
        public SexTypeEnum Sex
        {
            get;
            set;
        }

        [Field("EntityLook")]
        private string m_look;

        public EntityLook EntityLook
        {
            get;
            set;
        }

        [Property("TitleId", NotNull = true, Default = "0")]
        public uint TitleId
        {
            get;
            set;
        }

        [Property("TitleParam", NotNull = true, Default = "")]
        public string TitleParam
        {
            get;
            set;
        }

        [Property("HasRecolor", NotNull = true, Default = "0")]
        public bool Recolor
        {
            get;
            set;
        }

        [Property("HasRename", NotNull = true, Default = "0")]
        public bool Rename
        {
            get;
            set;
        }

        #region Position

        [Property("MapId", NotNull = true)]
        public int MapId
        {
            get;
            set;
        }

        [Property("CellId", NotNull = true)]
        public ushort CellId
        {
            get;
            set;
        }

        [Property("Direction", NotNull = true)]
        public DirectionsEnum Direction
        {
            get;
            set;
        }

        #endregion

        #region Stats

        [Property("BaseHealth", NotNull = true)]
        public uint BaseHealth
        {
            get;
            set;
        }

        [Property("DamageTaken", NotNull = true)]
        public uint DamageTaken
        {
            get;
            set;
        }

        [Property("Strength", NotNull = true)]
        public int Strength
        {
            get;
            set;
        }

        [Property("Chance", NotNull = true)]
        public int Chance
        {
            get;
            set;
        }

        [Property("Vitality", NotNull = true)]
        public int Vitality
        {
            get;
            set;
        }

        [Property("Wisdom", NotNull = true)]
        public int Wisdom
        {
            get;
            set;
        }

        [Property("Intelligence", NotNull = true)]
        public int Intelligence
        {
            get;
            set;
        }

        [Property("Agility", NotNull = true)]
        public int Agility
        {
            get;
            set;
        }

        #endregion

        #region Points

        [Property("Experience", NotNull = true, Default = "0")]
        public long Experience
        {
            get;
            set;
        }

        [Property("EnergyMax", NotNull = true, Default = "10000")]
        public uint EnergyMax
        {
            get;
            set;
        }

        [Property("Energy", NotNull = true, Default = "10000")]
        public uint Energy
        {
            get;
            set;
        }

        [Property("StatsPoints", NotNull = true, Default = "0")]
        public int StatsPoints
        {
            get;
            set;
        }

        [Property("SpellsPoints", NotNull = true, Default = "0")]
        public int SpellsPoints
        {
            get;
            set;
        }

        #endregion

        [Nested]
        public Restrictions Restrictionses
        {
            get;
            set;
        }

        public static CharacterRecord FindById(int characterId)
        {
            return FindByPrimaryKey(characterId);
        }

        public static CharacterRecord FindByName(string characterName)
        {
            return FindOne(NHibernate.Criterion.Restrictions.Eq("Name", characterName));
        }

        public static bool DoesNameExists(string name)
        {
            return Exists(NHibernate.Criterion.Restrictions.Eq("Name", name));
        }

        public static int GetCount()
        {
            return Count();
        }

        protected override bool BeforeSave(System.Collections.IDictionary state)
        {
            m_look = EntityLook.ConvertToString();

            return base.BeforeSave(state);
        }

        protected override void OnLoad(object id)
        {
            EntityLook = m_look.ToEntityLook();

            base.OnLoad(id);
        }
    }
}
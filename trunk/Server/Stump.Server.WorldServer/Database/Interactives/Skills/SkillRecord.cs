using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Database.Interactives.Skills
{
    [ActiveRecord("interactives_skills", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class SkillRecord : WorldBaseRecord<SkillRecord>
    {
        public const int DEFAULT_TEMPLATE = 184;

        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property]
        public uint Duration
        {
            get;
            set;
        }

        public abstract SkillTemplate Template // determin the name
        {
            get;
        }

        public abstract Skill GenerateSkill(int id, InteractiveObject interactiveObject);
    }
}
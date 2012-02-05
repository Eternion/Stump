using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Database.Interactives.Skills
{
    [ActiveRecord("interactives_skills", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class SkillRecord : WorldBaseRecord<SkillRecord>
    {
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

        public abstract int SkillId // determin the name
        {
            get;
        }

        public abstract Skill GenerateSkill(int id, InteractiveObject interactiveObject);
    }
}
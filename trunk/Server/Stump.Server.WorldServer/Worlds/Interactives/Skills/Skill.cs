using NHibernate.SqlCommand;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Interactives.Skills
{
    public abstract class Skill
    {
        protected Skill(int id, SkillTemplate template, InteractiveObject interactiveObject)
        {
            Id = id;
            Template = template;
            InteractiveObject = interactiveObject;
        }

        public int Id
        {
            get;
            private set;
        }

        public SkillTemplate Template
        {
            get;
            private set;
        }

        public InteractiveObject InteractiveObject
        {
            get;
            private set;
        }

        public abstract bool IsEnabled(Character character);
        public abstract void Execute(Character character);

        public InteractiveElementSkill GetInteractiveElementSkill()
        {
            return new InteractiveElementSkill(Template.SkillId, Id);
        }
    }
}
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public abstract class Skill
    {
        protected Skill(int id, SkillRecord record, InteractiveObject interactiveObject)
        {
            Id = id;
            Record = record;
            InteractiveObject = interactiveObject;
        }

        public int Id
        {
            get;
            private set;
        }

        public SkillRecord Record
        {
            get;
            private set;
        }

        public InteractiveObject InteractiveObject
        {
            get;
            private set;
        }

        public virtual uint GetDuration(Character character)
        {
            return Record.Duration;
        }

        public abstract bool IsEnabled(Character character);
        public abstract void Execute(Character character);

        public virtual void PostExecute(Character character)
        {
        }

        public InteractiveElementSkill GetInteractiveElementSkill()
        {
            return new InteractiveElementSkill(Record.Template.Id, Id);
        }
    }
}
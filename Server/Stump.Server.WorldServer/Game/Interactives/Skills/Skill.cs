using System;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public abstract class Skill
    {
        protected Skill(int id, InteractiveSkillTemplate skillTemplate, InteractiveObject interactiveObject)
        {
            Id = id;
            SkillTemplate = skillTemplate;
            InteractiveObject = interactiveObject;
        }

        public int Id
        {
            get;
            private set;
        }

        public InteractiveSkillTemplate SkillTemplate
        {
            get;
            set;
        }

        public InteractiveObject InteractiveObject
        {
            get;
            private set;
        }

        public virtual int GetDuration(Character character)
        {
            return 0;
        }

        public virtual bool IsEnabled(Character character)
        {
            return !character.IsGhost();
        }

        public abstract int StartExecute(Character character);

        public virtual void EndExecute(Character character)
        {
        }

        public InteractiveElementSkill GetInteractiveElementSkill()
        {
            return new InteractiveElementSkill(SkillTemplate.Id, Id);
        }
    }
}
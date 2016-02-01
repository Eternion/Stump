using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;

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
        }

        public InteractiveSkillTemplate SkillTemplate
        {
            get;
            set;
        }

        public InteractiveObject InteractiveObject
        {
            get;
        }

        public DateTime SkillEndTime
        {
            get;
            set;
        }

        public virtual int GetDuration(Character character) => 0;

        public virtual bool IsEnabled(Character character) => !character.IsGhost();

        public virtual int StartExecute(Character character)
        {
            SkillEndTime = DateTime.Now.AddMilliseconds(GetDuration(character));

            character.LastSkillUsed = this;
            return GetDuration(character);
        }

        public virtual void EndExecute(Character character)
        {
            character.LastSkillUsed = null;
        }

        public InteractiveElementSkill GetInteractiveElementSkill() => new InteractiveElementSkill(SkillTemplate.Id, Id);
    }
}
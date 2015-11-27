using Stump.Server.WorldServer.Database.Interactives;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public abstract class CustomSkill : Skill
    {
        protected CustomSkill(int id, InteractiveCustomSkillRecord record, InteractiveObject interactiveObject)
             : base(id, record.CustomTemplateId.HasValue ? 
                   InteractiveManager.Instance.GetSkillTemplate(record.CustomTemplateId.Value) : InteractiveManager.Instance.GetDefaultSkillTemplate(), interactiveObject)
        {
            Record = record;
        }

        public InteractiveCustomSkillRecord Record
        {
            get;
            private set;
        }
    }
}
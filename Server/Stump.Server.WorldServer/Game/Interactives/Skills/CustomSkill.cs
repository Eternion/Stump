using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

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

        public override bool AreConditionsFilled(Character character)
        {
            return Record.AreConditionsFilled(character);
        }

        public InteractiveCustomSkillRecord Record
        {
            get;
        }
    }
}
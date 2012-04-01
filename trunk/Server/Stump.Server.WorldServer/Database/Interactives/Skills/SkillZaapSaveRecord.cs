using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Database.Interactives.Skills
{
    [ActiveRecord(DiscriminatorValue = "Zaap")]
    public class SkillZaapSaveRecord : TemplateDependantSkill
    {
        public override int TemplateId
        {
            get
            {
                return DEFAULT_TEMPLATE;
            }
        }

        public override Skill GenerateSkill(int id, InteractiveObject interactiveObject)
        {
            return new SkillZaapTeleport(id, this, interactiveObject);
        }
    }
}
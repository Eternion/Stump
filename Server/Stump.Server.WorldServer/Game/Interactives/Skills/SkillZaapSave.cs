using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator((int)SkillTemplateEnum.BASE_44, typeof(Skill), typeof(int), typeof(InteractiveSkillTemplate), typeof(InteractiveObject))]
    [Discriminator("ZaapSave", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillZaapSave : Skill
    {
        public SkillZaapSave(int id, InteractiveSkillTemplate record, InteractiveObject interactiveObject)
            : base(id, record, interactiveObject)
        {
        }

        public SkillZaapSave(int id, InteractiveCustomSkillRecord record, InteractiveObject interactiveObject)
            : base(id, record.Template, interactiveObject)
        {
        }

        public override int StartExecute(Character character)
        {
            character.SetSpawnPoint(InteractiveObject.Map);

            return base.StartExecute(character);
        }
    }
}
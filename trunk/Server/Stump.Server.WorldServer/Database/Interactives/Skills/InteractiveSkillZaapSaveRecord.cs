using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Database
{
    public class InteractiveSkillZaapSaveRecordConfiguration : EntityTypeConfiguration<InteractiveSkillZaapSaveRecord>
    {
        public InteractiveSkillZaapSaveRecordConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("ZaapSave"));
        }
    }

    public partial class InteractiveSkillZaapSaveRecord : InteractiveSkillRecord
    {
        public override Skill GenerateSkill(int id, InteractiveObject interactiveObject)
        {
            return new SkillZaapTeleport(id, this, interactiveObject);
        }
    }
}
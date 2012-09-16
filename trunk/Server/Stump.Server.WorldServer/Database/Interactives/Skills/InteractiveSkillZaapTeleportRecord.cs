using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Database
{
    public class InteractiveSkillZaapTeleportRecordConfiguration : EntityTypeConfiguration<InteractiveSkillZaapTeleportRecord>
    {
        public InteractiveSkillZaapTeleportRecordConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("ZaapTeleport"));
        }
    }

    public class InteractiveSkillZaapTeleportRecord : SkillRecord
    {
        public override Skill GenerateSkill(int id, InteractiveObject interactiveObject)
        {
            return new SkillZaapTeleport(id, this, interactiveObject);
        }
    }
}
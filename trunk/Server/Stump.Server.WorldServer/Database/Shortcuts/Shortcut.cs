using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace Stump.Server.WorldServer.Database
{
    public class ShortcutConfiguration : EntityTypeConfiguration<Shortcut>
    {
        public ShortcutConfiguration()
        {
            ToTable("shortcuts");
            Map(x => x.Requires("Discriminator").HasValue("Base"));
        }
    }
    public abstract class Shortcut : WorldBaseRecord<Shortcut>
    {
        protected Shortcut()
        {
            
        }

        protected Shortcut(CharacterRecord owner, int slot)
        {
            OwnerId = owner.Id;
            Slot = slot;
        }

        public int Id
        {
            get;
            set;
        }

        public int OwnerId
        {
            get;
            set;
        }

        public int Slot
        {
            get;
            set;
        }

        public abstract DofusProtocol.Types.Shortcut GetNetworkShortcut();
    }
}
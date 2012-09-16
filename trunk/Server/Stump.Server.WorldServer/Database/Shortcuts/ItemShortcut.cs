using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Database
{
    public class ItemShortcutConfiguration : EntityTypeConfiguration<ItemShortcut>
    {
        public ItemShortcutConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("Item"));
        }
    }
    public class ItemShortcut : Shortcut
    {
        public ItemShortcut()
        {
            
        }

        public ItemShortcut(CharacterRecord owner, int slot, int itemTemplateId, int itemGuid)
            : base(owner, slot)
        {
            ItemTemplateId = itemTemplateId;
            ItemGuid = itemGuid;
        }

        public int ItemTemplateId
        {
            get;
            set;
        }

        public int ItemGuid
        {
            get;
            set;
        }

        public override DofusProtocol.Types.Shortcut GetNetworkShortcut()
        {
            return new ShortcutObjectItem(Slot, ItemGuid, ItemTemplateId);
        }
    }
}
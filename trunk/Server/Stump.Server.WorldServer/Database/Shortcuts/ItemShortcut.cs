using System;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Database.Characters;
using DofusItemShortcut = Stump.DofusProtocol.Types.ShortcutObjectItem;

namespace Stump.Server.WorldServer.Database.Shortcuts
{
    // not used
    [ActiveRecord(DiscriminatorValue = "Item")]
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

        [Property]
        public int ItemTemplateId
        {
            get;
            set;
        }

        [Property]
        public int ItemGuid
        {
            get;
            set;
        }

        public override DofusProtocol.Types.Shortcut GetNetworkShortcut()
        {
            return new DofusItemShortcut(Slot, ItemGuid, ItemTemplateId);
        }
    }
}
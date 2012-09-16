using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database
{
    public class UseItemReplyConfiguration : EntityTypeConfiguration<UseItemReply>
    {
        public UseItemReplyConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("UseItem"));

            Property(x => x.ItemId).HasColumnName("UseItem_Item");
            Property(x => x.Amount).HasColumnName("UseItem_Amount");
        }
    }

    public class UseItemReply : Npcs.NpcReply
    {
        private ItemTemplate m_itemTemplate;

        public int ItemId
        {
            get;
            set;
        }

        public ItemTemplate Item
        {
            get { return m_itemTemplate ?? (m_itemTemplate = ItemManager.Instance.TryGetTemplate(ItemId)); }
            set
            {
                m_itemTemplate = value;
                ItemId = value.Id;
            }
        }

        [Property("UseItem_Amount")]
        public uint Amount
        {
            get;
            set;
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            var item = character.Inventory.TryGetItem(Item);

            if (item == null)
                return false;

            character.Inventory.RemoveItem(item, Amount);
            character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, Amount, item.Template.Id);

            return true;
        }
    }
}
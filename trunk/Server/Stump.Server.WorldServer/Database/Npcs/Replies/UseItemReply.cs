using System.Data.Entity.ModelConfiguration;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
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

    public class UseItemReply : NpcReply
    {
        private ItemTemplate m_itemTemplate;

        public UseItemReply(NpcReplyRecord record) : base(record)
        {
        }

        /// <summary>
        /// Parameter 0
        /// </summary>
        public int ItemId
        {
            get { return GetParameter<int>(0); }
            set { SetParameter(0, value); }
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

        /// <summary>
        /// Parameter 1
        /// </summary>
        public uint Amount
        {
            get { return GetParameter<uint>(1); }
            set { SetParameter(1, value); }
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            PlayerItem item = character.Inventory.TryGetItem(Item);

            if (item == null)
                return false;

            character.Inventory.RemoveItem(item, Amount);
            character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, Amount,
                                             item.Template.Id);

            return true;
        }
    }
}
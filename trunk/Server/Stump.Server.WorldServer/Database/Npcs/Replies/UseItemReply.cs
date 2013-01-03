using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("UseItem", typeof(NpcReply), typeof(NpcReplyRecord))]
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
            get
            {
                return Record.GetParameter<int>(0);
            }
            set
            {
                Record.SetParameter(0, value);
            }
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
            get
            {
                return Record.GetParameter<uint>(1);
            }
            set
            {
                Record.SetParameter(1, value);
            }
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
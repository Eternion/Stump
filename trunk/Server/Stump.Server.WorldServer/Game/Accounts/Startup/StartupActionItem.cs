using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Startup;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Accounts.Startup
{
    public class StartupActionItem
    {
        private ItemTemplate m_itemTemplate;
        public StartupActionItemRecord m_record;

        public StartupActionItemRecord Record
        {
            get { return m_record; }
        }

        public StartupActionItem(StartupActionItemRecord record)
        {
            m_record = record;
        }

        public ItemTemplate ItemTemplate
        {
            get { return m_itemTemplate ?? (m_itemTemplate = ItemManager.Instance.TryGetTemplate(m_record.ItemTemplate)); }
            set
            {
                m_itemTemplate = value;
                m_record.ItemTemplate = value.Id;
            }
        }

        public int Amount
        {
            get { return m_record.Amount; }
            set { m_record.Amount = value; }
        }

        public bool MaxEffects
        {
            get { return m_record.MaxEffects; }
            set { m_record.MaxEffects = value; }
        }

        public void GiveTo(CharacterRecord record)
        {
            var effects = ItemManager.Instance.GenerateItemEffects(ItemTemplate, MaxEffects);

            var item = new PlayerItemRecord
            {
                Id = PlayerItemRecord.PopNextId(),
                OwnerId = record.Id,
                Template = ItemTemplate,
                Stack = Amount,
                Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                Effects = effects,
                New = true
            };

            item.Create();
        }

        public ObjectItemInformationWithQuantity GetObjectItemInformationWithQuantity()
        {
            return new ObjectItemInformationWithQuantity((short) ItemTemplate.Id, 0, false, ItemTemplate.Effects.Select(entry => entry.GetObjectEffect()).ToArray(), Amount);
        }
    }
}
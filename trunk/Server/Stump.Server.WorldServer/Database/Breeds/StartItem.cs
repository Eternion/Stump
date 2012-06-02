using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Breeds
{
    [ActiveRecord("breeds_items")]
    public class StartItem : WorldBaseRecord<LearnableSpell>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [BelongsTo("Breed")]
        public Breed Breed
        {
            get;
            set;
        }

        [Property("ItemId")]
        public int ItemId
        {
            get;
            set;
        }

        [Property("Amount")]
        public int Amount
        {
            get;
            set;
        }

        [Property("MaxEffects")]
        public bool MaxEffects
        {
            get;
            set;
        }

        public PlayerItemRecord GenerateItemRecord(CharacterRecord character)
        {
            var template = ItemManager.Instance.TryGetTemplate(ItemId);

            if (template == null)
            {
                throw new InvalidOperationException(string.Format("itemId {0} doesn't exists", ItemId));
            }

            var effects = ItemManager.Instance.GenerateItemEffects(template, MaxEffects);

            var record = new PlayerItemRecord()
            {
                Id = PlayerItemRecord.PopNextId(),
                OwnerId = character.Id,
                Template = template,
                Stack = Amount,
                Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                Effects = effects,
                New = true
            };

            return record;
        }
    }
}
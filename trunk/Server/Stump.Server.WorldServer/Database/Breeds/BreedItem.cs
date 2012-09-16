using System;
using System.Data.Entity.ModelConfiguration;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database
{
    public class BreedItemConfiguration : EntityTypeConfiguration<BreedItem>
    {
        public BreedItemConfiguration()
        {
            ToTable("breeds_items");
        }
    }

    public class BreedItem
    {
        // Primitive properties

        public int Id
        {
            get;
            set;
        }
        public int BreedId
        {
            get;
            set;
        }
        public int ItemId
        {
            get;
            set;
        }
        public int Amount
        {
            get;
            set;
        }
        public bool MaxEffects
        {
            get;
            set;
        }

        // Navigation properties

        public virtual Breed Breed
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
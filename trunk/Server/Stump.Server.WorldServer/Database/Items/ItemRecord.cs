using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Effects.Instances;

namespace Stump.Server.WorldServer.Database.Items
{
    [ActiveRecord("items")]
    public class ItemRecord : WorldBaseRecord<ItemRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Guid")]
        public int Guid
        {
            get;
            set;
        }

        [Property("ItemId", NotNull = true)]
        public int ItemId
        {
            get;
            set;
        }

        [BelongsTo("InventoryId")]
        public InventoryRecord Inventory
        {
            get;
            set;
        }

        [Property("Stack", NotNull = true, Default = "0")]
        public int Stack
        {
            get;
            set;
        }

        [Property("Position", NotNull = true, Default = "63")]
        public CharacterInventoryPositionEnum Position
        {
            get;
            set;
        }

        [Property("Effects", ColumnType = "Serializable")]
        public List<EffectBase> Effects
        {
            get;
            set;
        }
    }
}

using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Character;
using Stump.Database.WorldServer.Item;

namespace Stump.Database.WorldServer.Storage
{
    [ActiveRecord("sellbags")]
    public sealed class SellBagRecord : WorldBaseRecord<SellBagRecord>
    {
        private IList<PricedItemRecord> m_items;

        [PrimaryKey(PrimaryKeyType.Foreign, "CharacterId")]
        public uint CharacterId { get; set; }

        [OneToOne]
        public CharacterRecord Character { get; set; }

        [Property("Capacity")]
        public uint Capacity { get; set; }

        [HasMany(typeof (PricedItemRecord), Table = "sellbags_items", ColumnKey = "CharacterId")]
        public IList<PricedItemRecord> PricedItems
        {
            get { return m_items ?? new List<PricedItemRecord>(); }
            set { m_items = value; }
        }
    }
}
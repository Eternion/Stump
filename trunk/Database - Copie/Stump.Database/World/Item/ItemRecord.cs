
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.WorldServer.Item
{
    [ActiveRecord("items"), JoinedBase]
    public class ItemRecord : WorldBaseRecord<ItemRecord>
    {
        private IList<byte[]> m_effects;

        [PrimaryKey(PrimaryKeyType.Native, "Guid")]
        public long Guid { get; set; }

        [Property("ItemId", NotNull = true)]
        public int ItemId { get; set; }

        [Property("Stack", NotNull = true, Default = "0")]
        public uint Stack { get; set; }

        [Property("Position", NotNull = true, Default = "63")]
        public CharacterInventoryPositionEnum Position { get; set; }

        [HasMany(typeof (ItemEffectRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<byte[]> Effects
        {
            get { return m_effects ?? new List<byte[]>(); }
            set { m_effects = value; }
        }
    }
}
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Database.Items
{
    [ActiveRecord("items")]
    public class ItemRecord : AssignedWorldRecord<ItemRecord>
    {
        public ItemRecord()
        {
            m_serializedEffects = new byte[0];
        }

        [Property("ItemId", NotNull = true)]
        public int ItemId
        {
            get;
            set;
        }

        [Property("OwnerId")]
        public int OwnerId
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

        private byte[] m_serializedEffects;

        [Property("Effects", NotNull = true)]
        private byte[] SerializedEffects
        {
            get { return m_serializedEffects; }
            set
            {
                m_serializedEffects = value;
                Effects = EffectManager.Instance.DeserializeEffects(m_serializedEffects);
            }
        }

        public List<EffectBase> Effects
        {
            get;
            set;
        }

        protected override bool BeforeSave(System.Collections.IDictionary state)
        {
            SerializedEffects = EffectManager.Instance.SerializeEffects(Effects);

            return base.BeforeSave(state);
        }

        public static ItemRecord[] FindAllByOwner(int ownerId)
        {
            return FindAll(Restrictions.Eq("OwnerId", ownerId));
        }
    }
}
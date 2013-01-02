using System.Collections.Generic;
using Stump.ORM;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Items
{
    public abstract class ItemRecord<T> : AutoAssignedRecord<T>
    {
        private List<EffectBase> m_effects;
        private byte[] m_serializedEffects;
        private ItemTemplate m_template;

        public ItemRecord()
        {
            m_serializedEffects = new byte[0];
        }

        public int Id
        {
            get;
            set;
        }

        protected int ItemId
        {
            get;
            set;
        }

        [Ignore]
        public ItemTemplate Template
        {
            get { return m_template ?? (m_template = ItemManager.Instance.TryGetTemplate(ItemId)); }
            set
            {
                m_template = value;
                ItemId = value.Id;
            }
        }

        public int Stack
        {
            get;
            set;
        }

        private byte[] SerializedEffects
        {
            get { return m_serializedEffects; }
            set
            {
                m_serializedEffects = value;
                m_effects = EffectManager.Instance.DeserializeEffects(m_serializedEffects);
            }
        }

        [Ignore]
        public List<EffectBase> Effects
        {
            get { return m_effects; }
            set { m_effects = value; }
        }

        public override void BeforeSave(bool insert)
        {
            base.BeforeSave(insert);
            m_serializedEffects = EffectManager.Instance.SerializeEffects(Effects);
        }
    }
}
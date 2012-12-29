using System.Collections.Generic;
using Stump.ORM;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Items
{
    public abstract class ItemRecord : ISaveIntercepter
    {
        private List<EffectBase> m_effects;
        private byte[] m_serializedEffects;
        private ItemTemplate m_template;

        public ItemRecord()
        {
            m_serializedEffects = new byte[0];
        }

        protected int ItemId
        {
            get;
            set;
        }

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

        #region ISaveIntercepter Members

        public void BeforeSave(bool insert)
        {
            m_serializedEffects = EffectManager.Instance.SerializeEffects(Effects);
        }

        #endregion
    }
}
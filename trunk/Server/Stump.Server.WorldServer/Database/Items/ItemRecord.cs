using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Items
{
    public abstract class ItemRecord : ISaveIntercepter
    {
        public ItemRecord()
        {
            m_serializedEffects = new byte[0];
        }

        protected int ItemId
        {
            get;
            set;
        }

        private ItemTemplate m_template;

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

        private byte[] m_serializedEffects;

        private byte[] SerializedEffects
        {
            get { return m_serializedEffects; }
            set
            {
                m_serializedEffects = value;
                m_effects = EffectManager.Instance.DeserializeEffects(m_serializedEffects);
            }
        }

        private List<EffectBase> m_effects;

        [Ignore]
        public List<EffectBase> Effects
        {
            get { return m_effects; }
            set { m_effects = value; }
        }

        public void BeforeSave(bool insert)
        {
            m_serializedEffects = EffectManager.Instance.SerializeEffects(Effects);
        }
    }
}
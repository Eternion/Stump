using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Items
{
    public interface IItemRecord
    {
        ItemTemplate Template
        {
            get;
            set;
        }

        int Stack
        {
            get;
            set;
        }

        List<EffectBase> Effects
        {
            get;
            set;
        }

        int Id
        {
            get;
            set;
        }

        void AssignIdentifier();

        void Save();
        void Create();
        void Delete();
    }

    [ActiveRecord("items", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class ItemRecord<T> : AssignedWorldRecord<T>, IItemRecord where T : ItemRecord<T>
    {
        public ItemRecord()
        {
            m_serializedEffects = new byte[0];
        }

        [Property("Item", NotNull = true)]
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

        [Property("Stack", NotNull = true, Default = "0")]
        public int Stack
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
                m_effects = EffectManager.Instance.DeserializeEffects(m_serializedEffects);
            }
        }

        private List<EffectBase> m_effects;

        public List<EffectBase> Effects
        {
            get { return m_effects; }
            set { m_effects = value; }
        }

        protected override bool OnFlushDirty(object id, System.Collections.IDictionary previousState, System.Collections.IDictionary currentState, NHibernate.Type.IType[] types)
        {
            m_serializedEffects = (byte[])(currentState["SerializedEffects"] = EffectManager.Instance.SerializeEffects(Effects));

            return base.OnFlushDirty(id, previousState, currentState, types);
        }
    }
}
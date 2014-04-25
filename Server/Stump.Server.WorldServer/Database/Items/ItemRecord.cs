using System.Collections.Generic;
using System.Linq;
using Stump.ORM;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Items
{
    public interface IItemRecord
    {
        int Id
        {
            get;
            set;
        }

        ItemTemplate Template
        {
            get;
            set;
        }

        uint Stack
        {
            get;
            set;
        }

        List<EffectBase> Effects
        {
            get;
            set;
        }

        bool IsNew
        {
            get;
            set;
        }

        bool IsDirty
        {
            get;
            set;
        }

        void AssignIdentifier();
    }

    public abstract class ItemRecord<T> : AutoAssignedRecord<T>, IItemRecord
    {
        private List<EffectBase> m_effects;
        private byte[] m_serializedEffects;
        private ItemTemplate m_template;

        protected ItemRecord()
        {
            m_serializedEffects = new byte[0];
        }

        private int m_itemId;

        public int ItemId
        {
            get { return m_itemId; }
            set { m_itemId = value;
                m_template = null;
                IsDirty = true;
            }
        }

        [Ignore]
        public ItemTemplate Template
        {
            get { return m_template ?? (m_template = ItemManager.Instance.TryGetTemplate(ItemId)); }
            set
            {
                m_template = value;
                ItemId = value.Id;
                IsDirty = true;
            }
        }

        public uint Stack
        {
            get { return m_stack; }
            set
            {
                m_stack = value;
                IsDirty = true;
            }
        }

        public byte[] SerializedEffects
        {
            get { return m_serializedEffects; }
            set
            {
                m_serializedEffects = value;
                m_effects = EffectManager.Instance.DeserializeEffects(m_serializedEffects);
                IsDirty = true;
            }
        }

        [Ignore]
        public List<EffectBase> Effects
        {
            get { return m_effects; }
            set
            {
                m_effects = value; IsDirty = true;
            }
        }

        private bool m_isDirty;
        private uint m_stack;

        [Ignore]
        public bool IsDirty
        {
            get
            {
                return m_isDirty || m_effects.Any(x => x.IsDirty);
            }
            set { m_isDirty = value; }
        }

        public override void BeforeSave(bool insert)
        {
            base.BeforeSave(insert);
            m_serializedEffects = EffectManager.Instance.SerializeEffects(Effects);
            IsDirty = false;
            foreach (var effectBase in Effects)
            {
                effectBase.IsDirty = false;
            }
        }
    }
}
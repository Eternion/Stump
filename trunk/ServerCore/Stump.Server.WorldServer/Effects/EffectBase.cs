
using System;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using EffectBaseEx = Stump.DofusProtocol.D2oClasses.EffectInstance;

namespace Stump.Server.WorldServer.Effects
{
    public enum EffectGenerationContext
    {
        Item,
        Spell,
    }

    [Serializable]
    public class EffectBase
    {
        protected uint m_id;

        [NonSerialized]
        protected EffectTemplate m_template;

        public EffectBase(uint id)
        {
            m_id = id;
            m_template = EffectManager.GetTemplate(id);
        }

        public EffectBase(EffectBaseEx effect)
        {
            m_id = effect.effectId;
            m_template = EffectManager.GetTemplate(m_id);
        }

        public virtual int ProtocoleId
        {
            get { return 76; }
        }

        public uint Id
        {
            get { return m_id; }
            protected set { m_id = value; }
        }

        public EffectsEnum EffectId
        {
            get { return (EffectsEnum) Id; }
        }

        public EffectTemplate Template
        {
            get { return m_template ?? (m_template = EffectManager.GetTemplate(m_id)); }
            protected set { m_template = value; }
        }

        public virtual object[] GetValues()
        {
            return new object[0];
        }

        public virtual EffectBase GenerateEffect(EffectGenerationContext context)
        {
            return this;
        }

        public virtual ObjectEffect ToNetworkEffect()
        {
            return new ObjectEffect((uint) EffectId);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
                return false;

            var effect = obj as EffectBase;

            return m_id == effect.m_id && ProtocoleId == effect.ProtocoleId;
        }

        public static bool operator ==(EffectBase effect1, EffectBase effect2)
        {
            if (ReferenceEquals(effect1, effect2))
                return true;

            if (effect2 == null)
                return false;

            return effect1.m_id == effect2.m_id && effect1.ProtocoleId == effect2.ProtocoleId;
        }

        public static bool operator !=(EffectBase effect1, EffectBase effect2)
        {
            return !(effect1 == effect2);
        }

        public static byte[] Serialize(EffectBase effect)
        {
            return FormatterExtensions.SerializableObjectToBytes(effect);
        }

        public static EffectBase DeSerialize(byte[] buffer)
        {
            var effect = FormatterExtensions.UnserializeBytesToObject<EffectBase>(buffer);
            effect.Template = EffectManager.GetTemplate(effect.Id);

            return effect;
        }

        public bool Equals(EffectBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.m_id == m_id && Equals(other.m_template, m_template);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int) ((m_id*397) ^ (m_template != null ? m_template.GetHashCode() : 0));
            }
        }
    }
}
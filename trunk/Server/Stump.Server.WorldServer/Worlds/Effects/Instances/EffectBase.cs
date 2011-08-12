using System;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Effects;

namespace Stump.Server.WorldServer.Worlds.Effects.Instances
{
    public enum EffectGenerationContext
    {
        Item,
        Spell,
    }

    [Serializable]
    public class EffectBase
    {
        protected short m_id;

        [NonSerialized]
        protected EffectRecord m_template;

        public EffectBase(short id)
        {
            m_id = id;
            m_template = EffectManager.GetTemplate(id);
        }

        public EffectBase(EffectInstance effect)
        {
            m_id = (short) effect.effectId;
            m_template = EffectManager.GetTemplate(m_id);
        }

        public virtual int ProtocoleId
        {
            get { return 76; }
        }

        public short Id
        {
            get { return m_id; }
            protected set { m_id = value; }
        }

        public EffectsEnum EffectId
        {
            get { return (EffectsEnum) Id; }
        }

        public EffectRecord Template
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

        public virtual ObjectEffect GetObjectEffect()
        {
            return new ObjectEffect(Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EffectBase)) return false;
            return Equals((EffectBase) obj);
        }

        public static bool operator ==(EffectBase left, EffectBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EffectBase left, EffectBase right)
        {
            return !Equals(left, right);
        }

        public static byte[] Serialize(EffectBase effect)
        {
            return effect.ToBinary();
        }

        public static EffectBase DeSerialize(byte[] buffer)
        {
            var effect = buffer.ToObject<EffectBase>();
            effect.Template = EffectManager.GetTemplate(effect.Id);

            return effect;
        }

        public bool Equals(EffectBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.m_id == m_id;
        }

        public override int GetHashCode()
        {
            return m_id.GetHashCode();
        }
    }
}
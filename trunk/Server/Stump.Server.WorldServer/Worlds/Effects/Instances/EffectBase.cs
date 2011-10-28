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
        [NonSerialized]
        protected EffectTemplate m_template;

        public virtual int ProtocoleId
        {
            get { return 76; }
        }

        public EffectBase(short id)
        {
            Id = id;
            m_template = EffectManager.Instance.GetTemplate(id);
        }

        public EffectBase(short id, int targetId, int duration, int random, int modificator, bool trigger, bool hidden, uint zoneSize, uint zoneShape)
        {
            Id = id;
            Targets = (SpellTargetType) targetId;
            Duration = duration;
            Random = random;
            Modificator = modificator;
            Trigger = trigger;
            Hidden = hidden;
            ZoneSize = zoneSize;
            ZoneShape = zoneShape;
        }

        public EffectBase(EffectInstance effect)
        {
            Id = (short) effect.effectId;
            m_template = EffectManager.Instance.GetTemplate(Id);

            Targets = (SpellTargetType) effect.targetId;
            Duration = effect.duration;
            Random = effect.random;
            Modificator = effect.modificator;
            Trigger = effect.trigger;
            Hidden = effect.hidden;
            ZoneSize = effect.zoneSize;
            ZoneShape = effect.zoneShape;
        }

        public short Id
        {
            get;
            protected set;
        }

        public EffectsEnum EffectId
        {
            get { return (EffectsEnum) Id; }
        }

        public EffectTemplate Template
        {
            get
            {
                return m_template ?? ( m_template = EffectManager.Instance.GetTemplate(Id) );
            }
            protected set { m_template = value; }
        }

        public SpellTargetType Targets
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }

        public int Random
        {
            get;
            set;
        }

        public int Modificator
        {
            get;
            set;
        }

        public bool Trigger
        {
            get;
            set;
        }

        public bool Hidden
        {
            get;
            set;
        }

        public uint ZoneSize
        {
            get;
            set;
        }

        public uint ZoneShape
        {
            get;
            set;
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

        public static byte[] Serialize(EffectBase effect)
        {
            return effect.ToBinary();
        }

        public static EffectBase DeSerialize(byte[] buffer)
        {
            return buffer.ToObject<EffectBase>();
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

        public bool Equals(EffectBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
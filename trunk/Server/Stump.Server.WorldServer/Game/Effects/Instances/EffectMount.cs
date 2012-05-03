using System;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Effects.Instances
{
    [Serializable]
    public class EffectMount : EffectBase
    {
        protected double m_date;
        protected short m_modelId;
        protected int m_mountId;

        public EffectMount()
        {
            
        }

        public EffectMount(EffectMount copy)
            : this (copy.Id, copy.m_mountId, copy.m_date, copy.m_modelId, copy)
        {
            
        }

        public EffectMount(short id, int mountid, double date, int modelid, EffectBase effect)
            : base(id, effect)
        {
            m_mountId = mountid;
            m_date = date;
            m_modelId = (short) modelid;
        }

        public EffectMount(EffectInstanceMount effect)
            : base(effect)
        {
           m_mountId = (int) effect.mountId;
           m_date = effect.date;
           m_modelId = (short) effect.modelId;
        }

        public override int ProtocoleId
        {
            get { return 179; }
        }

        public override byte SerializationIdenfitier
        {
            get
            {
                return 9;
            }
        }

        public override object[] GetValues()
        {
            return new object[] {m_mountId, m_date, m_modelId};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectMount(Id, m_mountId, m_date, m_modelId);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceMount()
            {
                effectId = (uint)Id,
                targetId = (int)Targets,
                delay = Delay,
                duration = Duration,
                group = Group,
                random = Random,
                modificator = Modificator,
                trigger = Trigger,
                hidden = Hidden,
                rawZone = RawZone,
                modelId = (uint) m_modelId,
                date = (float) m_date,
                mountId =  (uint) m_mountId
            };
        }
        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectMount(this);
        }
        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);

            writer.Write(m_mountId);
            writer.Write(m_date);
            writer.Write(m_modelId);
        }

        protected override void InternalDeserialize(ref System.IO.BinaryReader reader)
        {
            base.InternalDeserialize(ref reader);

            m_mountId = reader.ReadInt16();
            m_date = reader.ReadInt32();
            m_modelId = reader.ReadInt16();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectMount))
                return false;
            var b = obj as EffectMount;
            return base.Equals(obj) && m_mountId == b.m_mountId && m_date == b.m_date && m_modelId == b.m_modelId;
        }

        public static bool operator ==(EffectMount a, EffectMount b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectMount a, EffectMount b)
        {
            return !(a == b);
        }

        public bool Equals(EffectMount other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_date.Equals(m_date) && other.m_modelId == m_modelId &&
                   other.m_mountId == m_mountId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ m_date.GetHashCode();
                result = (result*397) ^ m_modelId;
                result = (result*397) ^ m_mountId;
                return result;
            }
        }
    }
}
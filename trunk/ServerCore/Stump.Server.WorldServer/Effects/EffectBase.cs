// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.Enums;
using EffectBaseEx = Stump.DofusProtocol.D2oClasses.EffectInstance;

namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectBase
    {
        protected int m_id;

        [NonSerialized]
        protected EffectTemplate m_template;

        public EffectBase(int id)
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

        public int Id
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

        public virtual EffectBase GenerateEffect()
        {
            return this;
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
            return FormatterHelper.SerializableObjectToBytes(effect);
        }

        public static EffectBase DeSerialize(byte[] buffer)
        {
            var effect = FormatterHelper.UnserializeBytesToObject<EffectBase>(buffer);
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
                return (m_id*397) ^ (m_template != null ? m_template.GetHashCode() : 0);
            }
        }
    }
}
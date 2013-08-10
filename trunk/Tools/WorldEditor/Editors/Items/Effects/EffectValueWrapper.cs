#region License GNU GPL

// EffectValueWrapper.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using Stump.DofusProtocol.D2oClasses;

namespace WorldEditor.Editors.Items.Effects
{
    public class EffectValueWrapper : EffectWrapper
    {
        private readonly EffectInstanceInteger m_wrappedEffect;

        public EffectValueWrapper(EffectInstanceInteger wrappedEffect) : base(wrappedEffect)
        {
            m_wrappedEffect = wrappedEffect;
        }

        public int Value
        {
            get
            {
                return m_wrappedEffect.value;
            }
            set
            {
                if (value == m_wrappedEffect.value) return;
                m_wrappedEffect.value = value;
                OnPropertyChanged("Description");
            }
        }

        public override int ParametersCount
        {
            get { return 1; }
        }

        public override object this[int index]
        {
            get
            {
                if (index == 0)
                    return Value;

                return null;
            }
            set
            {
                if (index == 0)
                    Value = (int) value;
            }
        }

        public override object[] Parameters
        {
            get { return new object[] {Value}; }
        }
    }
}
#region License GNU GPL

// EffectDiceWrapper.cs
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
    public class EffectDiceWrapper : EffectValueWrapper
    {
        private readonly EffectInstanceDice m_effect;

        public EffectDiceWrapper(EffectInstanceDice effect)
            : base(effect)
        {
            m_effect = effect;
        }

        public uint DiceSide
        {
            get
            {
                return m_effect.diceSide;
            }
            set
            {
                if (value == m_effect.diceSide) return;
                m_effect.diceSide = value;
                OnPropertyChanged("Description");
            }
        }

        public uint DiceNum
        {
            get
            {
                return m_effect.diceNum;
            }
            set
            {
                if (value == m_effect.diceSide) return;
                m_effect.diceNum = value;
                OnPropertyChanged("Description");
            }
        }

        public override int ParametersCount
        {
            get { return 3; }
        }

        public override object[] Parameters
        {
            get { return new object[] {DiceNum, DiceSide, Value}; }
        }

        public override object this[int index]
        {
            get
            {
                if (index == 0)
                    return DiceNum;
                if (index == 1)
                    return DiceSide;
                if (index == 2)
                    return Value;

                return null;
            }
            set
            {
                if (index == 0)
                    DiceNum = (uint) (int) value;
                if (index == 1)
                    DiceSide = (uint) (int) value;
                if (index == 2)
                    Value = (int) value;
            }
        }
    }
}
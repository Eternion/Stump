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
using Stump.DofusProtocol.Enums;
using EffectTemplateEx = Stump.DofusProtocol.D2oClasses.Effect;

namespace Stump.Server.WorldServer.Effects
{
    public class EffectTemplate
    {
        private readonly uint m_category;
        private readonly int m_characteristic;
        private readonly int m_id;
        private readonly string m_operator;

        public EffectTemplate(EffectTemplateEx effect)
        {
            m_id = effect.id;
            m_category = effect.category;
            m_operator = effect.@operator;
            m_characteristic = effect.characteristic;
        }

        public int Id
        {
            get { return m_id; }
        }

        public EffectsEnum EffectId
        {
            get { return (EffectsEnum) Id; }
        }

        public uint Category
        {
            get { return m_category; }
        }

        public int Characteristic
        {
            get { return m_characteristic; }
        }

        public string @Operator
        {
            get { return m_operator; }
        }
    }
}
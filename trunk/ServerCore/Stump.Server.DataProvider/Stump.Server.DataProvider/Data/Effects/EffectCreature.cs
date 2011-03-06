//// /*************************************************************************
////  *
////  *  Copyright (C) 2010 - 2011 Stump Team
////  *
////  *  This program is free software: you can redistribute it and/or modify
////  *  it under the terms of the GNU General Public License as published by
////  *  the Free Software Foundation, either version 3 of the License, or
////  *  (at your option) any later version.
////  *
////  *  This program is distributed in the hope that it will be useful,
////  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
////  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////  *  GNU General Public License for more details.
////  *
////  *  You should have received a copy of the GNU General Public License
////  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
////  *
////  *************************************************************************/
//using System;
//using Stump.DofusProtocol.Classes;
//using EffectCreateEx = Stump.DofusProtocol.D2oClasses.EffectInstanceCreature;


//namespace Stump.Server.WorldServer.Effects
//{
//    [Serializable]
//    public class EffectCreature : EffectBase
//    {
//        protected uint m_monsterfamily;

//        public EffectCreature(uint id, uint monsterfamily)
//            : base(id)
//        {
//            m_monsterfamily = monsterfamily;
//        }

//        public EffectCreature(EffectCreateEx effect)
//            : base(effect.effectId)
//        {
//            m_monsterfamily = effect.monsterFamilyId;
//        }

//        public override int ProtocoleId
//        {
//            get { return 71; }
//        }

//        public uint MonsterFamily
//        {
//            get { return m_monsterfamily; }
//        }

//        public override object[] GetValues()
//        {
//            return new object[] {(short) m_monsterfamily};
//        }

//        public override ObjectEffect ToNetworkEffect()
//        {
//            return new ObjectEffectCreature((uint) EffectId, MonsterFamily);
//        }

//        public override bool Equals(object obj)
//        {
//            if (!(obj is EffectCreature))
//                return false;
//            return base.Equals(obj) && m_monsterfamily == (obj as EffectCreature).m_monsterfamily;
//        }

//        public static bool operator ==(EffectCreature a, EffectCreature b)
//        {
//            if (ReferenceEquals(a, b))
//                return true;

//            if (((object) a == null) || ((object) b == null))
//                return false;

//            return a.Equals(b);
//        }

//        public static bool operator !=(EffectCreature a, EffectCreature b)
//        {
//            return !(a == b);
//        }

//        public bool Equals(EffectCreature other)
//        {
//            if (ReferenceEquals(null, other)) return false;
//            if (ReferenceEquals(this, other)) return true;
//            return base.Equals(other) && other.m_monsterfamily == m_monsterfamily;
//        }

//        public override int GetHashCode()
//        {
//            unchecked
//            {
//                return (base.GetHashCode()*397) ^ m_monsterfamily.GetHashCode();
//            }
//        }
//    }
//}
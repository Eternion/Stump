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

namespace Stump.Server.WorldServer.Breeds
{
    public class IopBreed : BaseBreed
    {
        public override PlayableBreedEnum Id
        {
            get
            {
                return PlayableBreedEnum.Iop;
            }
        }

        public override int StartHealthPoint
        {
            get { return 48; }
        }

        protected override void OnInitialize()
        {
            StartSpells.Add(SpellIdEnum.Intimidation, 65);
            StartSpells.Add(SpellIdEnum.Pressure, 66);
            StartSpells.Add(SpellIdEnum.Jump, 67);
        }
    }
}
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
using Stump.BaseCore.Framework.IO;
using Stump.Server.WorldServer.Entities;

namespace Stump.DofusProtocol.Classes.Custom
{
    public class CellData
    {

        public ushort Id
        {
            get;
            set;
        }

        public short Floor
        {
            get;
            set;
        }

        public byte LosMov
        {
            get;
            set;
        }

        public byte Speed
        {
            get;
            set;
        }

        public byte MapChangeData
        {
            get;
            set;
        }

        public uint MapId
        {
            get;
            set;
        }

        public bool Los
        {
            get { return (LosMov & 2) >> 1 == 1; }
        }

        public bool Mov
        {
            get { return (LosMov & 1) == 1 && !NonWalkableDuringFight && !FarmCell; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (LosMov & 4) >> 2 == 1; }
        }

        public bool Red
        {
            get { return (LosMov & 8) >> 3 == 1; }
        }

        public bool Blue
        {
            get { return (LosMov & 16) >> 4 == 1; }
        }

        public bool FarmCell
        {
            get { return (LosMov & 32) >> 5 == 1; }
        }

        public bool Visible
        {
            get { return (LosMov & 64) >> 6 == 1; }
        }

    }
}
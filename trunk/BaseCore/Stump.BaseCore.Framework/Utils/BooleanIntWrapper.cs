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

namespace Stump.BaseCore.Framework.Utils
{
    public static class BooleanIntWrapper
    {
        public static int SetFlag(int flag, byte offset, bool value)
        {
            if (offset >= 32)
                throw new ArgumentException("offset must be lesser than 32");

                return value ? (flag | (1 << offset)) : (flag & 255 - (1 << offset));
        }

        public static bool GetFlag(int flag, byte offset)
        {
            if (offset >= 32)
                throw new ArgumentException("offset must be lesser than 32");

            return (flag & (1 << offset)) != 0;
        }
    }
}
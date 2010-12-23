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

namespace Stump.BaseCore.Framework.IO
{
    public class ConsoleProcent
    {
        private int m_lastValue;

        public ConsoleProcent()
        {
            PositionX = Console.CursorLeft;
            PositionY = Console.CursorTop;
        }

        public ConsoleProcent(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public int PositionX
        {
            get;
            set;
        }

        public int PositionY
        {
            get;
            set;
        }

        public void Update(int value)
        {
            if (m_lastValue == value)
                return;

            m_lastValue = value;

            int oldX = Console.CursorLeft;
            int oldY = Console.CursorTop;

            Console.SetCursorPosition(PositionX, PositionY);
            Console.Write(value + "%");
            Console.SetCursorPosition(oldX, oldY);
        }

        public void End()
        {
            string cleaner = string.Empty;

            for (int i = 0; i < Console.BufferWidth - PositionX; i++)
            {
                cleaner += " ";
            }


            int oldX = Console.CursorLeft;
            int oldY = Console.CursorTop;

            Console.SetCursorPosition(PositionX, PositionY);
            Console.Write(cleaner);
            Console.SetCursorPosition(oldX, oldY);
        }
    }
}

using System;

namespace Stump.Core.IO
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
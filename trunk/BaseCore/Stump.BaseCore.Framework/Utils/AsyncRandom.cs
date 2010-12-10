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
using System.Threading;
using System.Threading.Tasks;

namespace Stump.BaseCore.Framework.Utils
{
    /// <summary>
    ///   Represent a safe-thread random class
    /// </summary>
    public sealed class AsyncRandom
    {
        private const int MaxIncrementerValue = 20;

        private static bool m_reverse;
        private static int m_incrementer;

        private readonly Random m_random;

        public AsyncRandom()
        {
            int ticks = m_reverse ? ~unchecked((int) DateTime.Now.Ticks) : unchecked((int) DateTime.Now.Ticks);
            m_reverse = !m_reverse;

            Interlocked.Increment(ref m_incrementer);

            if (m_incrementer >= MaxIncrementerValue) // is it safe ?
            {
                m_incrementer ^= m_incrementer; // means set to 0
            }

            m_random = new Random(
                DateTime.Now.Millisecond +
                ticks +
                m_incrementer +
                Thread.CurrentThread.ManagedThreadId +
                (Task.CurrentId != null ? (int) Task.CurrentId : 0));
        }

        public int NextInt()
        {
            return m_random.Next();
        }

        public int NextInt(int maxvalue)
        {
            return m_random.Next(maxvalue);
        }

        public int NextInt(int minvalue, int maxvalue)
        {
            return m_random.Next(minvalue, maxvalue);
        }

        public double NextDouble()
        {
            return m_random.NextDouble();
        }

        public void NextBytes(byte[] buffer)
        {
            m_random.NextBytes(buffer);
        }
    }
}
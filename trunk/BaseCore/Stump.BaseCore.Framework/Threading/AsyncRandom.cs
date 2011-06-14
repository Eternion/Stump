using System;
using System.Threading;
using Stump.BaseCore.Framework.Mathematics;

namespace Stump.BaseCore.Framework.Threading
{
    /// <summary>
    ///   Represent a Random class that generate a thread unique seed
    /// </summary>
    public sealed class AsyncRandom : FastRandom
    {
        private static int m_incrementer = 0;

        public AsyncRandom()
            : base (Environment.TickCount + Thread.CurrentThread.ManagedThreadId + m_incrementer)
        {
            Interlocked.Increment(ref m_incrementer);
        }

        public AsyncRandom(int seed)
            : base(seed)
        {
        }
    }
}
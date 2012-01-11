using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Stump.Tools.ClientPatcher.Patchs
{
    public class PatternFinder
    {
        public PatternFinder(byte[] buffer)
        {
            Buffer = buffer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int[] FindPattern(byte[] pattern, string mask)
        {
            var list = new List<int>();

            if (Buffer == null || Buffer.Length == 0)
                throw new ArgumentException("Buffer null or empty");

            if (pattern.Length != mask.Length)
                throw new ArgumentException("Pattern length different from mask length");

            for (int i = 0; i < Buffer.Length; i++)
            {
                if (CheckMask(i, pattern, mask))
                {
                    list.Add(i);
                }
            }

            return list.ToArray();
        }

        private bool CheckMask(int offset, byte[] pattern, string mask)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (mask[i] == '?')
                    continue;

                if (mask[i] == 'x' && pattern[i] != Buffer[offset + i])
                    return false;
            }

            return true;
        }

        public byte[] Buffer
        {
            get;
            private set;
        }
    }
}
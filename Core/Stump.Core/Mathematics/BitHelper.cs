using System;

namespace Stump.Core.Mathematics
{
    public static class BitHelper
    {
        public static byte SetBit(this byte flag, byte offset, bool value)
        {
            if (offset >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            return value ? (byte) (flag | (1 << offset)) : (byte)(flag & (0xFF ^ (1 << offset)));
        }

        public static short SetBit(this short flag, byte offset, bool value)
        {
            if (offset >= 16)
                throw new ArgumentException("offset must be lesser than 16");

            return unchecked(value ? (short)(flag | (1 << offset)) : (short)(flag & (0xFFFF ^ (1 << offset))));
        }

        public static int SetBit(this int flag, byte offset, bool value)
        {
            if (offset >= 32)
                throw new ArgumentException("offset must be lesser than 32");

            return unchecked(value ? flag | (1 << offset) : (int)(flag & (0xFFFFFFFF ^ (1 << offset))));
        }
        public static long SetBit(this long flag, byte offset, bool value)
        {
            if (offset >= 64)
                throw new ArgumentException("offset must be lesser than 64");

            return unchecked(value ? flag | (1 << offset) : (long)((ulong)flag & (0xFFFFFFFFFFFFFFFF ^ ((ulong)1 << offset))));
        }

        public static bool GetBit(this byte flag, byte offset)
        {
            if (offset >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            return (flag & (byte) (1 << offset)) != 0;
        }

        public static bool GetBit(this short flag, byte offset)
        {
            if (offset >= 16)
                throw new ArgumentException("offset must be lesser than 16");

            return (flag & (short) (1 << offset)) != 0;
        }

        public static bool GetBit(this int flag, byte offset)
        {
            if (offset >= 32)
                throw new ArgumentException("offset must be lesser than 32");

            return (flag & (1 << offset)) != 0;
        }

        public static bool GetBit(this long flag, byte offset)
        {
            if (offset >= 64)
                throw new ArgumentException("offset must be lesser than 64");

            return ((ulong)flag & ((ulong)1 << offset)) != 0;
        }


        public static byte FlipBit(this byte flag, byte offset)
        {
            if (offset >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            return (byte)(flag ^ (byte) (1 << offset));
        }

        public static short FlipBit(this short flag, byte offset)
        {
            if (offset >= 16)
                throw new ArgumentException("offset must be lesser than 16");

            return unchecked((short)(flag ^ (short) (1 << offset)));
        }

        public static int FlipBit(this int flag, byte offset)
        {
            if (offset >= 32)
                throw new ArgumentException("offset must be lesser than 32");

            return (flag ^ (1 << offset));
        }

        public static long FlipBit(this long flag, byte offset)
        {
            if (offset >= 64)
                throw new ArgumentException("offset must be lesser than 64");

            return (long)unchecked((ulong)flag ^ (ulong) (1 << offset));
        }

        public static byte ShiftBitsLeft(this byte flag, byte offset, byte shift)
        {
            if (offset >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            return (byte) (flag & (0xFF >> (8 - offset)) | (flag >> offset << (shift + offset)));
        }

        public static short ShiftBitsLeft(this short flag, byte offset, byte shift)
        {
            if (offset >= 16)
                throw new ArgumentException("offset must be lesser than 16");

            return unchecked ((short) (flag & (0xFFFF >> (8 - offset)) | (flag >> offset << (shift + offset))));
        }

        public static int ShiftBitsLeft(this int flag, byte offset, byte shift)
        {
            if (offset >= 32)
                throw new ArgumentException("offset must be lesser than 32");

            return unchecked ((int) (flag & (0xFFFFFFFF >> (8 - offset)) | (flag >> offset << (shift + offset))));
        }

        public static long ShiftBitsLeft(this long flag, byte offset, byte shift)
        {
            if (offset >= 64)
                throw new ArgumentException("offset must be lesser than 64");

            return unchecked ((long) ((ulong)flag & (0xFFFFFFFFFFFFFFFF >> (8 - offset)) | ((ulong)flag >> offset << (shift + offset))));
        }

         public static byte ShiftBitsRight(this byte flag, byte offset, byte shift)
        {
            if (offset >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            return (byte) (flag & (0xFF << (8 - offset)) | (flag << offset >> (shift + offset)));
        }

        public static short ShiftBitsRight(this short flag, byte offset, byte shift)
        {
            if (offset >= 16)
                throw new ArgumentException("offset must be lesser than 16");

            return unchecked ((short) (flag & (0xFFFF << (8 - offset)) | (flag << offset >> (shift + offset))));
        }

        public static int ShiftBitsRight(this int flag, byte offset, byte shift)
        {
            if (offset >= 32)
                throw new ArgumentException("offset must be lesser than 32");

            return unchecked ((int) (flag & (0xFFFFFFFF << (8 - offset)) | (flag << offset >> (shift + offset))));
        }

        public static long ShiftBitsRight(this long flag, byte offset, byte shift)
        {
            if (offset >= 64)
                throw new ArgumentException("offset must be lesser than 64");

            return unchecked ((long) ((ulong)flag & (0xFFFFFFFFFFFFFFFF << (8 - offset)) | ((ulong)flag << offset >> (shift + offset))));
        }

        public static byte SwapBits(this byte flag, byte offset1, byte offset2)
        {
            if (offset1 >= 8 || offset2 >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            var bit1 = flag.GetBit(offset1);
            return flag.SetBit(offset1, flag.GetBit(offset2)).SetBit(offset2, bit1);
        }
        public static short SwapBits(this short flag, byte offset1, byte offset2)
        {
            if (offset1 >= 16 || offset2 >= 16)
                throw new ArgumentException("offset must be lesser than 16");

            var bit1 = flag.GetBit(offset1);
            return flag.SetBit(offset1, flag.GetBit(offset2)).SetBit(offset2, bit1);
        }
        public static int SwapBits(this int flag, byte offset1, byte offset2)
        {
            if (offset1 >= 32 || offset2 >= 32)
                throw new ArgumentException("offset must be lesser than 32");

            var bit1 = flag.GetBit(offset1);
            return flag.SetBit(offset1, flag.GetBit(offset2)).SetBit(offset2, bit1);
        }
        public static long SwapBits(this long flag, byte offset1, byte offset2)
        {
            if (offset1 >= 64 || offset2 >= 64)
                throw new ArgumentException("offset must be lesser than 64");

            var bit1 = flag.GetBit(offset1);
            return flag.SetBit(offset1, flag.GetBit(offset2)).SetBit(offset2, bit1);
        }
    }
}
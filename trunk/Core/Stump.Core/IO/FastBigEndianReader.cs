#region License GNU GPL
// FastBigEndianReader.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.IO;
using System.Text;
using Stump.Core.Pool.New;

namespace Stump.Core.IO
{
    /// <summary>
    /// Much faster reader that only reads memory buffer
    /// </summary>
    public unsafe class FastBigEndianReader : IDataReader
    {
        private long m_position;
        private readonly byte[] m_buffer;
        private long m_maxPosition = -1;

        public byte[] Buffer
        {
            get { return m_buffer; }
        }

        public long Position
        {
            get { return m_position; }
            set
            {
                if (m_maxPosition > 0 && value > m_maxPosition)
                    throw new InvalidOperationException("Buffer overflow");
                
                m_position = value;
            }
        }

        public long MaxPosition
        {
            get { return m_maxPosition; }
            set { m_maxPosition = value; }
        }

        public long BytesAvailable
        {
            get { return (m_maxPosition > 0 ? m_maxPosition : m_buffer.Length) - Position; }
        }

        public FastBigEndianReader(byte[] buffer)
        {
            m_buffer = buffer;
        }

        public FastBigEndianReader(BufferSegment segment)
        {
            m_buffer = segment.Buffer.Array;
            Position = segment.Offset;
            m_maxPosition = segment.Offset + segment.Length;
        }

        public byte ReadByte()
        {
            fixed (byte* pbyte = &m_buffer[Position++])
            {
                return *pbyte;
            }
        }

        public sbyte ReadSByte()
        {
            fixed (byte* pbyte = &m_buffer[Position++])
            {
                return (sbyte)*pbyte;
            }
        }

        public short ReadShort()
        {
            var position = Position;
            Position += 2;
            fixed (byte* pbyte = &m_buffer[position])
            {
                return (short) ((*pbyte << 8) | (*(pbyte + 1)));
            }
        }

        public int ReadInt()
        {
            var position = Position;
            Position += 4;
            fixed (byte* pbyte = &m_buffer[position])
            {
                return ( *pbyte << 24 ) | ( *( pbyte + 1 ) << 16 ) | ( *( pbyte + 2 ) << 8 ) | ( *( pbyte + 3 ) );
            }
        }

        public long ReadLong()
        {
            var position = Position;
            Position += 8;
            fixed (byte* pbyte = &m_buffer[position])
            {
                int i1 = ( *pbyte << 24 ) | ( *( pbyte + 1 ) << 16 ) | ( *( pbyte + 2 ) << 8 ) | ( *( pbyte + 3 ) );
                int i2  = ( *( pbyte + 4 ) << 24 ) | ( *( pbyte + 5 ) << 16 ) | ( *( pbyte + 6 ) << 8 ) | ( *( pbyte + 7 ) );
                return (uint)i2 | ( (long)i1 << 32 );
            }
        }

        public ushort ReadUShort()
        {
            return (ushort)ReadShort();
        }

        public uint ReadUInt()
        {
            return (uint)ReadInt();
        }

        public ulong ReadULong()
        {
            return (ulong)ReadLong();
        }

        public byte[] ReadBytes(int n)
        {
            if (BytesAvailable < n)
                throw new InvalidOperationException("Buffer overflow");

            var dst = new byte[n];
            fixed (byte* pSrc = &m_buffer[Position], pDst = dst)
            {
                byte* ps = pSrc;
                byte* pd = pDst;

                // Loop over the count in blocks of 4 bytes, copying an integer (4 bytes) at a time:
                for (int i = 0; i < n / 4; i++)
                {
                    *( (int*)pd ) = *( (int*)ps );
                    pd += 4;
                    ps += 4;
                }

                // Complete the copy by moving any bytes that weren't moved in blocks of 4:
                for (int i = 0; i < n % 4; i++)
                {
                    *pd = *ps;
                    pd++;
                    ps++;
                }
            }

            Position += n;

            return dst;
        }

        public bool ReadBoolean()
        {
            return ReadByte() != 0;
        }

        public char ReadChar()
        {
            return (char)ReadShort();
        }

        public float ReadFloat()
        {
            int val = ReadInt();
            return *(float*)&val;
        }

        public double ReadDouble()
        {
            long val = ReadLong();
            return *(double*)&val;
        }

        public string ReadUTF()
        {
            ushort length = ReadUShort();

            byte[] bytes = ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        public string ReadUTFBytes(ushort len)
        {
            byte[] bytes = ReadBytes(len);
            return Encoding.UTF8.GetString(bytes);
        }

        public void Seek(int offset, SeekOrigin seekOrigin)
        {
            if (seekOrigin == SeekOrigin.Begin)
                Position = offset;
            else if (seekOrigin == SeekOrigin.End)
                Position = m_buffer.Length + offset;
            else if (seekOrigin == SeekOrigin.Current)
                Position += offset;
        }

        public void Seek(long offset, SeekOrigin seekOrigin)
        {
            if (seekOrigin == SeekOrigin.Begin)
                Position = offset;
            else if (seekOrigin == SeekOrigin.End)
                Position = m_buffer.Length + offset;
            else if (seekOrigin == SeekOrigin.Current)
                Position += offset;
        }

        public void SkipBytes(int n)
        {
            Position += n;
        }

        public void Dispose()
        {
            
        }
    }
}
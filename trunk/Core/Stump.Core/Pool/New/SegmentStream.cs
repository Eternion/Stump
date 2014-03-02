using System;
using System.IO;

// credits to wcell

namespace Stump.Core.Pool.New
{
    /// <summary>
    /// Similar to MemoryStream, but with an underlying BufferSegment.
    /// Will automatically free the old and get a new segment if its length was exceeded.
    /// </summary>
    public class SegmentStream : Stream
    {
        private int m_position;
        private BufferSegment m_segment;

        private int m_length;
        public SegmentStream(BufferSegment segment)
        {
            m_segment = segment;
            m_position = m_segment.Offset;
        }

        public BufferSegment Segment
        {
            get { return m_segment; }
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    m_position = m_segment.Offset + (int)offset;
                    break;
                case SeekOrigin.Current:
                    m_position += (int)offset;
                    break;
                case SeekOrigin.End:
                    m_position = m_segment.Offset + m_segment.Length - (int)offset;
                    break;
            }
            if (Position > m_segment.Length)
            {
                Position = m_segment.Length;
            }
            return m_position;
        }

        public override void SetLength(long value)
        {
            m_length = (int)value;
            if (m_position > m_length)
            {
                m_position = m_length + m_segment.Offset;
            }
            if (m_length > m_segment.Length)
            {
                EnsureCapacity(m_length);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, m_segment.Offset + m_segment.Length - m_position);
            Buffer.BlockCopy(m_segment.Buffer.Array, m_position, buffer, offset, count);
            m_position += count;
            return count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (m_position + count >= m_segment.Offset + m_segment.Length)
            {
                EnsureCapacity(m_position - m_segment.Offset + count);
            }
            Buffer.BlockCopy(buffer, offset, m_segment.Buffer.Array, m_position, count);
            m_position += count;
            m_length = Math.Max(m_length, m_position - m_segment.Offset);
        }

        public override int ReadByte()
        {
            return m_segment.Buffer.Array[m_position++];
        }

        public override void WriteByte(byte value)
        {
            if (m_position + 1 >= m_segment.Offset + m_segment.Length)
            {
                EnsureCapacity(m_position - m_segment.Offset + 1);
            }
            m_segment.Buffer.Array[m_position++] = value;
            m_length = Math.Max(m_length, m_position - m_segment.Offset);
        }

        private void EnsureCapacity(int size)
        {
            // return the old segment and get a new, bigger one
            var newSegment = BufferManager.GetSegment(size);
            m_segment.CopyTo(newSegment, m_length);
            m_position = m_position - m_segment.Offset + newSegment.Offset;

            m_segment.DecrementUsage();
            m_segment = newSegment;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return m_length; }
        }

        public override long Position
        {
            get { return m_position - m_segment.Offset; }
            set { m_position = (int)value + m_segment.Offset; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            m_segment.DecrementUsage();
        }
    }
}
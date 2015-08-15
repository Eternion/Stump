using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using NLog;
using Stump.Core.Collections;

namespace Stump.Core.Pool
{
    public class ArrayBuffer
    {
        private readonly byte[] m_array;
        private readonly BufferManager m_manager;

        internal ArrayBuffer(byte[] array)
        {
            m_array = array;
        }

        internal ArrayBuffer(BufferManager manager, int bufferSize)
        {
            m_manager = manager;
            m_array = new byte[bufferSize];
        }

        public byte[] Array
        {
            get { return m_array; }
        }

        protected internal void CheckIn(BufferSegment segment)
        {
            if (m_manager != null)
            {
                m_manager.CheckIn(segment);
            }
        }
    }

    public class TemporaryBufferSegment : BufferSegment
    {
        public TemporaryBufferSegment(int length)
            : base(new ArrayBuffer(new byte[length]), 0, length, 0)
        {
        }

        protected override void CheckIn()
        {
        }
    }

    public class BufferSegment
    {
        private readonly ArrayBuffer m_buffer;
        private readonly int m_length;
        private readonly int m_offset;
        private int m_uses;

        public BufferSegment(ArrayBuffer buffer, int offset, int length, int id)
        {
            m_buffer = buffer;
            m_offset = offset;
            m_length = length;
            Number = id;
        }

        public byte this[int i]
        {
            get {
                if (i >= m_length)
                    throw new IndexOutOfRangeException(string.Format("i({0}) >= m_length({1})", i, m_length));

                return m_buffer.Array[m_offset + i];
            }
        }

        public byte[] SegmentData
        {
            get
            {
                var bytes = new byte[m_length];
                System.Buffer.BlockCopy(m_buffer.Array, m_offset, bytes, 0, m_length);

                return bytes;
            }
        }

        public int Uses
        {
            get { return m_uses; }
            set { m_uses = value; }
        }

#if DEBUG
        public string LastUserTrace
        {
            get;
            set;
        }

        public DateTime LastUsage
        {
            get;
            set;
        }

        public object Token
        {
            get;
            set;
        }
#endif

        /// <summary>
        ///     Unique identifier
        /// </summary>
        public int Number
        {
            get;
            internal set;
        }

        public ArrayBuffer Buffer
        {
            get { return m_buffer; }
        }

        public int Offset
        {
            get { return m_offset; }
        }

        public int Length
        {
            get { return m_length; }
        }

        public void CopyFrom(byte[] bytes, int offset)
        {
            System.Buffer.BlockCopy(bytes, offset, m_buffer.Array, m_offset + offset, (bytes.Length - offset));
        }

        public void CopyTo(BufferSegment segment, int length)
        {
            if (length > m_length)
                throw new IndexOutOfRangeException("length > m_length");

            System.Buffer.BlockCopy(m_buffer.Array, m_offset, segment.Buffer.Array, segment.Offset, length);
        }

        public void IncrementUsage()
        {
            Interlocked.Increment(ref m_uses);
        }

        public void DecrementUsage()
        {
            if (Interlocked.Decrement(ref m_uses) == 0)
            {
                CheckIn();
            }
        }

        protected virtual void CheckIn()
        {
            m_buffer.CheckIn(this);
        }

        public static BufferSegment CreateSegment(byte[] bytes)
        {
            return CreateSegment(bytes, 0, bytes.Length);
        }

        public static BufferSegment CreateSegment(byte[] bytes, int offset, int length)
        {
            if (offset > length)
                throw new ArgumentException("offset > length");

            if (length > bytes.Length)
                throw new ArgumentException("length > bytes.Length");

            return new BufferSegment(new ArrayBuffer(bytes), offset, length, -1);
        }
    }

    /// <summary>
    ///     Manages a pool of small buffers allocated from large, contiguous chunks of memory.
    /// </summary>
    /// <remarks>
    ///     When used in an async network call, a buffer is pinned. Large numbers of pinned buffers
    ///     cause problem with the GC (in particular it causes heap fragmentation).
    ///     This class maintains a set of large segments and gives clients pieces of these
    ///     segments that they can use for their buffers. The alternative to this would be to
    ///     create many small arrays which it then maintained. This methodology should be slightly
    ///     better than the many small array methodology because in creating only a few very
    ///     large objects it will force these objects to be placed on the LOH. Since the
    ///     objects are on the LOH they are at this time not subject to compacting which would
    ///     require an update of all GC roots as would be the case with lots of smaller arrays
    ///     that were in the normal heap.
    /// </remarks>
    public class BufferManager //: IDisposable
    {
        protected static Logger log = LogManager.GetCurrentClassLogger();

        public static readonly List<BufferManager> Managers = new List<BufferManager>();

        /// <summary>
        ///     Default BufferManager for small buffers with up to 128 bytes length
        /// </summary>
        public static readonly BufferManager Tiny = new BufferManager(1024, 128);

        /// <summary>
        ///     Default BufferManager for small buffers with up to 1kb length
        /// </summary>
        public static readonly BufferManager Small = new BufferManager(1024, 1024);

        /// <summary>
        ///     Default BufferManager for default-sized buffers (usually up to 8kb)
        /// </summary>
        public static readonly BufferManager Default = new BufferManager(128, 8192);

        /// <summary>
        ///     Large BufferManager for buffers up to 64kb size
        /// </summary>
        public static readonly BufferManager Large = new BufferManager(128, 64*1024);

        /// <summary>
        ///     Extra Large BufferManager holding 512kb buffers
        /// </summary>
        public static readonly BufferManager ExtraLarge = new BufferManager(32, 512*1024);

        /// <summary>
        ///     Super Large BufferManager holding 1MB buffers
        /// </summary>
        public static readonly BufferManager SuperSized = new BufferManager(16, 1024*1024);

        /// <summary>
        ///     Holds the total amount of memory allocated by all buffer managers.
        /// </summary>
        public static long GlobalAllocatedMemory = 0;

        private static volatile int m_segmentId;
        private readonly LockFreeQueue<BufferSegment> m_availableSegments;
        private readonly ConcurrentDictionary<int, BufferSegment> m_segmentsInUse;
        private readonly List<ArrayBuffer> m_buffers;

        /// <summary>
        ///     Count of segments per Buffer
        /// </summary>
        private readonly int m_segmentCount;

        /// <summary>
        ///     Segment size
        /// </summary>
        private readonly int m_segmentSize;

        /// <summary>
        ///     Total count of segments in all buffers
        /// </summary>
        private int m_totalSegmentCount;

        /// <summary>
        ///     The number of currently available segments
        /// </summary>
        public int AvailableSegmentsCount
        {
            get { return m_availableSegments.Count; } //do we really care about volatility here?
        }

        public bool InUse
        {
            get { return m_availableSegments.Count < m_totalSegmentCount; }
        }

        public int UsedSegmentCount
        {
            get { return m_totalSegmentCount - m_availableSegments.Count; }
        }

        /// <summary>
        ///     The total number of currently allocated buffers.
        /// </summary>
        public int TotalBufferCount
        {
            get { return m_buffers.Count; } //do we really care about volatility here?
        }

        /// <summary>
        ///     The total number of currently allocated segments.
        /// </summary>
        public int TotalSegmentCount
        {
            get { return m_totalSegmentCount; } //do we really care about volatility here?
        }

        /// <summary>
        ///     The total amount of all currently allocated buffers.
        /// </summary>
        public int TotalAllocatedMemory
        {
            get { return m_buffers.Count*(m_segmentCount*m_segmentSize); } // do we really care about volatility here?
        }

        /// <summary>
        ///     The size of a single segment
        /// </summary>
        public int SegmentSize
        {
            get { return m_segmentSize; }
        }

#if DEBUG
        public BufferSegment[] GetSegmentsInUse()
        {
            return m_segmentsInUse.Values.ToArray();
        }

#endif

        #region Constructors

        /// <summary>
        ///     Constructs a new <see cref="Default"></see> object
        /// </summary>
        /// <param name="segmentCount">The number of chunks tocreate per segment</param>
        /// <param name="segmentSize">The size of a chunk in bytes</param>
        public BufferManager(int segmentCount, int segmentSize)
        {
            m_segmentCount = segmentCount;
            m_segmentSize = segmentSize;
            m_buffers = new List<ArrayBuffer>();
            m_availableSegments = new LockFreeQueue<BufferSegment>();
            m_segmentsInUse = new ConcurrentDictionary<int, BufferSegment>();
            Managers.Add(this);
        }

        #endregion

        private static int lastValue;
        /// <summary>
        ///     Checks out a segment, creating more if the pool is empty.
        /// </summary>
        /// <returns>a BufferSegment object from the pool</returns>
        public BufferSegment CheckOut()
        {
            BufferSegment segment;

            if (!m_availableSegments.TryDequeue(out segment))
            {
                lock (m_buffers)
                {
                    while (!m_availableSegments.TryDequeue(out segment))
                    {
                        CreateBuffer();
                    }
                }
            }

            // this doubles up with what CheckIn() looks for, but no harm in that, really.
            if (segment.Uses > 1)
            {
#if DEBUG
                log.Error(
                    "Checked out segment (Size: {0}, Number: {1}, Uses: {6}) that is already in use! Queue contains: {2}, Buffer amount: {3} Last usage : {4} StackTrace : {5}",
                    segment.Length, segment.Number, m_availableSegments.Count, m_buffers.Count, segment.LastUserTrace, new StackTrace().ToString(), segment.Uses);
#else
                log.Error(
                    "Checked out segment (Size: {0}, Number: {1}, Uses: {5}) that is already in use! Queue contains: {2}, Buffer amount: {3}, Stack Trace : {4}",
                    segment.Length, segment.Number, m_availableSegments.Count, m_buffers.Count, new StackTrace().ToString(), segment.Uses);
#endif
            }

            // set initial usage to 1
            segment.Uses = 1;

            //Debug.WriteLineIf(UsedSegmentCount != lastValue, string.Format("Used:{0} Last:{1} Stack:{2}", UsedSegmentCount, lastValue, new StackTrace()));
            lastValue = UsedSegmentCount;
#if DEBUG
            m_segmentsInUse.TryAdd(segment.Number, segment);
            segment.LastUserTrace = new StackTrace().ToString();
            segment.LastUsage = DateTime.Now;
#endif
            return segment;
        }

        /// <summary>
        ///     Checks out a segment, and wraps it with a SegmentStream, creating more if the pool is empty.
        /// </summary>
        /// <returns>a SegmentStream object wrapping the BufferSegment taken from the pool</returns>
        public SegmentStream CheckOutStream()
        {
            return new SegmentStream(CheckOut());
        }

        /// <summary>
        ///     Requeues a segment into the buffer pool.
        /// </summary>
        /// <param name="segment">the segment to requeue</param>
        public void CheckIn(BufferSegment segment)
        {
            if (segment.Uses > 1)
            {
#if DEBUG
                log.Error(
                    "Checked in segment (Size: {0}, Number: {1}, Uses:{5}) that is already in use! Queue contains: {2}, Buffer amount: {3} Last Usage:{4}, StackTrace:{6}",
                    segment.Length, segment.Number, m_availableSegments.Count, m_buffers.Count, segment.LastUserTrace, segment.Uses, new StackTrace().ToString());
#else
                log.Error(
                    "Checked in segment (Size: {0}, Number: {1}, Uses:{4}) that is already in use! Queue contains: {2}, Buffer amount: {3}, StackTrace:{5}",
                    segment.Length, segment.Number, m_availableSegments.Count, m_buffers.Count, segment.Uses, new StackTrace().ToString());
#endif

            }

            m_availableSegments.Enqueue(segment);

            BufferSegment dummy;
            m_segmentsInUse.TryRemove(segment.Number, out dummy);
        }

        /// <summary>
        ///     Creates a new buffer and adds the segments to the buffer pool.
        /// </summary>
        private void CreateBuffer()
        {
            // create a new buffer 
            var newBuf = new ArrayBuffer(this, m_segmentCount*m_segmentSize);

            // create segments from the buffer
            for (int i = 0; i < m_segmentCount; i++)
            {
                m_availableSegments.Enqueue(new BufferSegment(newBuf, i*m_segmentSize, m_segmentSize, m_segmentId++));
            }

            // increment our total count
            m_totalSegmentCount += m_segmentCount;

            // hold a ref to our new buffer
            m_buffers.Add(newBuf);

            // update global alloc'd memory
            Interlocked.Add(ref GlobalAllocatedMemory, m_segmentCount*m_segmentSize);
        }

        /// <summary>
        ///     Returns a BufferSegment that is at least of the given size.
        /// </summary>
        /// <param name="payloadSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">In case that the payload exceeds the SegmentSize of the largest buffer available.</exception>
        public static BufferSegment GetSegment(int payloadSize, bool allowExceed = false)
        {
            if (payloadSize <= Tiny.SegmentSize)
            {
                return Tiny.CheckOut();
            }
            if (payloadSize <= Small.SegmentSize)
            {
                return Small.CheckOut();
            }
            if (payloadSize <= Default.SegmentSize)
            {
                return Default.CheckOut();
            }
            if (payloadSize <= Large.SegmentSize)
            {
                return Large.CheckOut();
            }
            if (payloadSize <= ExtraLarge.SegmentSize)
            {
                return ExtraLarge.CheckOut();
            }

            if (allowExceed)
                return new TemporaryBufferSegment(payloadSize);

            throw new ArgumentOutOfRangeException("Required buffer is way too big: " + payloadSize);
        }

        /// <summary>
        ///     Returns a SegmentStream that is at least of the given size.
        /// </summary>
        /// <param name="payloadSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">In case that the payload exceeds the SegmentSize of the largest buffer available.</exception>
        public static SegmentStream GetSegmentStream(int payloadSize)
        {
            return new SegmentStream(GetSegment(payloadSize));
        }

        #region IDisposable Members

        ~BufferManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            // clear the segment queue
            BufferSegment segment;
            while (m_availableSegments.TryDequeue(out segment))

                // clean up buffers

                m_buffers.Clear();
        }

        #endregion
    }
}
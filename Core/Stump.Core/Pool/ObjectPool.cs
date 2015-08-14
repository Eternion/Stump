using System;
using System.Threading;
using Stump.Core.Collections;

// all credits goes to WCell dev team

namespace Stump.Core.Pool
{
    /// <summary>
    /// Interface for an object pool.
    /// </summary>
    /// <seealso cref="ObjectPoolMgr"/>
    /// <remarks>
    /// An object pool holds reusable objects. See <see cref="ObjectPoolMgr"/> for more details.
    /// </remarks>
    public interface IObjectPool
    {
        /// <summary>
        /// Amount of available objects in pool
        /// </summary>
        int AvailableCount
        {
            get;
        }

        /// <summary>
        /// Amount of objects that have been obtained but not recycled.
        /// </summary>
        int ObtainedCount
        {
            get;
        }

        /// <summary>
        /// Enqueues an object in the pool to be reused.
        /// </summary>
        /// <param name="obj">The object to be put back in the pool.</param>
        void Recycle(object obj);

        /// <summary>
        /// Grabs an object from the pool.
        /// </summary>
        /// <returns>An object from the pool.</returns>
        object ObtainObj();
    }

    public interface IPooledObject
    {
        void Cleanup();
    }

    /// <summary>
    /// A structure that contains information about an object pool.
    /// </summary>
    public struct ObjectPoolInfo
    {
        /// <summary>
        /// The number of hard references contained in the pool.
        /// </summary>
        public int HardReferences;

        /// <summary>
        /// The number of weak references contained in the pool.
        /// </summary>
        public int WeakReferences;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="weak">The number of weak references in the pool.</param>
        /// <param name="hard">The number of hard references in the pool.</param>
        public ObjectPoolInfo(int weak, int hard)
        {
            HardReferences = hard;
            WeakReferences = weak;
        }
    }

    /// <summary>
    /// This class represents a pool of objects.
    /// </summary>
    public class ObjectPool<T> : IObjectPool where T : class
    {
        private bool m_isBalanced;

        /// <summary>
        /// A queue of objects in the pool.
        /// </summary>
        private readonly LockFreeQueue<object> m_queue = new LockFreeQueue<object>();

        /// <summary>
        /// The minimum # of hard references that must be in the pool.
        /// </summary>
        private volatile int m_minSize = 25;

        /// <summary>
        /// The number of hard references in the queue.
        /// </summary>
        private volatile int m_hardReferences = 0;

        /// <summary>
        /// The number of hard references in the queue.
        /// </summary>
        private volatile int m_obtainedReferenceCount;

        /// <summary>
        /// Function pointer to the allocation function.
        /// </summary>
        private readonly Func<T> m_createObj;

        /// <summary>
        /// Gets the number of hard references that are currently in the pool.
        /// </summary>
        public int HardReferenceCount
        {
            get { return m_hardReferences; }
        }

        /// <summary>
        /// Gets the minimum size of the pool.
        /// </summary>
        public int MinimumSize
        {
            get { return m_minSize; }
            set { m_minSize = value; }
        }

        public int AvailableCount
        {
            get { return m_queue.Count; }
        }

        public int ObtainedCount
        {
            get { return m_obtainedReferenceCount; }
        }

        /// <summary>
        /// Gets information about the object pool.
        /// </summary>
        /// <value>A new <see cref="ObjectPoolInfo"/> object that contains information about the pool.</value>
        public ObjectPoolInfo Info
        {
            get
            {
                ObjectPoolInfo info;
                info.HardReferences = m_hardReferences;
                info.WeakReferences = m_queue.Count - m_hardReferences;

                return info;
            }
        }

        public bool IsBalanced
        {
            get { return m_isBalanced; }
            set { m_isBalanced = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="func">Function pointer to the allocation function.</param>
        public ObjectPool(Func<T> func)
            : this(func, false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="func">Function pointer to the allocation function.</param>
        public ObjectPool(Func<T> func, bool isBalanced)
        {
            IsBalanced = isBalanced;
            m_createObj = func;
        }

        /// <summary>
        /// Adds an object to the queue.
        /// </summary>
        /// <param name="obj">The object to be added.</param>
        /// <remarks>If there are at least <see cref="ObjectPool&lt;T&gt;.MinimumSize"/> hard references in the pool then the object is added as a WeakReference.
        /// A WeakReference allows an object to be collected by the GC if there are no other hard references to it.</remarks>
        public void Recycle(T obj)
        {
            if (obj is IPooledObject)
            {
                ((IPooledObject)obj).Cleanup();
            }
            else if (obj is IDisposable)
            {
                ((IDisposable) obj).Dispose();
            }

            if (m_hardReferences >= m_minSize)
            {
                m_queue.Enqueue(new WeakReference(obj));
            }
            else
            {
                m_queue.Enqueue(obj);
                Interlocked.Increment(ref m_hardReferences);
            }

            if (m_isBalanced)
            {
                OnRecycle();
            }
        }

        /// <summary>
        /// Adds an object to the queue.
        /// </summary>
        /// <param name="obj">The object to be added.</param>
        /// <remarks>If there are at least <see cref="ObjectPool&lt;T&gt;.MinimumSize"/> hard references in the pool then the object is added as a WeakReference.
        /// A WeakReference allows an object to be collected by the GC if there are no other hard references to it.</remarks>
        public void Recycle(object obj)
        {
            if (obj is T)
            {
                if (obj is IPooledObject)
                {
                    ((IPooledObject)obj).Cleanup();
                }

                if (m_hardReferences >= m_minSize)
                {
                    m_queue.Enqueue(new WeakReference(obj));
                }
                else
                {
                    m_queue.Enqueue(obj);
                    Interlocked.Increment(ref m_hardReferences);
                }

                if (m_isBalanced)
                {
                    OnRecycle();
                }
            }
        }

        private void OnRecycle()
        {
            if (Interlocked.Decrement(ref m_obtainedReferenceCount) < 0)
            {
                throw new InvalidOperationException("Objects in Pool have been recycled too often: " + this);
            }
        }

#pragma warning disable 0693
        /// <summary>
        /// Removes an object from the queue.
        /// </summary>
        /// <returns>An object from the queue or a new object if none were in the queue.</returns>

        public T Obtain()
        {
            if (m_isBalanced)
            {
                Interlocked.Increment(ref m_obtainedReferenceCount);
            }

        DequeueObj:
            {
                object obj;
                if (!m_queue.TryDequeue(out obj))
                {
                    return m_createObj();
                }

                if (obj is WeakReference)
                {
                    var robj = ((WeakReference)obj).Target;
                    if (robj != null)
                        return robj as T;

                    goto DequeueObj;
                }

                Interlocked.Decrement(ref m_hardReferences);
                return obj as T;
            }
        }
#pragma warning restore 0693

        /// <summary>
        /// Removes an object from the queue.
        /// </summary>
        /// <returns>An object from the queue or a new object if none were in the queue.</returns>
        public object ObtainObj()
        {
            object obj;
            if (m_isBalanced)
            {
                Interlocked.Increment(ref m_obtainedReferenceCount);
            }

        DequeueObj:
            {
                if (!m_queue.TryDequeue(out obj))
                {
                    return m_createObj();
                }

                var robj = obj as WeakReference;
                if (robj != null)
                {
                    var robj2 = robj.Target;
                    if (robj2 != null)
                        return robj2;

                    goto DequeueObj;
                }
                Interlocked.Decrement(ref m_hardReferences);
            }

            return obj;
        }

        public override string ToString()
        {
            return GetType().Name + " for " + typeof(T).FullName;
        }
    }
}
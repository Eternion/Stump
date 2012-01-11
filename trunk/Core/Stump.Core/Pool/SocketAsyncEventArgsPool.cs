using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Stump.Core.Pool
{
    /// <summary>
    ///   Collection de SocketAsynEventArgs réutilisable
    /// </summary>
    public sealed class SocketAsyncEventArgsPool : IDisposable
    {
        private Stack<SocketAsyncEventArgs> m_pool;
        private bool m_disposed;

        /// <summary>
        ///   On initialise le Pool de SocketEventArgs à sa taille Maximum
        /// </summary>
        /// <param name = "capacity">Nombre Maximum de SocketAsyncEventArgs que pourra contenir notre pool</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        ///   Gets the number of SocketAsyncEventArgs instances in the pool
        /// </summary>
        public int Count
        {
            get { return m_pool.Count; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_pool.Clear();
            m_disposed = true;
        }

        #endregion

        /// <summary>
        ///   Met un SocketAsyncEventArgs dans le pool
        /// </summary>
        /// <param name = "item">Le SocketAsyncEventArgs à mettre dans le pool</param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Impossible d'ajouter un élement nul au pool");
            }
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        /// <summary>
        ///   Récupère un SocketAsyncEventArgs à partir du pool
        /// </summary>
        /// <returns>le SocketAsyncEventArgs provenant du pool</returns>
        public SocketAsyncEventArgs Pop()
        {
            if (m_disposed)
                throw new ObjectDisposedException("SocketAsyncEventArgsPool");

            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }
    }
}
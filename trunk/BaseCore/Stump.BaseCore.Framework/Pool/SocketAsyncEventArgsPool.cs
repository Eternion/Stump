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
using System.Collections.Generic;
using System.Net.Sockets;

namespace Stump.BaseCore.Framework.Pool
{
    /// <summary>
    ///   Collection de SocketAsynEventArgs réutilisable
    /// </summary>
    public sealed class SocketAsyncEventArgsPool : IDisposable
    {
        private Stack<SocketAsyncEventArgs> m_pool;

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
            m_pool = null;
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
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }
    }
}
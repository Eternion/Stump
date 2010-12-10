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
using System.Threading;
using Castle.ActiveRecord.Queries;
using NLog;

namespace Stump.Database
{
    /// <summary>
    ///   Utility class which give out next primary key for a table.
    /// </summary>
    public class IdGenerator
    {
        #region Fields

        /// <summary>
        ///   List containing all
        /// </summary>
        private static readonly List<IdGenerator> m_creators = new List<IdGenerator>();

        private static bool m_DBInitialized;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string m_idMember;
        private readonly long m_minId;
        private readonly string m_table;

        private readonly Type m_type;

        private long m_highestId;

        #endregion

        public IdGenerator(Type type, string idMember)
            : this(type, idMember, long.MinValue)
        {
        }

        public IdGenerator(Type type, string tableName, string idMember)
            : this(type, idMember, tableName, long.MinValue)
        {
        }

        public IdGenerator(Type type, string idMember, long minId)
            : this(type, idMember, type.Name, minId)
        {
        }

        public IdGenerator(Type type, string idMember, string tableName, long minId)
        {
            m_type = type;
            m_table = tableName;
            m_idMember = idMember;
            m_minId = minId;
            if (m_DBInitialized)
            {
                Init();
            }
            else
            {
                m_creators.Add(this);
            }
        }

        public long LastId
        {
            get { return Interlocked.Read(ref m_highestId); }
        }

        public static void InitializeCreators()
        {
            foreach (IdGenerator creator in m_creators)
            {
                creator.Init();
            }
            m_DBInitialized = true;
        }

        private void Init()
        {
            string str = string.Format("SELECT max(r.{0}) FROM {1} r", m_idMember, m_table);
            var query = new ScalarQuery<object>(m_type, str);
            object highestId;
            try
            {
                highestId = query.Execute();
            }
            catch (Exception e)
            {
                logger.Error("Database : Error when executing query : {0}\n Exception : {1}", query.Query, e.Message);
                highestId = query.Execute();
            }

            if (highestId == null)
            {
                m_highestId = 0;
            }
            else
            {
                m_highestId = (long) Convert.ChangeType(highestId, typeof (long));
            }

            if (m_highestId < m_minId)
            {
                m_highestId = m_minId;
            }
        }

        public long Next()
        {
            return Interlocked.Increment(ref m_highestId);
        }
    }
}
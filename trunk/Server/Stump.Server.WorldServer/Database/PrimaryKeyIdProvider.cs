using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Stump.Core.Pool;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Database
{
    public class PrimaryKeyIdProvider : UniqueIdProvider
    {
        public ORM.Database Database
        {
            get;
            set;
        }

        private static readonly ConcurrentBag<PrimaryKeyIdProvider> m_pool = new ConcurrentBag<PrimaryKeyIdProvider>();
        private static bool m_synchronised;

        public PrimaryKeyIdProvider(string columnName, string tableName)
        {
            ColumnName = columnName;
            TableName = tableName;

            if (m_synchronised)
                Synchronize();
            else
                m_pool.Add(this);

            Database = WorldServer.Instance.DBAccessor.Database;
        }

        public string ColumnName
        {
            get;
            private set;
        }

        public string TableName
        {
            get;
            private set;
        }

        [Initialization(InitializationPass.Eighth, "Synchronize id providers")]
        public static void SynchronizeAll()
        {
            foreach (PrimaryKeyIdProvider provider in m_pool)
            {
                provider.Synchronize();
            }

            m_synchronised = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Synchronize()
        {
            int id;
            try
            {
                var result = Database.ExecuteScalar<object>(string.Format("SELECT max({0}) FROM {1}", ColumnName, TableName));

                if (result is DBNull)
                    id = 0;
                else
                    id = (int)result;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Cannot retrieve max({0}) from table {1} : {2}", ColumnName, TableName, ex));
            }

            m_highestId = id;
        }

        public override int Pop()
        {
            return base.Pop();
        }

        public override void Push(int freeId)
        {
            // we disable it
        }
    }
}
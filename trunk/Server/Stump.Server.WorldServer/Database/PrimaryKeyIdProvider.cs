using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Castle.ActiveRecord.Queries;
using Stump.Core.Pool;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Database
{
    public class PrimaryKeyIdProvider : UniqueIdProvider
    {
        private static readonly ConcurrentBag<PrimaryKeyIdProvider> m_pool = new ConcurrentBag<PrimaryKeyIdProvider>();
        private static bool m_synchronised;

        [Initialization(InitializationPass.Eighth, "Synchronize id providers")]
        public static void SynchronizeAll()
        {
            foreach (var provider in m_pool)
            {
                provider.Synchronize();
            }

            m_synchronised = true;
        }

        public PrimaryKeyIdProvider(Type recordType, string columnName)
        {
            RecordType = recordType;
            ColumnName = columnName;

            if (m_synchronised)
                Synchronize();
            else
                m_pool.Add(this);
        }

        public Type RecordType
        {
            get;
            private set;
        }

        public string ColumnName
        {
            private get;
            set;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Synchronize()
        {
            var query = new ScalarQuery<object>(RecordType, string.Format("SELECT max(r.{0}) FROM {1} r", ColumnName, RecordType.Name));

            object id;
            try
            {
                id = query.Execute() ?? 0;
            }
            catch
            {
                // it's a hack
                id = query.Execute() ?? 0;
            }

            m_highestId = (int)Convert.ChangeType(id, typeof(int));
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
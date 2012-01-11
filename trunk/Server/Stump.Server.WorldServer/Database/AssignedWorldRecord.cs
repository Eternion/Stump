using System;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Database
{
    [IgnoreTable]
    public abstract class AssignedWorldRecord<T> : WorldBaseRecord<T>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static PrimaryKeyIdProvider m_idProvider;

        static AssignedWorldRecord()
        {
            if (m_idProvider != null)
                return;

            var type = typeof(T);
            var attribute = type.GetCustomAttribute<ActiveRecordAttribute>();

            if (attribute == null)
            {
                logger.Error("ActiveRecord Attribute not found in {0}", type.Name);
                return;
            }
            
            var primaryKeyField = (from property in type.GetProperties()
                let attr = property.GetCustomAttribute<PrimaryKeyAttribute>()
                where attr != null && attr.Generator == PrimaryKeyType.Assigned
                select Tuple.Create(property, attr)).FirstOrDefault();

            if (primaryKeyField == null)
            {
                logger.Error("Primary Key Property not found in {0}", type.Name);
                return;
            }

            m_idProvider = new PrimaryKeyIdProvider(typeof(T), primaryKeyField.Item1.Name);
        }

        public void AssignRecordId()
        {
            Id = m_idProvider.Pop();
        }

        [PrimaryKey(PrimaryKeyType.Assigned)]
        public virtual int Id
        {
            get;
            set;
        }

        public bool New
        {
            get;
            set;
        }

        public bool IdAssigned
        {
            get { return Id > 0; }
        }

        public override void Save()
        {
            if (IdAssigned && !New)
                Update();
            else
                Create();
        }

        public override void Create()
        {
            if (!IdAssigned)
                AssignRecordId();

            base.Create();

            New = false;
        }
    }

    static class AssignedWorldRecordAllocator
    {
        [Initialization(InitializationPass.First, "Register id providers")]
        public static void InitializeProviders()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOfGeneric(typeof(AssignedWorldRecord<>)))
                {
                    type.BaseType.TypeInitializer.Invoke(null, null);
                }
            }
        }
    }
}
using System;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Database
{
    public abstract class AssignedWorldRecord<T> : ISaveIntercepter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly PrimaryKeyIdProvider IdProvider;

        static AssignedWorldRecord()
        {
            if (IdProvider != null)
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

            IdProvider = new PrimaryKeyIdProvider(typeof(T), primaryKeyField.Item1.Name);
        }

        public static int PopNextId()
        {
            return IdProvider.Pop();
        }

        public void AssignIdentifier()
        {
            Id = PopNextId();
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

        public virtual void BeforeSave(ObjectStateEntry currentEntry)
        {
            if (currentEntry.State == EntityState.Added)
                AssignIdentifier();
        }
    }

    static class AssignedWorldRecordAllocator
    {
        [Initialization(InitializationPass.First, "Register id providers")]
        public static void InitializeProviders()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsAbstract && type.IsSubclassOfGeneric(typeof(AssignedWorldRecord<>)) && type != typeof(AssignedWorldRecord<>))
                {
                    var baseType = type.BaseType;

                    while (baseType != null && baseType.GetGenericTypeDefinition() != typeof(AssignedWorldRecord<>))
                    {
                        baseType = baseType.BaseType;
                    }

                    if (baseType == null)
                        continue;

                    baseType.TypeInitializer.Invoke(null, null);
                }
            }
        }
    }
}
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Database
{
    public abstract class AutoAssignedRecord<T> : ISaveIntercepter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly PrimaryKeyIdProvider IdProvider;

        static AutoAssignedRecord()
        {
            if (IdProvider != null)
                return;

            Type type = typeof(T);
            var attribute = type.GetCustomAttribute<TableNameAttribute>();

            if (attribute == null)
            {
                logger.Error("TableNameAttribute not found in {0}", type.Name);
                return;
            }

            IdProvider = new PrimaryKeyIdProvider("Id", attribute.TableName);
        }

        [PrimaryKey("Id", false)]
        public virtual int Id
        {
            get;
            set;
        }

        [Ignore]
        public bool IsNew
        {
            get;
            set;
        }

        public static int PopNextId()
        {
            return IdProvider.Pop();
        }

        public void AssignIdentifier()
        {
            Id = PopNextId();
        }

        public virtual void BeforeSave(bool insert)
        {
            if (insert && Id == 0)
                AssignIdentifier();
        }
    }

    internal static class AssignedWorldRecordAllocator
    {
        [Initialization(InitializationPass.First, "Register id providers")]
        public static void InitializeProviders()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsAbstract && type.IsSubclassOfGeneric(typeof (AutoAssignedRecord<>)) &&
                    type != typeof (AutoAssignedRecord<>))
                {
                    Type baseType = type.BaseType;

                    while (baseType != null && baseType.GetGenericTypeDefinition() != typeof (AutoAssignedRecord<>))
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
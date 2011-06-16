
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using Stump.Core.Extensions;
using Stump.Database.AuthServer;

namespace Stump.Database
{
    public static class ActiveRecordHelper
    {
        public static IEnumerable<Type> GetTables(Type recordBaseType)
        {
            var asm = Assembly.GetExecutingAssembly();
            var types = asm.GetTypes();
            
            return types.Where(t => t.IsSubclassOfGeneric(recordBaseType)).ToArray();
        }

        public static Type GetVersionType(IEnumerable<Type> types)
        {
            return types.First(t => t.GetInterfaces().Contains(typeof(IVersionRecord)));
        }

        public static Func<IVersionRecord> GetFindVersionMethod(Type versionType)
        {
            var method = versionType.BaseType.BaseType.GetMethod("FindAll", Type.EmptyTypes);

            var deleg = Delegate.CreateDelegate(typeof (Func<IEnumerable<IVersionRecord>>), method) as Func<IEnumerable<IVersionRecord>>;
            return () => deleg().FirstOrDefault();
        }

        public static void CreateVersionRecord(Type versionType, uint revision)
        {
            var instance = Activator.CreateInstance(versionType) as IVersionRecord;
            instance.Revision = revision;
            instance.UpdateDate = DateTime.Now;
            instance.CreateAndFlush();
        }

        public static void DeleteVersionRecord(Type versionType)
        {
            versionType.BaseType.BaseType.GetMethod("DeleteAll", Type.EmptyTypes).Invoke(null,null);
        }
    }
}
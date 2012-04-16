using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.BaseServer.Database
{
    public static class ActiveRecordHelper
    {
        public static Type[] GetTables(Assembly assembly, Type recordBaseType)
        {
            var types = assembly.GetTypes();

            return types.Where(t => t.GetCustomAttribute<IgnoreTableAttribute>() == null && t.IsSubclassOfGeneric(recordBaseType)).ToArray();
        }

        public static Type GetVersionType(IEnumerable<Type> types)
        {
            return types.First(t => t.GetInterfaces().Contains(typeof(IVersionRecord)));
        }

        public static Func<IVersionRecord> GetFindVersionMethod(Type versionType)
        {
            var method = versionType.BaseType.BaseType.GetMethod("FindAll", Type.EmptyTypes);

            if (method == null)
                throw new Exception(string.Format("Can't find method 'FindAll' on type {0}", versionType.Name));

            var deleg = Delegate.CreateDelegate(typeof(Func<IEnumerable<IVersionRecord>>), method) as Func<IEnumerable<IVersionRecord>>;

            if (deleg == null)
                throw new Exception(
                    string.Format("Method 'FindAll' on type {0} doesn't return a IEnumerable<IVersionRecord>",
                                  versionType.Name));

            return () => deleg().FirstOrDefault();
        }

        public static void CreateVersionRecord(Type versionType, uint revision)
        {
            var instance = Activator.CreateInstance(versionType) as IVersionRecord;

            if (instance == null)
                throw new Exception(string.Format("Type : {0} doesn't implement IVersionRecord", versionType.Name));

            instance.Revision = revision;
            instance.UpdateDate = DateTime.Now;
            instance.CreateAndFlush();
        }

        public static void DeleteVersionRecord(Type versionType)
        {
            var method = versionType.BaseType.BaseType.GetMethod("DeleteAll", Type.EmptyTypes);

            if (method == null)
                throw new Exception(string.Format("Can't find method 'DeleteAll' on type {0}", versionType.Name));

            method.Invoke(null, null);
        }
    }
}
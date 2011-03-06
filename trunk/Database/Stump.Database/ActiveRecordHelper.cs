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
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using Stump.BaseCore.Framework.Extensions;
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
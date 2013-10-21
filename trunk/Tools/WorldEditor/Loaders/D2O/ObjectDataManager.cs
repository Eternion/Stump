#region License GNU GPL
// ObjectDataManager.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DBSynchroniser;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using WorldEditor.Config;
using WorldEditor.Database;

namespace WorldEditor.Loaders.D2O
{
    /// <summary>
    /// Retrieves D2O objects. Thread safe
    /// </summary>
    public class ObjectDataManager : Singleton<ObjectDataManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<Type, D2OTable> m_tables = new Dictionary<Type, D2OTable>();

        public void Initialize()
        {
            LoadTables();
        }

        private void LoadTables()
        {
            foreach (var table in from type in typeof(D2OTable).Assembly.GetTypes()
                                  let attr = type.GetCustomAttribute<D2OClassAttribute>()
                                  let tableAttr = type.GetCustomAttribute<TableNameAttribute>()
                                  where tableAttr != null && attr != null
                                  select new D2OTable
                                  {
                                      Type = type,
                                      ClassName = attr.Name,
                                      TableName = tableAttr.TableName,
                                      Constructor = type.GetConstructor(new Type[0]).CreateDelegate()
                                  })
            {
                m_tables.Add(table.Type, table);
            }
        }

        public IEnumerable<D2OTable> Tables
        {
            get
            {
                return m_tables.Values;
            }
        }

        public T Get<T>(uint key)
            where T : class
        {
            return Get<T>((int)key);
        }

        public T Get<T>(int key)
            where T : class
        {
            if (!m_tables.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find table corresponding to type : " + typeof(T));

            return DatabaseManager.Instance.Database.Single<T>(key);
        }

        public T GetOrDefault<T>(uint key)
            where T : class
        {
            return GetOrDefault<T>((int)key);
        }

        public T GetOrDefault<T>(int key)
            where T : class
        {
            try
            {
                return Get<T>(key);
            }
            catch
            {
                return null;
            }
        }

        public void Insert<T>(T value)
            where T : class
        {            
            if (!m_tables.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find table corresponding to type : " + typeof(T));

            DatabaseManager.Instance.Database.Insert(value);
        }

        public void Update<T>(T value)
            where T : class
        {            
            if (!m_tables.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find table corresponding to type : " + typeof(T));

            DatabaseManager.Instance.Database.Update(value);
        }

        public void Delete<T>(T value)
        {          
            if (!m_tables.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find table corresponding to type : " + typeof(T));

            DatabaseManager.Instance.Database.Delete(value);
        }

        public int FindFreeId<T>()
        {          
            if (!m_tables.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find table corresponding to type : " + typeof(T));

            var table = m_tables[typeof (T)];

            var maxId = DatabaseManager.Instance.Database.ExecuteScalar<int>("SELECT MAX(Id) FROM " + table.TableName) + 1;

            return maxId < Settings.MinDataId ? Settings.MinDataId : maxId;
        }

        public IEnumerable<T> EnumerateObjects<T>() where T : class
        {
            if (!m_tables.ContainsKey(typeof(T))) // This exception should be called in all cases (serious)
                throw new ArgumentException("Cannot find table corresponding to type : " + typeof(T));

            var table = m_tables[typeof (T)];

            return DatabaseManager.Instance.Database.Query<T>("SELECT * FROM " + table.TableName);
        }
    }
}
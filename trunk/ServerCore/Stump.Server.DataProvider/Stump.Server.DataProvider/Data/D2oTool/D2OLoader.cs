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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.DataProvider.Data.D2oTool;

namespace Stump.Server.DataProvider.Data
{
    public static class D2OLoader
    {
        /// <summary>
        /// Path to 'data' folder
        /// </summary>
        [Variable]
        public static string DataPath = "./../../data";

        /// <summary>
        /// Name of d2o folder in 'data' folder
        /// </summary>
        [Variable]
        public static string D2ODir = "/common/";

        /// <summary>
        /// Name of i18n folder in 'data' folder
        /// </summary>
        [Variable]
        public static string I18NDir = "/i18n/";

        /// <summary>
        /// Name of the i18n file used by the server
        /// </summary>
        [Variable]
        public static string I18NFileName = "i18n_en.d2i";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static I18NFile m_i18nFile;

        public static I18NFile I18NFile
        {
            get { return m_i18nFile ?? (m_i18nFile = new I18NFile(DataPath + I18NDir + I18NFileName)); }
        }

        /// <summary>
        ///   Load a D2O file with a class constraint
        /// </summary>
        /// <typeparam name = "T">Constraint class</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LoadData<T>(bool ignoreException = false) where T : class
        {
            object[] attributes = typeof(T).GetCustomAttributes(typeof(AttributeAssociatedFile), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This class '{0}' hasn't any associated d2o file", typeof(T)));

            string name = attributes.Cast<AttributeAssociatedFile>().First().FilesName.First();
            var datafile = new D2OFile(DataPath + D2ODir + name + ".d2o");

            Dictionary<int, D2OClassDefinition> classes = datafile.GetClasses();

            var copy = new List<T>(classes.Count);

            foreach (var @class in classes)
            {
                try
                {
                    if (@class.Value.ClassType == typeof(T))
                    {
                        var data = datafile.ReadObject<T>(@class.Key);

                        if (data != null)
                        {
                            copy.Add(data);
                        }
                    }
                }
                catch
                {
                    logger.Warn("Error thrown when parsing {0} <id:{1}>", typeof(T).Name, @class.Key);
                }
            }
            return copy;
        }

        /// <summary>
        ///   Load a D2O file
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object> LoadData(string fileName, bool ignoreException = false)
        {
            var datafile = new D2OFile(DataPath + D2ODir + fileName + ".d2o");

            var classes = datafile.GetClasses();

            var copy = new List<object>(classes.Count);

            foreach (var @class in classes)
            {
                try
                {
                    object data = datafile.ReadObject(@class.Key);

                    if (data != null)
                        copy.Add(data);
                }
                catch
                {
                    logger.Warn("Error thrown when parsing (?) <id:{0}>", @class.Key);
                }
            }
            return copy;
        }

        /// <summary>
        ///   Load a D2O file with a class constraint
        /// </summary>
        /// <typeparam name = "T">Constraint class</typeparam>
        /// <returns></returns>
        public static T LoadData<T>(int index, bool ignoreException = false) where T : class
        {
            object[] attributes = typeof(T).GetCustomAttributes(typeof(AttributeAssociatedFile), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This class '{0}' hasn't any associated d2o file", typeof(T)));

            var name = attributes.Cast<AttributeAssociatedFile>().First().FilesName.First();
            var datafile = new D2OFile(DataPath + D2ODir + name + ".d2o");

             return datafile.ReadObject<T>(index);
        }

        /// <summary>
        ///   Load a D2O file
        /// </summary>
        /// <returns></returns>
        public static T LoadData<T>(string fileName,int index, bool ignoreException = false)
        {
            var datafile = new D2OFile(DataPath + D2ODir + fileName + ".d2o");

            return datafile.ReadObject<T>(index);
        }


        public static T[] LoadDataById<T>(Func<T, int> idSelector) where T : class
        {
            IEnumerable<T> items = LoadData<T>();

            var result = new T[items.Max(idSelector) + 1];

            foreach (T item in items)
            {
                result[idSelector(item)] = item;
            }

            return result;
        }

        public static Dictionary<TKey, TValue> LoadDataByIdAsDictionary<TKey, TValue>(Func<TValue, TKey> idSelector)
            where TValue : class
        {
            object[] attributes = typeof(TValue).GetCustomAttributes(typeof(AttributeAssociatedFile), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This class '{0}' hasn't any associated d2o file", typeof(TValue)));

            string name = attributes.Cast<AttributeAssociatedFile>().First().FilesName.First();
            var items = new List<TValue>();

            return LoadData(name).ToDictionary(entry => idSelector((entry as TValue)), entry => entry as TValue);
        }

        public static string GetI18NText(int id)
        {
            return !I18NFile.Exists(id) ? "{undefined}" : I18NFile.ReadText(id);
        }

        public static string GetI18NText(uint id)
        {
            return GetI18NText((int)id);
        }
    }
}
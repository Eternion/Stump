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
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Data.D2oTool;

namespace Stump.Server.BaseServer.Data
{
    public static class DataLoader
    {
        [Variable]
        public static string DataPath = "./../../data";

        [Variable]
        public static string D2ODir = "/common/";

        [Variable]
        public static string I18NDir = "/i18n/";

        [Variable]
        public static string I18NFileName = "i18n_fr.d2i";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static I18nFile m_i18nFile;

        public static I18nFile I18NFile
        {
            get { return m_i18nFile ?? (m_i18nFile = new I18nFile(DataPath + I18NDir + I18NFileName)); }
        }

        public static void LoadData<T>(ref List<T> items) where T : class
        {
            object[] attributes = typeof (T).GetType().GetCustomAttributes(typeof (AttributeAssociatedFile), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This file is not associated to the class {0}", typeof (T)));

            string name = attributes.Cast<AttributeAssociatedFile>().First().FilesName.First();
            var datafile = new D2oFile(DataPath + D2ODir + name + ".d2o");

            items.AddRange(from data in datafile.ReadObjects<T>(true)
                           where data.Value != null
                           select data.Value);
        }

        public static void LoadData<T, TC>(ref List<T> items1, ref List<TC> items2) where T : class
        {
            object[] attributes = typeof (T).GetType().GetCustomAttributes(typeof (AttributeAssociatedFile), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This file is not associated to the class {0}", typeof (T)));

            string name = attributes.Cast<AttributeAssociatedFile>().First().FilesName.First();
            var datafile = new D2oFile(DataPath + D2ODir + name + ".d2o");

            Dictionary<int, D2oClassDefinition> classes = datafile.GetClasses();

            foreach (var @class in classes)
            {
                if (@class.Value.ClassType.IsSubclassOf(typeof (T)))
                {
                    var data = datafile.ReadObject<T>(@class.Key);
                    items1.Add(data);
                }
                else if (@class.Value.ClassType.IsSubclassOf(typeof (TC)))
                {
                    var data = datafile.ReadObject<TC>(@class.Key);
                    items2.Add(data);
                }
            }
        }

        public static void LoadData<T>(ref List<T> items, bool ignoreException) where T : class
        {
            if (!ignoreException)
            {
                LoadData(ref items);
                return;
            }

            object[] attributes = typeof (T).GetType().GetCustomAttributes(typeof (AttributeAssociatedFile), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This file is not associated to the class {0}", typeof (T)));

            string name = attributes.Cast<AttributeAssociatedFile>().First().FilesName.First();
            var datafile = new D2oFile(DataPath + D2ODir + name + ".d2o");

            Dictionary<int, D2oClassDefinition> classes = datafile.GetClasses();


            foreach (var @class in classes)
            {
                try
                {
                    if (@class.Value.ClassType.IsSubclassOf(typeof (T)))
                    {
                        var data = datafile.ReadObject<T>(@class.Key);

                        if (data != null)
                            items.Add(data);
                    }
                }
                catch
                {
                    logger.Warn("Error thrown when parsing {0} <id:{1}>", typeof (T).Name, @class.Key);
                }
            }
        }

        public static void LoadData<T, TC>(ref List<T> items1, ref List<TC> items2, bool ignoreException)
            where T : class
        {
            if (!ignoreException)
            {
                LoadData(ref items1, ref items2);
                return;
            }

            object[] attributes = typeof (T).GetType().GetCustomAttributes(typeof (AttributeAssociatedFile), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This file is not associated to the class {0}", typeof (T)));

            string name = attributes.Cast<AttributeAssociatedFile>().First().FilesName.First();
            var datafile = new D2oFile(DataPath + D2ODir + name + ".d2o");

            Dictionary<int, D2oClassDefinition> classes = datafile.GetClasses();

            foreach (var @class in classes)
            {
                try
                {
                    if (@class.Value.ClassType.IsSubclassOf(typeof (T)))
                    {
                        var data = datafile.ReadObject<T>(@class.Key);
                        items1.Add(data);
                    }
                    else if (@class.Value.ClassType.IsSubclassOf(typeof (TC)))
                    {
                        var data = datafile.ReadObject<TC>(@class.Key);
                        items2.Add(data);
                    }
                }
                catch
                {
                    logger.Warn("Error thrown when parsing {0} or {1} <id:{2}>", typeof (T).Name, typeof (TC).Name,
                                @class.Key);
                }
            }
        }

        public static T[] LoadDataById<T>(Func<T, int> idSelector) where T : class
        {
            var items = new List<T>();

            LoadData(ref items);

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
            var items = new List<TValue>();

            LoadData(ref items);

            return items.ToDictionary(idSelector);
        }

        public static string GetI18NText(int id)
        {
            using (var i18Nfile = new I18nFile(DataPath + I18NDir + I18NFile))
            {
                return !i18Nfile.Exists(id) ? "" : i18Nfile.ReadText(id);
            }
        }
    }
}
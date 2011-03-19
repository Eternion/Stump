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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using NLog;
using ProtoBuf;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.Server.DataProvider.Data.D2oTool
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

        /// <summary>
        /// Store D2O filenames that are already checked
        /// </summary>
        private static readonly List<string> m_checkedFiles = new List<string>();

        private static readonly Dictionary<string, D2OReader> m_loadedFiles = new Dictionary<string, D2OReader>();

        /// <summary>
        /// Common I18N file
        /// </summary>
        public static I18NFile I18NFile
        {
            get { return m_i18nFile ?? (m_i18nFile = new I18NFile(Path.Combine(DataPath, I18NDir, I18NFileName))); }
        }

        private static void ConvertToProtobuf(string filename, IEnumerable filedata)
        {
            var datafile = new D2OReader(Path.Combine(DataPath, D2ODir, filename));

            using (FileStream file = File.Create(Path.GetFileNameWithoutExtension(filename) + ".bin"))
            {
                Serializer.Serialize(file, filedata);
            }

            // hash checksum to verify if the d2o file has any change
            using (FileStream file = File.Open(filename, FileMode.Open))
            {
                File.WriteAllBytes(Path.GetFileNameWithoutExtension(filename) + ".hash", MD5.Create().ComputeHash(file));
            }

            m_checkedFiles.Add(filename);
        }

        private static bool HasToConvert(string filename)
        {
            if (!File.Exists(Path.GetFileNameWithoutExtension(filename) + ".bin") ||
                !File.Exists(Path.GetFileNameWithoutExtension(filename) + ".hash"))
                return false;

            if (!CheckD2OHash(filename))
                return false;

            return true;
        }

        private static bool CheckD2OHash(string filename)
        {
            if (!File.Exists(Path.GetFileNameWithoutExtension(filename) + ".hash"))
                return false;

            if (m_checkedFiles.Contains(filename))
                return true;

            byte[] hash;

            using (FileStream file = File.Open(filename, FileMode.Open))
            {
                hash = MD5.Create().ComputeHash(file);
            }

            if (File.ReadAllBytes(Path.GetFileNameWithoutExtension(filename) + ".hash") == hash)
            {
                m_checkedFiles.Add(filename);
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Load a D2O file with a class constraint
        /// </summary>
        /// <typeparam name = "T">Constraint class</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LoadData<T>(bool ignoreException = false) where T : class
        {
            object[] attributes = typeof (T).GetCustomAttributes(typeof (D2OClassAttribute), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This class '{0}' hasn't any associated d2o file", typeof (T)));

            string filename = attributes.Cast<D2OClassAttribute>().First().FilesName.First() + ".d2o";

            // read the D2O file directly and convert it into a protobuf file
            if (HasToConvert(filename))
            {
                D2OReader datafile;

                if (m_loadedFiles.ContainsKey(filename))
                    datafile = m_loadedFiles[filename];
                else
                {
                    datafile = new D2OReader(Path.Combine(DataPath, D2ODir, filename));
                    m_loadedFiles.Add(filename, datafile);
                }

                Dictionary<int, D2OClassDefinition> classes = datafile.GetClasses();

                var copy = new List<T>(classes.Count);

                foreach (var @class in classes)
                {
                    try
                    {
                        if (@class.Value.ClassType == typeof (T))
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
                        if (ignoreException)
                            logger.Warn("Error thrown when parsing {0} <id:{1}>", typeof (T).Name, @class.Key);
                        else
                            throw;
                    }
                }
                ConvertToProtobuf(filename, copy);

                return copy;
            }

            // in the other case we just read the protobuf file that is much faster
            List<T> result;
            using (FileStream file = File.Open(filename, FileMode.Open))
            {
                result = Serializer.Deserialize<List<T>>(file);
            }

            return result;
        }

        /// <summary>
        ///   Load a D2O file
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object> LoadData(string filename, bool ignoreException = false)
        {
            // read the D2O file directly and convert it into a protobuf file
            if (HasToConvert(filename))
            {
                D2OReader datafile;

                if (m_loadedFiles.ContainsKey(filename))
                    datafile = m_loadedFiles[filename];
                else
                {
                    datafile = new D2OReader(Path.Combine(DataPath, D2ODir, filename));
                    m_loadedFiles.Add(filename, datafile);
                }

                Dictionary<int, D2OClassDefinition> classes = datafile.GetClasses();

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
                        if (ignoreException)
                            logger.Warn("Error thrown when parsing {0} <id:{1}>", @class.Value.ClassType, @class.Key);
                        else
                            throw;
                    }
                }

                ConvertToProtobuf(filename, copy);

                return copy;
            }

            // in the other case we just read the protobuf file that is much faster
            List<object> result;
            using (FileStream file = File.Open(filename, FileMode.Open))
            {
                result = Serializer.Deserialize<List<object>>(file);
            }

            return result;
        }

        /// <summary>
        ///   Load an object from a D2O file with a class constraint by his id
        /// </summary>
        /// <typeparam name = "T">Constraint class</typeparam>
        /// <returns></returns>
        public static T LoadData<T>(int index, bool ignoreException = false) where T : class
        {
            object[] attributes = typeof (T).GetCustomAttributes(typeof (D2OClassAttribute), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This class '{0}' hasn't any associated d2o file", typeof (T)));

            string filename = attributes.Cast<D2OClassAttribute>().First().FilesName.First() + ".d2o";

            // we can't convert the D2O file because this function have to be fast

            D2OReader datafile;

            if (m_loadedFiles.ContainsKey(filename))
                datafile = m_loadedFiles[filename];
            else
            {
                datafile = new D2OReader(Path.Combine(DataPath, D2ODir, filename));
                m_loadedFiles.Add(filename, datafile);
            }

            return datafile.ReadObject<T>(index);
        }

        /// <summary>
        ///   Load an object from a D2O file by his id
        /// </summary>
        /// <returns></returns>
        public static T LoadData<T>(string filename, int index, bool ignoreException = false)
        {
            D2OReader datafile;

            if (m_loadedFiles.ContainsKey(filename))
                datafile = m_loadedFiles[filename];
            else
            {
                datafile = new D2OReader(Path.Combine(DataPath, D2ODir, filename + ".d2o"));
                m_loadedFiles.Add(filename, datafile);
            }

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
            object[] attributes = typeof (TValue).GetCustomAttributes(typeof (D2OClassAttribute), false);

            if (attributes.Length <= 0)
                throw new Exception(string.Format("This class '{0}' hasn't any associated d2o file", typeof (TValue)));

            string name = attributes.Cast<D2OClassAttribute>().First().FilesName.First();
            var items = new List<TValue>();

            return LoadData(name).ToDictionary(entry => idSelector((entry as TValue)), entry => entry as TValue);
        }

        public static string GetI18NText(int id)
        {
            return !I18NFile.Exists(id) ? "{undefined}" : I18NFile.ReadText(id);
        }

        public static string GetI18NText(uint id)
        {
            return GetI18NText((int) id);
        }

        /// <summary>
        /// Clear the cached files to reload the D2O files into memory
        /// </summary>
        public static void ClearCachedFiles()
        {
            m_loadedFiles.Clear();
        }
    }
}
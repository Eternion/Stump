using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.BaseServer.Database.Interfaces;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Tools.CacheManager
{
    public class DatabaseBuilder
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Assembly m_assembly;
        private readonly string m_d2oFolder; 
        private string m_d2iFolder;
        private D2OReader[] m_d2oReaders;

        public DatabaseBuilder(Assembly assembly, string d2oFolder, string d2iFolder)
        {
            m_assembly = assembly;
            m_d2oFolder = d2oFolder;
            m_d2iFolder = d2iFolder;
        }

        public void Build()
        {
            BuildD2ITables();
            BuildD2OTables();
        }

        private void BuildD2ITables()
        {
            var textRecordType = m_assembly.GetTypes().Where(entry => entry.Name == "TextRecord").Single();
            var textUIRecordType = m_assembly.GetTypes().Where(entry => entry.Name == "TextUIRecord").Single();

            // delete all existing rows. BE CAREFUL !!
            var deleteAllMethod = textRecordType.GetMethod("DeleteAll",
                                                            BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy,
                                                            Type.DefaultBinder, Type.EmptyTypes, null);
            deleteAllMethod.Invoke(null, new object[0]);


            deleteAllMethod = textUIRecordType.GetMethod("DeleteAll",
                                                            BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy,
                                                            Type.DefaultBinder, Type.EmptyTypes, null);
            deleteAllMethod.Invoke(null, new object[0]);


            var d2iFiles = new Dictionary<string, I18NFile>();
            foreach (var file in Directory.EnumerateFiles(m_d2iFolder, "*.d2i"))
            {
                var match = Regex.Match(Path.GetFileName(file), @"i18n_(\w+)\.d2i");
                var i18NFile = new I18NFile(file);

                d2iFiles.Add(match.Groups[1].Value, i18NFile);
            }

            logger.Info("Build table 'texts' ...");
            var records = new Dictionary<int, ITextRecord>();
            foreach (var file in d2iFiles)
            {
                foreach (var text in file.Value.ReadAllText())
                {
                    if (!records.ContainsKey(text.Key))
                    {
                        records.Add(text.Key, Activator.CreateInstance(textRecordType) as ITextRecord);
                        records[text.Key].Id = (uint) text.Key;
                    }

                    switch (file.Key)
                    {
                        case "fr":
                            records[text.Key].Fr = text.Value;
                            break;
                        case "en":
                            records[text.Key].En = text.Value;
                            break;
                        case "de":
                            records[text.Key].De = text.Value;
                            break;
                        case "it":
                            records[text.Key].It = text.Value;
                            break;
                        case "es":
                            records[text.Key].Es = text.Value;
                            break;
                        case "ja":
                            records[text.Key].Ja = text.Value;
                            break;
                        case "nl":
                            records[text.Key].Nl = text.Value;
                            break;
                        case "pt":
                            records[text.Key].Pt = text.Value;
                            break;
                        case "ru":
                            records[text.Key].Ru = text.Value;
                            break;
                    }
                }
            }

            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            int counter = 0;
            foreach (var textRecord in records)
            {
                textRecord.Value.Create();
                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, records.Count, (int)((counter / (double)records.Count) * 100d));
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);


            logger.Info("Build table 'texts_ui' ...");
            var recordsUI = new Dictionary<string, ITextUIRecord>();
            foreach (var file in d2iFiles)
            {
                foreach (var text in file.Value.ReadAllUiText())
                {
                    if (!recordsUI.ContainsKey(text.Key))
                    {
                        recordsUI.Add(text.Key, Activator.CreateInstance(textUIRecordType) as ITextUIRecord);
                        recordsUI[text.Key].Name = text.Key;
                    }

                    switch (file.Key)
                    {
                        case "fr":
                            recordsUI[text.Key].Fr = text.Value;
                            break;
                        case "en":
                            recordsUI[text.Key].En = text.Value;
                            break;
                        case "de":
                            recordsUI[text.Key].De = text.Value;
                            break;
                        case "it":
                            recordsUI[text.Key].It = text.Value;
                            break;
                        case "es":
                            recordsUI[text.Key].Es = text.Value;
                            break;
                        case "ja":
                            recordsUI[text.Key].Ja = text.Value;
                            break;
                        case "nl":
                            recordsUI[text.Key].Nl = text.Value;
                            break;
                        case "pt":
                            recordsUI[text.Key].Pt = text.Value;
                            break;
                        case "ru":
                            recordsUI[text.Key].Ru = text.Value;
                            break;
                    }
                }
            }

            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;
            counter = 0;
            foreach (var textRecord in recordsUI)
            {
                textRecord.Value.Create();
                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, recordsUI.Count, (int)((counter / (double)recordsUI.Count) * 100d));
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);

        }

        private void BuildD2OTables()
        {
            foreach (var table in GetTables())
            {
                logger.Info("Build table '{0}' ...", table.TableName);
                
                var reader = FindD2OFile(table);

                // delete all existing rows. BE CAREFUL !!
                var deleteAllMethod = table.TableType.GetMethod("DeleteAll",
                                                                BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy,
                                                                Type.DefaultBinder, Type.EmptyTypes, null);

                deleteAllMethod.Invoke(null, new object[0]);

                var objects = reader.ReadObjects().Values.ToArray();
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;
                for (int i = 0; i < objects.Length; i++)
                {
                    var row = table.GenerateRow(objects[i]);
                    ((ActiveRecordBase) row).Create();

                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("{0}/{1} ({2}%)", i, objects.Length, (int)((i / (double)objects.Length) * 100d));
                }

                Console.SetCursorPosition(cursorLeft, cursorTop);
            }
        }

        private IEnumerable<D2OTable> GetTables()
        {
            return from type in m_assembly.GetTypes()
                   where type.IsDerivedFromGenericType(typeof (ActiveRecordBase<>))
                   let attribute = type.GetCustomAttribute<D2OClassAttribute>()
                   where attribute != null
                   select new D2OTable(type);
        }

        private D2OReader FindD2OFile(D2OTable table)
        {
            if (m_d2oReaders == null)
            {
                m_d2oReaders = (from file in Directory.EnumerateFiles(m_d2oFolder, "*.d2o")
                                where Path.GetExtension(file) == "d2o" // it's a fucking quirk with 3 characters length extensions
                                select new D2OReader(file)).ToArray();
            }

            return m_d2oReaders.Where(entry => 
                entry.Classes.Values.Count(subentry => subentry.Name == table.ClassAttribute.Name &&
                    subentry.PackageName == table.ClassAttribute.PackageName) > 0).Single();
        }
    }
}
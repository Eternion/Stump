using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DBSynchroniser.Records;
using DBSynchroniser.Records.Langs;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.Core.Xml.Config;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser
{
    internal class Program
    {
        public const string ConfigFile = "config.xml";
        private static XmlConfig m_config;

        public static DatabaseAccessor Database;

        [Variable] public static readonly DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
            {
                DbName = "stump_data",
                Host = "localhost",
                User = "root",
                Password = "",
                ProviderName = "MySql.Data.MySqlClient",
            };

        [Variable(true)]
        public static string SpecificLanguage = "fr,en";

        [Variable(true)]
        public static string DofusCustomPath = "";

        [Variable(true)]
        public static string FilesOutput = "./generate";



        private static readonly Tuple<string, Action>[] m_menus = new[]
        {
            Tuple.Create<string, Action>("Set Dofus Path (empty = default)", SetDofusPath),
            Tuple.Create<string, Action>("Set languages (empty = all)", SetLanguages),
            Tuple.Create<string, Action>("Create database", CreateDatabase),
            Tuple.Create<string, Action>("Load langs", LoadLangsWithWarning),
            Tuple.Create<string, Action>("Generate client files", GenerateFiles),
        };

        private static readonly Dictionary<string, D2OTable> m_tables = new Dictionary<string, D2OTable>();

        private static void Main()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attr = type.GetCustomAttribute<D2OClassAttribute>();
                if (attr != null)
                {
                    var tableAttr = type.GetCustomAttribute<TableNameAttribute>();

                    if (tableAttr != null)
                    {
                        var table = new D2OTable
                            {
                                Type = type,
                                ClassName = attr.Name,
                                TableName = tableAttr.TableName,
                                Constructor = type.GetConstructor(new Type[0]).CreateDelegate()
                            };

                        m_tables.Add(attr.Name, table);
                    }
                }
            }

            Console.WriteLine("Load {0}...", ConfigFile);
            m_config = new XmlConfig(ConfigFile);
            m_config.AddAssembly(Assembly.GetExecutingAssembly());

            if (!File.Exists(ConfigFile))
                m_config.Create();
            else
                m_config.Load();


            Console.WriteLine("Open database ...");
            Database = new DatabaseAccessor(DatabaseConfiguration);
            Database.RegisterMappingAssembly(Assembly.GetExecutingAssembly());
            Database.Initialize();

            try
            {
                Database.OpenConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open database : {0}", e);
                Exit("Modify config file");
            }

            while (true)
            {
                ShowMenus();

                try
                {
                    var input = Console.ReadLine();
                    int number;
                    while (!int.TryParse(input, out number) || number < 1 || number > m_menus.Length)
                    {
                        Console.WriteLine("Give a valid number between 1 and {0}", m_menus.Length);
                        input = Console.ReadLine();
                    }

                    Console.Clear();
                    m_menus[number - 1].Item2();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex);
                }

                Console.WriteLine("Press Enter to return to the menu");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static void ShowMenus()
        {
            Console.WriteLine("Dofus path = {0}", FindDofusPath());
            Console.WriteLine("Languages = {0}", SpecificLanguage);
            Console.WriteLine();
            Console.WriteLine("What to do ?");
            int number = 1;
            foreach (var menu in m_menus)
            {
                Console.WriteLine(number + ". " + menu.Item1);
                number++;
            }
        }

        #region Menus actions
        private static void SetDofusPath()
        {
            Console.WriteLine("Enter dofus path (empty = default path):");
            var path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Path '{0}' doesn't exists", path);
            }
            else
            {
                DofusCustomPath = path;

                m_config.Save();

                Console.WriteLine("Path changed (saved)");
            }
        }

        private static void SetLanguages()
        {
            Console.WriteLine("Enter languages (default = fr,en) :");
            var langs = Console.ReadLine();
            SpecificLanguage = langs;

            m_config.Save();

            Console.WriteLine("Languages changed (saved)");
        }

        private static void CreateDatabase()
        {
            var d2oFolder = Path.Combine(FindDofusPath(), "data", "common");

            Console.WriteLine("WARNING IT WILL ERASE ALL TABLES. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            foreach (var table in m_tables)
            {
                Database.Database.Execute("DELETE FROM " + table.Value.TableName);
            }

            foreach (var filePath in Directory.EnumerateFiles(d2oFolder))
            {
                var ext = Path.GetExtension(filePath);
                if (ext != ".d2o" && ext != ".d2os")
                    continue;

                Console.Write("Import {0}...", Path.GetFileName(filePath));

                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;
                int i = 0;
                var d2oReader = new D2OReader(filePath);
                foreach (var entry in d2oReader.GetObjectsClasses())
                {
                    var table = !m_tables.ContainsKey(entry.Value.Name) ? 
                                        m_tables[entry.Value.ClassType.BaseType.Name] : m_tables[entry.Value.Name];
                    var obj = d2oReader.ReadObject(entry.Key);

                    var record = table.Constructor.DynamicInvoke() as ID2ORecord;
                    record.AssignFields(obj);

                    Database.Database.Insert(record);

                    i++;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("{0}/{1} ({2}%)", i, d2oReader.IndexCount,
                                  (int)( ( i / (double)d2oReader.IndexCount ) * 100d ));
                }

                Console.WriteLine("");
            }

            LoadLangs();
        }

        private static void LoadLangs()
        {
            Database.Database.Execute("DELETE FROM langs");
            Database.Database.Execute("DELETE FROM langs_ui");

            var d2iFiles = new Dictionary<string, D2IFile>();
            string d2iFolder = Path.Combine(FindDofusPath(), "data", "i18n");

            foreach (string file in Directory.EnumerateFiles(d2iFolder, "*.d2i"))
            {
                Match match = Regex.Match(Path.GetFileName(file), @"i18n_(\w+)\.d2i");
                var i18NFile = new D2IFile(file);

                d2iFiles.Add(match.Groups[1].Value, i18NFile);
            }

            var records = new Dictionary<int, LangText>();
            var uiRecords = new Dictionary<string, LangTextUi>();
            foreach (var file in d2iFiles)
            {
                if (!SpecificLanguage.Contains(file.Key))
                    continue;

                Console.WriteLine("Import {0}...", Path.GetFileName(file.Value.FilePath));
                foreach (var text in file.Value.GetAllText())
                {
                    LangText record;
                    if (!records.ContainsKey(text.Key))
                    {
                        record = new LangText();
                        record.Id = (uint)text.Key;
                        records.Add(text.Key, record);
                    }
                    else record = records[text.Key];

                    switch (file.Key)
                    {
                        case "fr":
                            record.French = text.Value;
                            break;
                        case "en":
                            record.English = text.Value;
                            break;
                        case "de":
                            record.German = text.Value;
                            break;
                        case "it":
                            record.Italian = text.Value;
                            break;
                        case "es":
                            record.Spanish = text.Value;
                            break;
                        case "ja":
                            record.Japanish = text.Value;
                            break;
                        case "nl":
                            record.Dutsh = text.Value;
                            break;
                        case "pt":
                            record.Portugese = text.Value;
                            break;
                        case "ru":
                            record.Russish = text.Value;
                            break;
                    }

                    Database.Database.Insert(record);
                }

                foreach (var text in file.Value.GetAllUiText())
                {
                    LangTextUi record;
                    if (!uiRecords.ContainsKey(text.Key))
                    {
                        record = new LangTextUi();
                        record.Name = text.Key;
                        uiRecords.Add(text.Key, record);
                    }
                    else record = uiRecords[text.Key];

                    switch (file.Key)
                    {
                        case "fr":
                            record.French = text.Value;
                            break;
                        case "en":
                            record.English = text.Value;
                            break;
                        case "de":
                            record.German = text.Value;
                            break;
                        case "it":
                            record.Italian = text.Value;
                            break;
                        case "es":
                            record.Spanish = text.Value;
                            break;
                        case "ja":
                            record.Japanish = text.Value;
                            break;
                        case "nl":
                            record.Dutsh = text.Value;
                            break;
                        case "pt":
                            record.Portugese = text.Value;
                            break;
                        case "ru":
                            record.Russish = text.Value;
                            break;
                    }

                    Database.Database.Insert(record);
                }
            }


            Console.WriteLine("Save texts...");
        }

        private static void LoadLangsWithWarning()
        {
            Console.WriteLine("WARNING IT WILL ERASE TABLES 'langs' AND 'langs_ui'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            LoadLangs();
        }

        private static void GenerateFiles()
        {
            if (!Directory.Exists(FilesOutput))
                Directory.CreateDirectory(FilesOutput);

            foreach (var table in m_tables.Values)
            {
                D2OWriter writer;
                if (table.Type.BaseType != typeof (object) &&
                    m_tables.ContainsKey(table.Type.BaseType.Name))
                {
                    writer = new D2OWriter(Path.Combine(FilesOutput, m_tables[table.Type.BaseType.Name].TableName + ".d2o"));
                }
                else
                    writer = new D2OWriter(Path.Combine(FilesOutput, table.TableName + ".d2o"));

                Console.WriteLine("Generating {0} ...", Path.GetFileName(writer.Filename));


                MethodInfo method = typeof(Database).GetMethodExt("Fetch", 1, new[]{ typeof(Sql)});
                MethodInfo generic = method.MakeGenericMethod(table.Type);
                var rows = ((IList)generic.Invoke(Database.Database, new object[] {new Sql("SELECT * FROM `" + table.TableName + "`")}));

                writer.StartWriting(false);
                foreach (ID2ORecord row in rows)
                {
                    var obj = row.CreateObject();
                    writer.Write(obj, row.Id);
                }

                writer.EndWriting();
            }
        }

        #endregion

        private static string FindDofusPath()
        {
            if (!string.IsNullOrEmpty(DofusCustomPath))
            {
                return DofusCustomPath;
            }

            var programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");

            if (string.IsNullOrEmpty(programFiles))
                programFiles = Environment.GetEnvironmentVariable("programfiles");

            if (string.IsNullOrEmpty(programFiles))
                return Path.Combine(AskDofusPath(), "app");

            if (Directory.Exists(Path.Combine(programFiles, "Dofus2", "app")))
                return Path.Combine(programFiles, "Dofus2", "app");
            if (Directory.Exists(Path.Combine(programFiles, "Dofus 2", "app")))
                return Path.Combine(programFiles, "Dofus 2", "app");

            var dofusDataPath = Path.Combine(AskDofusPath(), "app");

            if (!Directory.Exists(dofusDataPath))
                throw new Exception("Dofus data path not found");

            return dofusDataPath;
        }

        private static string AskDofusPath()
        {
            Console.WriteLine("Dofus path not found. Enter Dofus 2 root folder (%programFiles%/Dofus2):");

            return Path.GetFullPath(Console.ReadLine());
        }

        private static void Exit(string reason = "")
        {
            if (!string.IsNullOrEmpty(reason))
                Console.WriteLine(reason);

            Console.WriteLine("Press enter to exit");
            Console.Read();

            Environment.Exit(-1);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DBSynchroniser.Records;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.Core.Xml.Config;
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
        public static string PatchsFolder = "world_patchs";

        private static Tuple<string, Action>[] m_menus = new[]
        {
            Tuple.Create<string, Action>("Set Dofus Path (empty = default)", SetDofusPath),
            Tuple.Create<string, Action>("Set languages (empty = all)", SetLanguages),
            Tuple.Create<string, Action>("Create database", CreateDatabase),
        };

        private static Dictionary<string, D2OTable> m_tables = new Dictionary<string, D2OTable>();

        private static void Main(string[] args)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attr = type.GetCustomAttribute<D2OClassAttribute>();
                if (attr != null)
                {
                    var tableAttr = type.GetCustomAttribute<TableNameAttribute>();

                    if (tableAttr != null)
                    {
                        var table = new D2OTable();
                        table.Type = type;
                        table.ClassName = attr.Name;
                        table.TableName = tableAttr.TableName;
                        table.Constructor = type.GetConstructor(new Type[0]).CreateDelegate();

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
            string d2oFolder = Path.Combine(FindDofusPath(), "data", "common");

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
        }

        #endregion

        private static string FindDofusPath()
        {
            if (!string.IsNullOrEmpty(DofusCustomPath))
            {
                return DofusCustomPath;
            }

            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");

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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DBSynchroniser.Maps.Placements;
using DBSynchroniser.Records;
using DBSynchroniser.Records.Icons;
using DBSynchroniser.Records.Maps;
using Stump.Core.Attributes;
using Stump.Core.I18N;
using Stump.Core.Reflection;
using Stump.Core.Xml.Config;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database;
using Stump.DofusProtocol.D2oClasses.Tools.Ele;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Monsters;
using DBSynchroniser.Http;
using DBSynchroniser.Interactives;
using DBSynchroniser.Maps.Transitions;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Effects;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Items.Pets;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Maps.Cells;
using LangText = DBSynchroniser.Records.Langs.LangText;
using LangTextUi = DBSynchroniser.Records.Langs.LangTextUi;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.DofusProtocol.D2oClasses.Tools.Ma3;
using Stump.Server.WorldServer.Game.Actors.Look;

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

        [Variable] public static readonly DatabaseConfiguration WorldDatabaseConfiguration = new DatabaseConfiguration
        {
            DbName = "stump_world",
            Host = "localhost",
            User = "root",
            Password = "",
            ProviderName = "MySql.Data.MySqlClient",
        };

        [Variable(true)] public static string SpecificLanguage = "fr,en";

        [Variable(true)] public static string DofusCustomPath = "";

        [Variable(true)] public static string D2OOutput = "./generate_d2o";

        [Variable(true)] public static string D2IOutput = "./generate_d2i";

        [Variable(true)] public static string MapDecryptionKey = "649ae451ca33ec53bbcbcc33becf15f4";

        private static readonly Dictionary<string, Languages> m_stringToLang = new Dictionary<string, Languages>
        {
            {"fr", Languages.French},       
            {"en", Languages.English},
            {"es", Languages.Spanish},
            {"de", Languages.German},
            {"it", Languages.Italian},
            {"ja", Languages.Japanish},
            {"nl", Languages.Dutsh},
            {"pt", Languages.Portugese},
            {"ru", Languages.Russish},
        };


        private static readonly Tuple<string, Action>[] m_menus =
        {
            Tuple.Create<string, Action>("Set Dofus Path (empty = default)", SetDofusPath),
            Tuple.Create<string, Action>("Set languages (empty = all)", SetLanguages),
            Tuple.Create<string, Action>("Create database (stump_data)", CreateDatabase),
            Tuple.Create<string, Action>("Load langs (on stump_data)", LoadLangsWithWarning),
            Tuple.Create<string, Action>("Load icons files (on stump_data)", LoadIconsWithWarnings),
            Tuple.Create<string, Action>("Generate client files (from stump_data)", GenerateFiles),
            Tuple.Create<string, Action>("Synchronise world database (stump_data->stump_world)", SyncDatabases),
            Tuple.Create<string, Action>("Import maps (on stump_data)", LoadMapsWithWarning),
            Tuple.Create<string, Action>("Fix fight placement (on stump_data)", PlacementsFix.ApplyFix),
            Tuple.Create<string, Action>("Generate interactive spawn (on stump_world)", GenerateInteractiveSpawnWithWarning),
            Tuple.Create<string, Action>("Generate monsters spells, spawns and drops (on stump_world)", GenerateMonstersSpawnsAndDrops),
            Tuple.Create<string, Action>("Import pets foods (on stump_world)", ImportPetsFoods),
            Tuple.Create<string, Action>("Import mounts (on stump_world)", ImportMounts),
            Tuple.Create<string, Action>("Fix map transitions (on stump_world)", MapTransitionFix.ApplyFix),
            Tuple.Create<string, Action>("Import items appearanceId (on stump_data)", ImportItemsAppearance)
        };

        private static Dictionary<string, D2OTable> m_tables = new Dictionary<string, D2OTable>();

        private static void Main()
        {
            m_tables = EnumerateTables(Assembly.GetExecutingAssembly()).ToDictionary(x => x.ClassName);
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
                    string input = Console.ReadLine();
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

        private static IEnumerable<D2OTable> EnumerateTables(Assembly assembly)
        {
            return from type in assembly.GetTypes()
                   let attr = type.GetCustomAttribute<D2OClassAttribute>()
                   where attr != null && type.GetCustomAttribute<D2OIgnore>(false) == null
                   let tableAttr = type.GetCustomAttribute<TableNameAttribute>()
                   where tableAttr != null
                   select new D2OTable
                   {
                       Type = type,
                       ClassName = attr.Name,
                       TableName = tableAttr.TableName,
                       Constructor = type.GetConstructor(new Type[0]).CreateDelegate()
                   };
        }

        private static void ShowMenus()
        {
            Console.WriteLine("Dofus path = {0}", FindDofusPath());
            Console.WriteLine("Languages = {0}", SpecificLanguage);
            Console.WriteLine();
            Console.WriteLine("What to do ?");
            var number = 1;
            foreach (var menu in m_menus)
            {
                Console.WriteLine(number + ". " + menu.Item1);
                number++;
            }
        }

        public static string FindDofusPath()
        {
            if (!string.IsNullOrEmpty(DofusCustomPath))
                return DofusCustomPath;

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

        #region Menus actions

        private static void SetDofusPath()
        {
            Console.WriteLine("Enter dofus path (empty = default path):");
            string path = Console.ReadLine();

            if (!Directory.Exists(path))
                Console.WriteLine("Path '{0}' doesn't exists", path);
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
            string langs = Console.ReadLine();
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
                Database.Database.Execute("ALTER TABLE " + table.Value.TableName + " AUTO_INCREMENT=1");
            }

            foreach (var filePath in from filePath in Directory.EnumerateFiles(d2oFolder) let ext = Path.GetExtension(filePath) where ext == ".d2o" || ext == ".d2os" select filePath)
            {
                Console.Write("Import {0}...", Path.GetFileName(filePath));

                var cursorLeft = Console.CursorLeft;
                var cursorTop = Console.CursorTop;
                var i = 0;
                var d2oReader = new D2OReader(filePath);
                foreach (var entry in d2oReader.GetObjectsClasses())
                {
                    if (!m_tables.ContainsKey(entry.Value.Name) && !m_tables.ContainsKey(entry.Value.ClassType.BaseType.Name))
                        continue;

                    var table = !m_tables.ContainsKey(entry.Value.Name)
                        ? m_tables[entry.Value.ClassType.BaseType.Name]
                        : m_tables[entry.Value.Name];

                    var obj = d2oReader.ReadObject(entry.Key);

                    var record = table.Constructor.DynamicInvoke() as ID2ORecord;
                    record.AssignFields(obj);

                    Database.Database.Insert(record);
                    
                    i++;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("{0}/{1} ({2}%)", i, d2oReader.IndexCount,
                        (int) ((i/(double) d2oReader.IndexCount)*100d));
                }

                Console.WriteLine("");
            }

            LoadLangs();
            LoadIcons();
        }

        private static void LoadLangs()
        {
            Database.Database.Execute("DELETE FROM langs");
            Database.Database.Execute("DELETE FROM langs_ui");
            Database.Database.Execute("ALTER TABLE langs_ui AUTO_INCREMENT=1");


            var d2iFiles = new Dictionary<string, D2IFile>();
            var d2iFolder = Path.Combine(FindDofusPath(), "data", "i18n");

            foreach (var file in Directory.EnumerateFiles(d2iFolder, "*.d2i"))
            {
                var match = Regex.Match(Path.GetFileName(file), @"i18n_(\w+)\.d2i");
                var i18NFile = new D2IFile(file);

                d2iFiles.Add(match.Groups[1].Value, i18NFile);
            }

            var records = new Dictionary<int, LangText>();
            var uiRecords = new Dictionary<string, LangTextUi>();
            foreach (var file in d2iFiles.Where(file => SpecificLanguage.Contains(file.Key)))
            {
                var lang = m_stringToLang[file.Key];
                Console.WriteLine("Import {0}...", Path.GetFileName(file.Value.FilePath));
                foreach (var text in file.Value.GetAllText())
                {
                    LangText record;
                    if (!records.ContainsKey(text.Key))
                    {
                        record = new LangText {Id = (uint) text.Key};
                        records.Add(text.Key, record);
                    }
                    else record = records[text.Key];

                    record.SetText(lang, text.Value);
                }

                foreach (var text in file.Value.GetAllUiText())
                {
                    LangTextUi record;
                    if (!uiRecords.ContainsKey(text.Key))
                    {
                        record = new LangTextUi {Name = text.Key};
                        uiRecords.Add(text.Key, record);
                    }
                    else record = uiRecords[text.Key];

                    record.SetText(lang, text.Value);
                }
            }


            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var i = 0;
            var count = records.Count + uiRecords.Count;

            Console.WriteLine("Save texts(1/2)...");
            foreach (var record in records.Values)
            {
                Database.Database.Insert(record);

                i++;
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", i, count,
                    (int) ((i/(double) count)*100d));
            }

            Console.WriteLine("Save texts(2/2)...");
            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;
            foreach (var record in uiRecords.Values)
            {
                Database.Database.Insert(record);

                i++;
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", i, count,
                    (int) ((i/(double) count)*100d));
            }
            Console.WriteLine();
        }

        private static void LoadLangsWithWarning()
        {
            Console.WriteLine("WARNING IT WILL ERASE TABLES 'langs' AND 'langs_ui'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            LoadLangs();
        }

        private static void LoadIconsWithWarnings()
        {
            Console.WriteLine("WARNING IT WILL ERASE TABLE 'icons'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            LoadIcons();
        }

        private static void LoadIcons()
        {
            Database.Database.Execute("DELETE FROM icons");

            string iconsFilePath = Path.Combine(FindDofusPath(), "content", "gfx", "items", "bitmap0.d2p");
            var d2pFile = new D2pFile(iconsFilePath);

            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            int i = 0;
            int count = d2pFile.Entries.Count();
            foreach (D2pEntry entry in d2pFile.Entries)
            {
                var icon = new IconRecord();

                if (!entry.FullFileName.EndsWith(".png"))
                    continue;

                byte[] data = d2pFile.ReadFile(entry);
                string name = entry.FileName.Replace(".png", "");

                int id;
                switch (name)
                {
                    case "empty":
                        id = 0;
                        break;
                    case "error":
                        id = -1;
                        break;
                    default:
                        id = int.Parse(name);
                        break;
                }

                icon.Id = id;
                icon.ImageBinary = data;

                Database.Database.Insert(icon);

                i++;
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", i, count,
                    (int) ((i/(double) count)*100d));
            }
        }


        private static void GenerateFiles()
        {
            if (!Directory.Exists(D2OOutput))
                Directory.CreateDirectory(D2OOutput);
            else
                foreach (var file in Directory.EnumerateFiles(D2OOutput))
                    File.Delete(file);

            foreach (D2OTable table in m_tables.Values)
            {
                D2OWriter writer;
                if (table.Type.BaseType != typeof (object))
                {
                    var baseTable = table.Type.BaseType.GetCustomAttribute<D2OClassAttribute>().Name;
                    writer =
                        new D2OWriter(Path.Combine(D2OOutput, m_tables[baseTable].TableName + ".d2o"));
                }
                else
                    writer = new D2OWriter(Path.Combine(D2OOutput, table.TableName + ".d2o"));

                Console.WriteLine("Generating {0} ...", Path.GetFileName(writer.Filename));

                var rows = GetTableRows(table);
                writer.StartWriting(false);
                foreach (ID2ORecord row in rows)
                {
                    object obj = row.CreateObject();

                    if (obj is Item)
                    {
                        ((Item)obj).AppearanceId = 0;
                    }

                    writer.Write(obj, row.Id);
                }

                writer.EndWriting();
            }

            if (!Directory.Exists(D2IOutput))
                Directory.CreateDirectory(D2IOutput);
            else
                foreach (var file in Directory.EnumerateFiles(D2IOutput))
                    File.Delete(file);

            Console.WriteLine("Loading langs");
            var langs = Database.Database.Fetch<LangText>("SELECT * FROM langs");
            var langsUi = Database.Database.Fetch<LangTextUi>("SELECT * FROM langs_ui");

            foreach (var langStr in SpecificLanguage.Split(','))
            {
                var d2i = new D2IFile(Path.Combine(D2IOutput, "i18n_" + langStr + ".d2i"));

                 Console.WriteLine("Generating {0} ...", Path.GetFileName(d2i.FilePath));

                var lang = m_stringToLang[langStr];
                foreach (var record in langsUi)
                {
                    d2i.SetText(record.Name, record.GetText(lang) ?? "{null}");
                }

                foreach (var record in langs)
                {
                    d2i.SetText((int)record.Id, record.GetText(lang) ?? "{null}");
                }

                d2i.Save();
            }
        }

        public static DatabaseAccessor ConnectToWorld(bool showmsg = true)
        {
            var worldDatabase = new DatabaseAccessor(WorldDatabaseConfiguration);

            if (showmsg)
                Console.WriteLine("Connecting to {0}@{1}", WorldDatabaseConfiguration.DbName, WorldDatabaseConfiguration.Host);

            worldDatabase.RegisterMappingAssembly(typeof(WorldServer).Assembly);
            worldDatabase.Initialize();
            try
            {
                worldDatabase.OpenConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect : " + e);
                return null;
            }

            if (showmsg)
                Console.WriteLine("Connected!");

            return worldDatabase;
        }

        public static void SyncDatabases()
        {
            Console.WriteLine("Enter the tables to build (separated by comma, empty = all)");
            var tables = Console.ReadLine().Split(',');

            var worldDatabase = ConnectToWorld();

            if (worldDatabase == null)
                return;

            var worldTables = EnumerateTables(typeof(WorldServer).Assembly).ToList();
            var monsterGradeTable = worldTables.FirstOrDefault(x => x.ClassName == "MonsterGrade");

            if (monsterGradeTable == null)
            {
                Console.WriteLine("Table associated to class MonsterGrade not found !");
            }

            Console.WriteLine("Load patches");

            var patchs = new Dictionary<string, List<string>>();
            foreach (var filePath in Directory.EnumerateFiles("./Patchs"))
            {
                using (var reader = new StreamReader(filePath))
                {
                    if (reader.EndOfStream)
                        continue;

                    var line = reader.ReadLine().Trim().Replace(" ", "");

                    if (!line.StartsWith("--EXECUTEON:"))
                        continue;

                    var table = line.Remove(0, "--EXECUTEON:".Length).ToLower();
                    if (!patchs.ContainsKey(table))
                        patchs.Add(table, new List<string>());
                    patchs[table].Add(filePath);
                }
            }

            foreach (var table in worldTables.Where(table => tables.Any(x => !x.StartsWith("!") && table.TableName.Contains(x)) || tables.All(x => x.StartsWith("!") && !table.TableName.Contains(x.Remove(0, 1)))))
            {
                // reset the table
                worldDatabase.Database.Execute("DELETE FROM " + table.TableName);
                worldDatabase.Database.Execute("ALTER TABLE " + table.TableName + " AUTO_INCREMENT=1");

                if (table.TableName == "monsters_grades") // handled by monsters_templates
                    continue;

                if (table.TableName == "monsters_templates" && monsterGradeTable != null)
                {
                    worldDatabase.Database.Execute($"DELETE FROM {monsterGradeTable.TableName}");
                    worldDatabase.Database.Execute($"ALTER TABLE {monsterGradeTable.TableName} AUTO_INCREMENT=1");
                }

                Console.WriteLine($"Build table '{table.TableName}' ...");

                if (!m_tables.ContainsKey(table.ClassName))
                {
                    Console.WriteLine($"{DatabaseConfiguration.DbName} does not contain a table bound to class {table.ClassName}");
                    continue;
                }

                var dataTable = m_tables[table.ClassName];

                using (var transaction = Database.Database.GetTransaction())
                {
                    InitializeCounter();
                    var i = 0;
                    var rows = GetTableRows(dataTable);
                    foreach (ID2ORecord row in rows)
                    {
                        IAssignedByD2O record;
                        var obj = row.CreateObject();

                        // monster grades are in an other table
                        if (obj is Monster monster && monsterGradeTable != null)
                        {
                            foreach (var monsterGrade in monster.grades)
                            {
                                record = (IAssignedByD2O)monsterGradeTable.Constructor.DynamicInvoke();
                                record.AssignFields(monsterGrade);

                                worldDatabase.Database.Insert(record);
                            }
                        }

                        record = (IAssignedByD2O)table.Constructor.DynamicInvoke();
                        record.AssignFields(obj);

                        worldDatabase.Database.Insert(record);

                        i++;
                        UpdateCounter(i, rows.Count);
                    }
                    EndCounter();
                    transaction.Complete();
                }

                if (!patchs.ContainsKey(table.TableName.ToLower()))
                    continue;

                foreach (var filePath in patchs[table.TableName.ToLower()])
                {
                    ExecutePatch(filePath, worldDatabase.Database);
                }
            }

            var count = 0;
            if (tables.Length == 0 ||
                tables.Any(x => !x.StartsWith("!") && "langs".Contains(x)) || tables.All(x => x.StartsWith("!") && !"langs".Contains(x.Remove(0, 1))))
            {
                Console.WriteLine("Synchronise langs ...");

                var langs = Database.Database.Fetch<LangText>("SELECT * FROM langs");
                var langsUi = Database.Database.Fetch<LangTextUi>("SELECT * FROM langs_ui");

                worldDatabase.Database.Execute("DELETE FROM langs");
                worldDatabase.Database.Execute("ALTER TABLE langs AUTO_INCREMENT=1");

                worldDatabase.Database.Execute("DELETE FROM langs_ui");
                worldDatabase.Database.Execute("ALTER TABLE langs_ui AUTO_INCREMENT=1");

                Console.WriteLine("Build table 'langs' ...");

                InitializeCounter();
                foreach (var lang in langs)
                {
                    worldDatabase.Database.Insert(lang);
                    count++;
                    UpdateCounter(count, langs.Count);
                }
                EndCounter();

                Console.WriteLine("Build table 'langs_ui' ...");
                count = 0;
                foreach (var lang in langsUi)
                {
                    worldDatabase.Database.Insert(lang);
                    count++;
                    UpdateCounter(count, langsUi.Count);
                }
                EndCounter();
            }

            count = 0;
            if (tables.Length == 0 ||
                tables.Any(x => !x.StartsWith("!") && "world_maps".Contains(x)) || tables.All(x => x.StartsWith("!") && !"world_maps".Contains(x.Remove(0, 1))))
            {
                Console.WriteLine("Synchronise maps ...");

                var maps = Database.Database.Fetch<MapRecord>("SELECT * FROM maps");

                worldDatabase.Database.Execute("DELETE FROM world_maps");
                worldDatabase.Database.Execute("ALTER TABLE world_maps AUTO_INCREMENT=1");

                Console.WriteLine("Build table 'world_maps' ...");

                InitializeCounter();
                foreach (var map in maps)
                {
                    worldDatabase.Database.Insert(map.GetWorldRecord());
                    count++;
                    UpdateCounter(count, maps.Count);
                }
                EndCounter();
            }
        }
        

        private static void LoadMapsWithWarning()
        {
            Console.WriteLine("WARNING IT WILL ERASE TABLES 'maps'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            ImportMaps();
        }


        public static void ImportMaps()
        {
            Console.WriteLine("Import maps");
            Database.Database.Execute("DELETE FROM maps");
            Database.Database.Execute("ALTER TABLE maps AUTO_INCREMENT=1");

            InitializeCounter();
            string mapsfilePath = Path.Combine(FindDofusPath(), "content", "maps", "maps0.d2p");
            var d2pFile = new D2pFile(mapsfilePath);
            var entries = d2pFile.ReadAllFiles();
            var i = 0;
            int fails = 0;
            var failures = new List<int>();
            foreach (var mapBytes in entries.Values)
            {
                var mapFile = new DlmReader(mapBytes) { DecryptionKey = MapDecryptionKey };
                MapRecord record;
                try
                {
                    record = new MapRecord(mapFile.ReadMap());
                }
                catch(BadEncodedMapException ex)
                {
                    fails++;
                    failures.Add(ex.Map.Id);
                    continue;
                }

                Database.Database.Insert(record);

                UpdateCounter(i++, entries.Count);
            }
            EndCounter();
            if (fails > 0)
            {
                Console.WriteLine($"{fails} fails !");
                Console.WriteLine(string.Join(", ", failures.Select(x => x.ToString())));
            }
        }

        public static void GenerateInteractiveSpawnWithWarning()
        {
            Console.WriteLine("WARNING IT WILL ERASE TABLES 'interactives_spawns'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            InteractiveSpawnLoader.LoadSpawns();
        }

        public static void GenerateInteractiveSpawns()
        {
            Console.WriteLine("Generating interactive spawns");
            var worldDatabase = ConnectToWorld();
            if (worldDatabase == null)
                return;

            worldDatabase.Database.Execute("DELETE FROM interactives_spawns");
            worldDatabase.Database.Execute("ALTER TABLE interactives_spawns AUTO_INCREMENT=1");
            string eleFilePath = Path.Combine(FindDofusPath(), "content", "maps", "elements.ele");
            string mapsfilePath = Path.Combine(FindDofusPath(), "content", "maps", "maps0.d2p");

            var eleFile = new EleReader(eleFilePath);
            var eleInstance = eleFile.ReadElements();
            var d2pFile = new D2pFile(mapsfilePath);
            var entries = d2pFile.ReadAllFiles();
            var i = 0;
            var ids = new List<int>();
            var failures = new List<int>();
            var spawns = new Dictionary<int, InteractiveSpawn>();
            var elementsGlobal = new Dictionary<int, DlmGraphicalElement>();
            int fails = 0;
            InitializeCounter();
            foreach (var mapBytes in entries.Values)
            {
                DlmReader mapFile;
                DlmMap map = null;
                try
                {
                    mapFile = new DlmReader(mapBytes) {DecryptionKey = MapDecryptionKey};
                    map = mapFile.ReadMap();
                }
                catch (Exception)
                {
                    fails++;
                    continue;
                }

                var elements = (from layer in map.Layers
                               from cell in layer.Cells
                               from element in cell.Elements.OfType<DlmGraphicalElement>()
                               where element.Identifier != 0
                               let point = new MapPoint(cell.Id)
                               where Math.Abs(element.PixelOffset.X) + Math.Abs(element.PixelOffset.Y) < 50
                               select element).Distinct(ProjectionEqualityComparer<DlmGraphicalElement>.Create(x => x.Identifier));

                var elements2 = (from layer in map.Layers
                                from cell in layer.Cells
                                from element in cell.Elements.OfType<DlmGraphicalElement>()
                                where element.Identifier != 0
                                select element).ToArray();

                var diff = elements2.Where(x => !elements.Contains(x)).ToArray();
                
                foreach (var element in elements)
                {
                    var eleElement = eleInstance.GraphicalDatas[(int)element.ElementId];

                    var spawn = new InteractiveSpawn
                    {
                        Id = (int) element.Identifier,
                        MapId = map.Id,
                        CellId = element.Cell.Id,
                        Animated = eleElement.Type == EleGraphicalElementTypes.ANIMATED,
                        ElementId = (int) element.ElementId,
                    };

                    if (ids.Contains(spawn.Id))
                    {
                        Console.WriteLine($"Id {spawn.Id} already added");
                        failures.Add(spawn.Id);
                        continue;
                    }

                    ids.Add(spawn.Id);
                    worldDatabase.Database.Insert("interactives_spawns", "Id", false, spawn);
                    spawns.Add(spawn.Id, spawn);
                    elementsGlobal.Add(spawn.Id, element);

                }

                UpdateCounter(i++, entries.Count);
            }
            EndCounter();

            if (fails > 0)
                Console.WriteLine($"{fails} failes !");

            ExecutePatch("./Patchs/interactives_spawns_patch.sql", worldDatabase.Database);
        }

        public static void GenerateMonstersSpawnsAndDrops()
        {
            Console.WriteLine("WARNING IT WILL ERASE TABLES 'monsters_spells', 'monsters_spawns' AND 'monsters_drops'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            Console.WriteLine("Generating monsters spells, spawns and drops");
            var worldDatabase = ConnectToWorld();
            if (worldDatabase == null)
                return;

            worldDatabase.Database.Execute("DELETE FROM monsters_spawns");
            worldDatabase.Database.Execute("ALTER TABLE monsters_spawns AUTO_INCREMENT=1");

            worldDatabase.Database.Execute("DELETE FROM monsters_drops");
            worldDatabase.Database.Execute("ALTER TABLE monsters_drops AUTO_INCREMENT=1");

            worldDatabase.Database.Execute("DELETE FROM monsters_spells");
            worldDatabase.Database.Execute("ALTER TABLE monsters_spells AUTO_INCREMENT=1");

            var dataTable = m_tables["Monster"];

            if (dataTable == null)
                return;

            InitializeCounter();
            var i = 0;
            var rows = GetTableRows(dataTable);

            foreach (ID2ORecord row in rows)
            {
                var obj = row.CreateObject() as Monster;

                foreach (var subarea in obj.Subareas)
                {
                    var record = new MonsterSpawn
                    {
                        SubAreaId = (int?)subarea,
                        Frequency = obj.IsMiniBoss ? 0.01 : (obj.FavoriteSubareaId == subarea ? 1.25 : 1),
                        MonsterId = obj.Id,
                        MinGrade = (int)obj.Grades.First()?.Grade,
                        MaxGrade = (int)obj.Grades.Last()?.Grade,
                        IsDisabled = obj.IsQuestMonster
                    };

                    worldDatabase.Database.Insert(record);
                }

                foreach (var drop in obj.Drops)
                {
                    if (drop.HasCriteria)
                        continue;

                    var record = new DroppableItem
                    {
                        MonsterOwnerId = drop.MonsterId,
                        ItemId = (short)drop.ObjectId,
                        DropLimit = drop.Count,
                        RollsCounter = 1,
                        DropRateForGrade1 = drop.PercentDropForGrade1,
                        DropRateForGrade2 = drop.PercentDropForGrade2,
                        DropRateForGrade3 = drop.PercentDropForGrade3,
                        DropRateForGrade4 = drop.PercentDropForGrade4,
                        DropRateForGrade5 = drop.PercentDropForGrade5
                    };

                    worldDatabase.Database.Insert(record);
                }

                foreach (var spell in obj.Spells)
                {
                    var levels = worldDatabase.Database.Fetch<SpellLevelTemplate>($"SELECT * FROM spells_levels WHERE SpellId = {spell}");


                    foreach (var grade in obj.Grades)
                    {
                        var gradeId = worldDatabase.Database.Fetch<int>($"SELECT Id FROM monsters_grades WHERE MonsterId = {grade.MonsterId} AND GradeId = {grade.Grade}").FirstOrDefault();

                        // we have to exclude spell that have 0 AP cost (generally triggered spells)
                        var level = new[] {(int)grade.Grade, levels.Count, levels.FindLastIndex(x => x.ApCost > 0 || x.MinCastInterval > 0) + 1}.Min();

                        var record = new MonsterSpell
                        {
                            MonsterGradeId = gradeId,
                            SpellId = (int)spell,
                            Level = (short)level
                        };

                        worldDatabase.Database.Insert(record);
                    }
                }

                i++;
                UpdateCounter(i, rows.Count);
            }

            worldDatabase.Database.Execute("DELETE FROM monsters_spells WHERE SpellId = -1");//Avoid bad spells

            EndCounter();
        }

        public static void ImportItemsAppearance()
        {
            Console.WriteLine("WARNING IT WILL UPDATE TABLES 'Items'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            if (!File.Exists("Items.ma3"))
            {
                Console.WriteLine("Items.ma3 not found. Please download here: http://www.dofus.tools/myAvatar3/assets/data/Items.ma3");
                return;
            }

            var reader = new Ma3Reader("Items.ma3");
            var items = reader.ReadFile();
            reader.Dispose();

            InitializeCounter();

            var worldDatabase = ConnectToWorld();
            if (worldDatabase == null)
                return;

            var i = 0;
            foreach (var item in items)
            {
                var appearanceId = (short)item.SkinId;

                if (item.Look != string.Empty)
                {
                    try
                    {
                        worldDatabase.Database.Execute($"UPDATE `items_pets` SET `LookString` = '{item.Look}' WHERE `Id` = '{item.Id}'");
                        appearanceId = ActorLook.Parse(item.Look).BonesID;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }

                Database.Database.Execute($"UPDATE `Items` SET `AppearanceId` = '{appearanceId}' WHERE `Id` = '{item.Id}'");

                i++;
                UpdateCounter(i, items.Count);
            }

            EndCounter();
        }

        public static void ImportMounts()
        {
            Console.WriteLine("WARNING IT WILL ERASE TABLES 'mounts_bonus' AND UPDATE 'mounts_templates'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            Console.WriteLine("Importing mounts stats ...");
            var worldDatabase = ConnectToWorld();
            if (worldDatabase == null)
                return;

            Console.WriteLine("Initializing texts ...");
            TextManager.Instance.ChangeDataSource(worldDatabase.Database);
            TextManager.Instance.Initialize();
            TextManager.Instance.SetDefaultLanguage(Languages.French);

            Console.WriteLine("Fetch effects template ...");
            var effectsTemplates = worldDatabase.Database.Fetch<EffectTemplate>(EffectTemplateRelator.FetchQuery).ToDictionary(entry => (short)entry.Id);

            Console.WriteLine("Fetch items template ...");
            var mountsTemplates = worldDatabase.Database.Fetch<MountTemplate>("SELECT * FROM mounts_templates").ToDictionary(entry => (short)entry.Id);

            worldDatabase.Database.Execute("DELETE FROM mounts_bonus");
            worldDatabase.Database.Execute("ALTER TABLE mounts_bonus AUTO_INCREMENT=1");

            InitializeCounter();

            var i = 0;
            foreach (var mountTemplate in mountsTemplates)
            {
                var info = MountsExplorer.GetMountWebInfo(mountTemplate.Key);

                if (info == null)
                    continue;

                foreach (var effect in info.Effects)
                {
                    var field = "";

                    switch (effect.Name)
                    {
                        case "Temps de gestation":
                            field = "FecondationTime";
                            break;
                        case "Maturité":
                            field = "MaturityBase";
                            break;
                        case "Nombre de pods":
                            field = "PodsBase";
                            break;
                        case "Energie":
                            field = "EnergyBase";
                            break;
                        case "Taux d'apprentisage":
                            field = "LearnCoefficient";
                            effect.Value.Replace("%", string.Empty);
                            break;
                    }

                    if (field == "")
                    {
                        var possibleEffect = effectsTemplates.FirstOrDefault(x => x.Value.BonusType == 1
                            && x.Value.Operator == "+" && x.Value.Description.ToLower().EndsWith(effect.Name.ToLower()));

                        if (possibleEffect.Value == null)
                            continue;

                        var query = $"INSERT INTO `mounts_bonus` VALUES (NULL, '{mountTemplate.Key}', '{possibleEffect.Key}', '{effect.Value}')";
                        worldDatabase.Database.Execute(query);
                    }
                    else
                    {
                        var query = $"UPDATE `mounts_templates` SET `{field}` = '{effect.Value}' WHERE `Id` = '{mountTemplate.Key}'";
                        worldDatabase.Database.Execute(query);
                    }
                }

                i++;
                UpdateCounter(i, mountsTemplates.Count);
            }

            EndCounter();
        }

        public static void ImportPetsFoods()
        {
            Console.WriteLine("WARNING IT WILL ERASE TABLES 'items_pets_foods'. ARE YOU SURE ? (y/n)");
            if (Console.ReadLine() != "y")
                return;

            Console.WriteLine("Importing pets foods ...");
            var worldDatabase = ConnectToWorld();
            if (worldDatabase == null)
                return;

            Console.WriteLine("Initializing texts ...");
            TextManager.Instance.ChangeDataSource(worldDatabase.Database);
            TextManager.Instance.Initialize();
            TextManager.Instance.SetDefaultLanguage(Languages.French);

            Console.WriteLine("Fetch effects template ...");
            var effectsTemplates = worldDatabase.Database.Fetch<EffectTemplate>(EffectTemplateRelator.FetchQuery).ToDictionary(entry => (short)entry.Id);
            
            Console.WriteLine("Fetch items template ...");
            var itemsTemplates = worldDatabase.Database.Fetch<ItemTemplate>(ItemTemplateRelator.FetchQuery).ToDictionary(entry => (short)entry.Id);

            worldDatabase.Database.Execute("DELETE FROM items_pets_foods");
            worldDatabase.Database.Execute("ALTER TABLE items_pets_foods AUTO_INCREMENT=1");

            Console.WriteLine("Fetching pets web links ...");

            var links = PetsExplorer.FetchPetsLinks();

            Console.WriteLine("Importing pets foods from web site...");

            var templates = Database.Database.Query<PetRecord>("SELECT * FROM Pets").ToDictionary(x => x.Id);


            InitializeCounter();
            for (int i = 0; i < links.Length; i++)
            {
                var petLink = links[i];
                var info = PetsExplorer.GetPetWebInfo(petLink);

                if (!templates.ContainsKey(info.Id))
                    continue; // some pets doesn't eat

                var petTemplate = templates[info.Id];

                foreach (var webFood in info.Foods)
                {
                    for (int j = 0; j < webFood.Effects.Length; j++)
                    {
                        EffectsEnum effect;

                        if (petTemplate.PossibleEffects.Count == 0)
                        {
                            Console.WriteLine("");
                            Console.WriteLine($"ERROR - Pets {petTemplate.Id} has no effect");
                            break;
                        }

                        if (petTemplate.PossibleEffects.Count == 1)
                            effect = (EffectsEnum) petTemplate.PossibleEffects[0].EffectId;
                        else
                        {
                            var possibleEffect = petTemplate.PossibleEffects.OrderByDescending(x => effectsTemplates[(short) x.EffectId].Description.ToLower().Split(' ').Count(c => webFood.Effects[j].ToLower().Contains(c))).FirstOrDefault();
                            if (possibleEffect == null)
                            {
                                Console.WriteLine("");
                                Console.WriteLine($"ERROR - Effect \"{webFood.Effects[j]}\" not found");
                                break;
                            }

                            Console.WriteLine($"{webFood.Effects[j]} -> {(EffectsEnum) possibleEffect.EffectId}");
                            effect = (EffectsEnum) possibleEffect.EffectId;
                        }

                        var record = new PetFoodRecord()
                        {
                            PetId = info.Id,
                            FoodId = webFood.FoodId,
                            FoodType = webFood.FoodType,
                            FoodQuantity = webFood.Quantity,
                            BoostedEffect = effect,
                            BoostAmount = webFood.BoostQuantities[j],
                        };

                        worldDatabase.Database.Insert(record);
                    }
                }

                if (info.Ghost != null)
                {
                    worldDatabase.Database.Execute($"UPDATE items_pets SET GhostItemId={info.Ghost.Value} WHERE Id={info.Id}");
                }

                if (itemsTemplates.ContainsKey((short)info.Id))
                {
                    var template = itemsTemplates[(short)info.Id];
                    var certificate = itemsTemplates.Values.FirstOrDefault(x => x.Name.Contains("Certificat") && x.Name.Contains(template.Name));
                    if (certificate != null)
                        worldDatabase.Database.Execute($"UPDATE items_pets SET CertificateItemId={certificate.Id} WHERE Id={info.Id}");
                }
                UpdateCounter(i, links.Length);
            }

            EndCounter();
        }

        public static void ExecutePatch(string file, Database database)
        {
            Console.WriteLine("Execute patch '{0}'", Path.GetFileName(file));
            var lineIndex = 0;
            InitializeCounter();
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(line))
                       database.Execute(line);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error at line {0} : {1}", lineIndex, ex.Message);
                }
                finally
                {
                    lineIndex++;

                    UpdateCounter(lineIndex, lines.Length);
                }
            }
            EndCounter();
        }

        #endregion
            
        #region Helpers

        static IList GetTableRows(D2OTable table)
        {
            var method = typeof (Database).GetMethodExt("Fetch", 1, new[] {typeof (Sql)});
            var generic = method.MakeGenericMethod(table.Type);
            var rows =
                ((IList)
                    generic.Invoke(Database.Database,
                        new object[] {new Sql("SELECT * FROM `" + table.TableName + "`")}));

            return rows;
        }

        private static int m_cursorLeft;
        private static int m_cursorTop;
        public static void InitializeCounter()
        {
            m_cursorLeft = Console.CursorLeft;
            m_cursorTop = Console.CursorTop;
        }


        public static void UpdateCounter(int i, int count)
        {
            Console.SetCursorPosition(m_cursorLeft, m_cursorTop);
            Console.Write("{0}/{1} ({2}%)", i, count,
                (int) ((i/(double) count)*100d));
        }

        public static void EndCounter()
        {
            Console.SetCursorPosition(m_cursorLeft, m_cursorTop);
            Console.Write(new string(' ', Console.BufferWidth - m_cursorLeft));
            Console.SetCursorPosition(m_cursorLeft, m_cursorTop);
        }

        #endregion
    }   
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Reflection;
using Stump.Core.Sql;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.Server.WorldServer.Database;
using MonsterGrade = Stump.Server.WorldServer.Database.Monsters.MonsterGrade;

namespace Stump.Tools.CacheManager
{
    public class DatabaseBuilder
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Assembly m_assembly;
        private readonly string m_d2iFolder;
        private readonly string m_patchsFolder;
        private readonly string m_d2oFolder;
        private D2OReader[] m_d2oReaders;

        private DatabaseAccessor m_dbAccessor;
        public Database Database
        {
            get
            {
                return m_dbAccessor.Database;
            }
        }

        public DatabaseBuilder(DatabaseAccessor dbAccesor, Assembly assembly, string d2oFolder, string d2iFolder, string patchsFolder)
        {
            m_assembly = assembly;
            m_dbAccessor = dbAccesor;
            m_d2oFolder = d2oFolder;
            m_d2iFolder = d2iFolder;
            m_patchsFolder = patchsFolder;
        }

        public void Build()
        {
            BuildD2ITables();
            BuildD2OTables();
            ExecutePatchs();
        }

        private void BuildD2ITables()
        {
            // delete all existing rows. BE CAREFUL !!
            Database.Execute(SqlBuilder.BuildDelete("langs"));
            Database.Execute(SqlBuilder.BuildDelete("langs_ui"));

            var d2iFiles = new Dictionary<string, D2IFile>();
            foreach (string file in Directory.EnumerateFiles(m_d2iFolder, "*.d2i"))
            {
                Match match = Regex.Match(Path.GetFileName(file), @"i18n_(\w+)\.d2i");
                var i18NFile = new D2IFile(file);

                d2iFiles.Add(match.Groups[1].Value, i18NFile);
            }

            logger.Info("Build table 'texts' ...");
            var records = new Dictionary<int, Dictionary<string, object>>();
            foreach (var file in d2iFiles)
            {
                foreach (var text in file.Value.GetAllText())
                {
                    if (!records.ContainsKey(text.Key))
                    {
                        records.Add(text.Key, new Dictionary<string, object>());
                        records[text.Key].Add("Id", (uint) text.Key);
                    }

                    switch (file.Key)
                    {
                        case "fr":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("fr"))
                                records[text.Key].Add("French", text.Value);
                            break;
                        case "en":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("en"))
                                records[text.Key].Add("English", text.Value);
                            break;
                        case "de":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("de"))
                                records[text.Key].Add("German", text.Value);
                            break;
                        case "it":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("it"))
                                records[text.Key].Add("Italian", text.Value);
                            break;
                        case "es":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("es"))
                                records[text.Key].Add("Spanish", text.Value);
                            break;
                        case "ja":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("ja"))
                                records[text.Key].Add("Japanish", text.Value);
                            break;
                        case "nl":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("nl"))
                                records[text.Key].Add("Dutsh", text.Value);
                            break;
                        case "pt":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("pt"))
                                records[text.Key].Add("Portugese", text.Value);
                            break;
                        case "ru":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("ru"))
                                records[text.Key].Add("Russish", text.Value);
                            break;
                    }
                }
            }

            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            int counter = 0;
            using (var transaction = Database.GetTransaction())
            {
                foreach (var record in records)
                {
                    var listKey = new KeyValueListBase("langs", record.Value);
                    Database.Execute(SqlBuilder.BuildInsert(listKey));
                    counter++;

                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("{0}/{1} ({2}%)", counter, records.Count,
                                  (int) ((counter/(double) records.Count)*100d));
                }
                transaction.Complete();
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);


            logger.Info("Build table 'texts_ui' ...");
            var recordsUi = new Dictionary<string, Dictionary<string, object>>();
            foreach (var file in d2iFiles)
            {
                foreach (var text in file.Value.GetAllUiText())
                {
                    if (!recordsUi.ContainsKey(text.Key))
                    {
                        recordsUi.Add(text.Key, new Dictionary<string, object>());
                        recordsUi[text.Key].Add("Name", text.Key);
                    }

                    switch (file.Key)
                    {
                        case "fr":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("fr"))
                                recordsUi[text.Key].Add("French", text.Value);
                            break;
                        case "en":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("en"))
                                recordsUi[text.Key].Add("English", text.Value);
                            break;
                        case "de":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("de"))
                                recordsUi[text.Key].Add("German", text.Value);
                            break;
                        case "it":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("it"))
                                recordsUi[text.Key].Add("Italian", text.Value);
                            break;
                        case "es":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("es"))
                                recordsUi[text.Key].Add("Spanish", text.Value);
                            break;
                        case "ja":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("ja"))
                                recordsUi[text.Key].Add("Japanish", text.Value);
                            break;
                        case "nl":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("nl"))
                                recordsUi[text.Key].Add("Dutsh", text.Value);
                            break;
                        case "pt":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("pt"))
                                recordsUi[text.Key].Add("Portugese", text.Value);
                            break;
                        case "ru":
                            if (string.IsNullOrEmpty(Program.SpecificLanguage) || Program.SpecificLanguage.Contains("ru"))
                                recordsUi[text.Key].Add("Russish", text.Value);
                            break;
                    }
                }
            }

            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;
            counter = 0;
            using (var transaction = Database.GetTransaction())
            {
                foreach (var record in recordsUi)
                {
                    var listKey = new KeyValueListBase("langs_ui", record.Value);
                    Database.Execute(SqlBuilder.BuildInsert(listKey));
                    counter++;

                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("{0}/{1} ({2}%)", counter, recordsUi.Count,
                                  (int) ((counter/(double) recordsUi.Count)*100d));
                }
                transaction.Complete();
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        private void BuildD2OTables()
        {
            foreach (var table in GetTables())
            {
                logger.Info("Build table '{0}' ...", table.TableName);

                D2OReader reader = FindD2OFile(table);

                // reset the table
                Database.Execute(SqlBuilder.BuildDelete(table.TableName));
                Database.Execute("ALTER TABLE " + table.TableName + " AUTO_INCREMENT=1");

                using (var transaction = Database.GetTransaction())
                {
                    int cursorLeft = Console.CursorLeft;
                    int cursorTop = Console.CursorTop;
                    int i = 0;
                    foreach (var obj in reader.EnumerateObjects())
                    {
                        if (!IsSubClassOf(obj.GetType(), table.ClassAttribute.Name))
                            continue;

                        if (obj is Monster)
                            BuildMonsterGrades(obj as Monster);

                        var peta = table.GenerateRow(obj);
                        Database.Insert(peta);

                        i++;
                        Console.SetCursorPosition(cursorLeft, cursorTop);
                        Console.Write("{0}/{1} ({2}%)", i, reader.IndexCount,
                                      (int) ((i/(double) reader.IndexCount)*100d));
                    }
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    transaction.Complete();
                }
            }
        }

        private D2OTable m_monsterGradeTable;
        private void BuildMonsterGrades(Monster monster)
        {
            if (m_monsterGradeTable == null)
               m_monsterGradeTable = new D2OTable(typeof(MonsterGrade));

            foreach (var monsterGrade in monster.grades)
            {
                var row = (MonsterGrade)m_monsterGradeTable.GenerateRow(monsterGrade);

                Database.Execute(SqlBuilder.BuildDelete(m_monsterGradeTable.TableName, "MonsterId = " + row.MonsterId + " AND GradeId = " + row.GradeId));
                Database.Insert(row);
            }
        }

        private static bool IsSubClassOf(Type type, string compareTypeName)
        {
            if (type.Name == compareTypeName)
                return true;

            if (type.BaseType == typeof(object) || type.BaseType == null)
                return false;

            return IsSubClassOf(type.BaseType, compareTypeName);
        }

        private IEnumerable<D2OTable> GetTables()
        {
            foreach (var type in m_assembly.GetTypes())
            {
                if (type.GetCustomAttribute<D2OClassAttribute>() != null &&
                    type.HasInterface(typeof(IAssignedByD2O)))
                    yield return new D2OTable(type);
            }
        }

        private D2OReader FindD2OFile(D2OTable table)
        {
            if (m_d2oReaders == null)
            {
                m_d2oReaders = (from file in Directory.EnumerateFiles(m_d2oFolder, "*.d2o")
                                where Path.GetExtension(file) == ".d2o"
                                // it's a fucking quirk with 3 characters length extensions
                                select new D2OReader(file)).ToArray();
            }

            return m_d2oReaders.Single(entry => entry.Classes.Values.Count(
                subentry => subentry.Name == table.ClassAttribute.Name) > 0);
        }

        private void ExecutePatchs()
        {
            if (!Directory.Exists("./"+ m_patchsFolder + "/"))
                return;

            foreach (var sqlFile in Directory.EnumerateFiles("./"+ m_patchsFolder + "/", "*.sql"))
            {
                logger.Info("Execute patch '{0}'", sqlFile);
                foreach (var line in File.ReadAllLines(sqlFile))
                {
                     if (!string.IsNullOrWhiteSpace(line))
                         Database.Execute(line);
                }
            }
        }
    }
}
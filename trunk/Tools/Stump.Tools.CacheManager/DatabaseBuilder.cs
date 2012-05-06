using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.ActiveRecord;
using NLog;
using Stump.Core.Reflection;
using Stump.Core.Sql;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Tools.CacheManager.SQL;
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

        public DatabaseBuilder(Assembly assembly, string d2oFolder, string d2iFolder, string patchsFolder)
        {
            m_assembly = assembly;
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
            Type textRecordType = m_assembly.GetTypes().Where(entry => entry.Name == "TextRecord").Single();
            Type textUIRecordType = m_assembly.GetTypes().Where(entry => entry.Name == "TextUIRecord").Single();

            // delete all existing rows. BE CAREFUL !!
            Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildDelete("texts"));
            Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildDelete("texts_ui"));

            var d2iFiles = new Dictionary<string, I18NFile>();
            foreach (string file in Directory.EnumerateFiles(m_d2iFolder, "*.d2i"))
            {
                Match match = Regex.Match(Path.GetFileName(file), @"i18n_(\w+)\.d2i");
                var i18NFile = new I18NFile(file);

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
            Program.DBAccessor.ExecuteNonQuery("START TRANSACTION");
            foreach (var record in records)
            {
                var listKey = new KeyValueListBase("texts", record.Value);
                Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildInsert(listKey));
                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, records.Count, (int) ((counter/(double) records.Count)*100d));
            }
            Program.DBAccessor.ExecuteNonQuery("COMMIT");
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
            foreach (var record in recordsUi)
            {
                var listKey = new KeyValueListBase("texts_ui", record.Value);
                Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildInsert(listKey));
                counter++;

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write("{0}/{1} ({2}%)", counter, records.Count, (int) ((counter/(double) recordsUi.Count)*100d));
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        private void BuildD2OTables()
        {
            foreach (D2OTable table in GetTables())
            {
                logger.Info("Build table '{0}' ...", table.TableName);

                D2OReader reader = FindD2OFile(table);

                // delete all existing rows. BE CAREFUL !!
                if (table.Inheritance == null)
                {
                    Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildDelete(table.TableName));
                    Program.DBAccessor.ExecuteNonQuery("ALTER TABLE " + table.TableName + " AUTO_INCREMENT=1");
                }

                object[] objects = reader.ReadObjects().Values.ToArray();
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;
                for (int i = 0; i < objects.Length; i++)
                {
                    if (!IsSubClassOf(objects[i].GetType(), table.ClassAttribute.Name))
                        continue;

                    Dictionary<string, object> row = table.GenerateRow(objects[i]);

                    if (objects[i] is Monster)
                        BuildMonsterGrades(objects[i] as Monster);

                    // row might already exists
                    if (table.Inheritance != null && row.ContainsKey("Id"))
                        Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildDelete(table.TableName, "Id = " + row["Id"]));

                    var listKey = new KeyValueListBase(table.TableName, row);
                    Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildInsert(listKey));

                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("{0}/{1} ({2}%)", i, objects.Length, (int) ((i/(double) objects.Length)*100d));
                }

                Console.SetCursorPosition(cursorLeft, cursorTop);
            }
        }

        private D2OTable m_monsterGradeTable;
        private void BuildMonsterGrades(Monster monster)
        {
            if (m_monsterGradeTable == null)
               m_monsterGradeTable = new D2OTable(typeof(MonsterGrade));

            foreach (var monsterGrade in monster.grades)
            {
                var row = m_monsterGradeTable.GenerateRow(monsterGrade);

                Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildDelete(m_monsterGradeTable.TableName, "MonsterId = " + row["MonsterId"] + " AND Grade = " + row["Grade"]));
                var listKey = new KeyValueListBase(m_monsterGradeTable.TableName, row);
                Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildInsert(listKey));
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
            return from type in m_assembly.GetTypes()
                   where type.IsDerivedFromGenericType(typeof (ActiveRecordBase<>))
                   let attribute = type.GetCustomAttribute<D2OClassAttribute>()
                   where attribute != null && attribute.AutoBuild
                   select new D2OTable(type);
        }

        internal static Dictionary<string, string> GetNamesRelations(Type type)
        {
            var result = new Dictionary<string, string>();
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var d2oattribute = property.GetCustomAttribute<D2OFieldAttribute>();
                var dbattribute = property.GetCustomAttribute<PropertyAttribute>();

                if (d2oattribute == null)
                    continue;

                if (dbattribute != null && !string.IsNullOrEmpty(dbattribute.Column))
                {
                    result.Add(d2oattribute.FieldName, dbattribute.Column);
                }
                else
                {
                    result.Add(d2oattribute.FieldName, property.Name);
                }
            }

            return result;
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

            return m_d2oReaders.Where(entry =>
                                      entry.Classes.Values.Count(
                                          subentry => subentry.Name == table.ClassAttribute.Name &&
                                                      subentry.PackageName == table.ClassAttribute.PackageName) > 0).
                Single();
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
                         Program.DBAccessor.ExecuteNonQuery(line);
                }
            }
        }
    }
}
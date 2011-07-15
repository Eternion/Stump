using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Tools.CacheManager
{
    public class DatabaseBuilder
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Assembly m_assembly;
        private string m_d2oFolder;
        private D2OReader[] m_d2oReaders;

        public DatabaseBuilder(Assembly assembly, string d2oFolder)
        {
            m_assembly = assembly;
            m_d2oFolder = d2oFolder;
        }

        public void Build()
        {
            foreach (var table in GetTables())
            {
                logger.Info("Build table {0} ...", table.TableType.Name);
                
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
                                select new D2OReader(file)).ToArray();
            }

            return m_d2oReaders.Where(entry => 
                entry.Classes.Values.Count(subentry => subentry.Name == table.ClassAttribute.Name &&
                    subentry.PackageName == table.ClassAttribute.PackageName) > 0).Single();
        }
    }
}
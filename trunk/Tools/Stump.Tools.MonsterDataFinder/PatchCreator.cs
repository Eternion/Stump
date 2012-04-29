using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Stump.Core.Sql;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Tools.MonsterDataFinder.Readers;
using Monster = Stump.Tools.MonsterDataFinder.Data.Monster;

namespace Stump.Tools.MonsterDataFinder
{
    public class PatchCreator
    {
        public event Action<PatchCreator, MonsterTemplate, Monster> MonsterAnalysed;

        private long m_counter;
        private readonly string m_destination;

        private TextWriter m_monstersPatchWriter;
        private TextWriter m_monstersSpellsPatchWriter;
        private TextWriter m_monstersDropsPatchWriter;

        public long Counter
        {
            get { return Interlocked.Read(ref m_counter); }
        }

        public int Total
        {
            get;
            private set;
        }

        public double Percent
        {
            get { return Counter/(double)Total*100d; }
        }

        public PatchCreator(string destination)
        {
            m_destination = destination;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreatePatchs()
        {
            m_monstersPatchWriter = CreateSyncPatch(Path.Combine(m_destination, "monsters.sql"));
            m_monstersSpellsPatchWriter = CreateSyncPatch(Path.Combine(m_destination, "monsters_spells.sql"));
            m_monstersDropsPatchWriter = CreateSyncPatch(Path.Combine(m_destination, "monsters_drops.sql"));

            var monstersId = MonsterManager.Instance.GetTemplates().Select(entry => entry.Id).ToArray();
            Total = monstersId.Length;
            m_counter = 0;

            Parallel.ForEach(monstersId, ComputeMonster);

            m_monstersPatchWriter.Dispose();
            m_monstersDropsPatchWriter.Dispose();
            m_monstersSpellsPatchWriter.Dispose();
        }

        private void ComputeMonster(int id)
        {
            var reader = new MonsterDataReader();
            var dropParser = new DropParser();

            Monster monsterData;
            try
            {
                monsterData = reader.Request(id);
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                Interlocked.Increment(ref m_counter);
            }

            if (monsterData == null)
                return;

            MonsterTemplate monster = MonsterManager.Instance.GetTemplate(monsterData.id);

            if (monster == null)
                return;

            Console.WriteLine("Parse ... {0}", id);

            var spells = new List<SpellTemplate>();

            foreach (string spellName in monsterData.spells.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                SpellTemplate spell = SpellManager.Instance.GetFirstSpellTemplate(entry => entry.Name == spellName.Trim());

                if (spell == null)
                {
                    Console.WriteLine("Spell {0} not found", spellName.Trim());
                    continue;
                }

                spells.Add(spell);
            }

            if (monster.Grades != null)
            {
                foreach (MonsterGrade grade in monster.Grades)
                {
                    foreach (var spell in spells)
                    {
                        var list = new KeyValueListBase("monsters_spells");

                        list.AddPair("SpellId", spell.Id);
                        list.AddPair("Level", grade.GradeId <= spell.SpellLevelsIds.Count ? (int) grade.GradeId : spell.SpellLevelsIds.Count);

                        var subQueryId = 
                            SqlBuilder.BuildSelect(new [] { "Id" }, "monsters_grades",
                            "WHERE " + SqlBuilder.BuildWhere(new List<KeyValuePair<string, object>> {
                                new KeyValuePair<string, object>("MonsterId", monster.Id),
                                new KeyValuePair<string, object>("Grade", grade.GradeId)}));

                        list.AddPair("MonsterGradeId", RawData.Raw("(" + subQueryId + ")"));

                        string spellQuery = SqlBuilder.BuildInsert(list);

                        AppendQueryToPatch(m_monstersSpellsPatchWriter, spellQuery);
                    }
                }
            }

            Drop[] drops = dropParser.Parse(monsterData.dropslist);

            foreach (Drop drop in drops)
            {
                var list = new KeyValueListBase("monsters_drops");

                list.AddPair("MonsterOwnerId", monster.Id);
                list.AddPair("ItemId", drop.Id);
                list.AddPair("DropRate", drop.Percent.ToString(CultureInfo.InvariantCulture));
                list.AddPair("ProspectingLock", drop.Threshold.ToString(CultureInfo.InvariantCulture));

                string dropQuery = SqlBuilder.BuildInsert(list);
                AppendQueryToPatch(m_monstersDropsPatchWriter, dropQuery);
            }

            var where = new List<KeyValuePair<string, object>> {new KeyValuePair<string, object>("Id", monster.Id)};

            var updateList = new UpdateKeyValueList("monsters", where);
            updateList.AddPair("MinDroppedKamas", monsterData.goldmin);
            updateList.AddPair("MaxDroppedKamas", monsterData.goldmax);

            string query = SqlBuilder.BuildUpdate(updateList);
            AppendQueryToPatch(m_monstersPatchWriter, query);

            var evnt = MonsterAnalysed;

            if (evnt != null)
                evnt(this, monster, monsterData);
        }

        private TextWriter CreateSyncPatch(string path)
        {
            return TextWriter.Synchronized(new StreamWriter(File.Open(path, FileMode.Create)));
        }

        private void AppendQueryToPatch(TextWriter writer, string query)
        {
            writer.WriteLine(query + ";");
        }
    }
}
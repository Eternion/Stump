using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Stump.Core.Threading;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps
{
    public class SubArea
    {
        public static readonly Dictionary<Difficulty, double[]> MonsterGroupLengthProb =
            new Dictionary<Difficulty, double[]>
                {
                    {Difficulty.VeryEasy, new[] {2.2, 2.0, 1.6, 0.2, 0.1, 0.0, 0.0, 0.0}},
                    {Difficulty.Easy, new[] {1.8, 2.0, 1.8, 1.1, 0.4, 0.2, 0.0, 0.0}},
                    {Difficulty.Normal, new[] {1.3, 1.5, 1.5, 1.1, 0.6, 0.4, 0.0, 0.0}},
                    {Difficulty.Hard, new[] {0.8, 1.0, 1.5, 1.9, 1.3, 0.9, 0.5, 0.1}},
                    {Difficulty.VeryHard, new[] {0.2, 0.6, 1.2, 2.2, 1.9, 1.3, 1.1, 0.6}},
                    {Difficulty.Insane, new[] {0.1, 0.3, 0.6, 1.0, 1.7, 2.2, 1.7, 1.5}},
                };

        public static readonly Dictionary<Difficulty, double[]> MonsterGradeProb =
            new Dictionary<Difficulty, double[]>
                {
                    {Difficulty.VeryEasy, new[] {3.0, 2.0, 1.0, 0.8, 0.5}},
                    {Difficulty.Easy, new[] {2.5, 1.5, 1.5, 0.9, 0.6}},
                    {Difficulty.Normal, new[] {1.0, 1.0, 1.0, 1.0, 1.0}},
                    {Difficulty.Hard, new[] {0.6, 0.8, 1.0, 1.2, 1.2}},
                    {Difficulty.VeryHard, new[] {0.4, 0.6, 0.8, 1.0, 1.2}},
                    {Difficulty.Insane, new[] {0.1, 0.4, 0.6, 1.0, 2.0}},
                };

        public static readonly Dictionary<Difficulty, uint> MonsterSpawnInterval =
            new Dictionary<Difficulty, uint>
                {
                    {Difficulty.VeryEasy, 60},
                    {Difficulty.Easy, 90},
                    {Difficulty.Normal, 120},
                    {Difficulty.Hard, 160},
                    {Difficulty.VeryHard, 180},
                    {Difficulty.Insane, 220},
                };

        private readonly List<Map> m_maps = new List<Map>();
        private readonly List<MonsterSpawn> m_monsterSpawns = new List<MonsterSpawn>();
        private readonly Dictionary<Point, List<Map>> m_mapsByPoint = new Dictionary<Point, List<Map>>();

        public SubArea(SubAreaRecord record)
        {
            Record = record;
        }

        public SubAreaRecord Record
        {
            get;
            private set;
        }

        public int Id
        {
            get { return Record.Id; }
        }

        public IEnumerable<Map> Maps
        {
            get { return m_maps; }
        }

        public Dictionary<Point, List<Map>> MapsByPosition
        {
            get
            {
                return m_mapsByPoint;
            }
        }

        public Area Area
        {
            get;
            internal set;
        }

        public SuperArea SuperArea
        {
            get { return Area.SuperArea; }
        }

        public Difficulty Difficulty
        {
            get { return Record.Difficulty; }
            set { Record.Difficulty = value; }
        }

        public int SpawnsLimit
        {
            get { return Record.SpawnsLimit; }
            set { Record.SpawnsLimit = value; }
        }

        public uint? CustomSpawnInterval
        {
            get { return Record.CustomSpawnInterval; }
            set
            {
                Record.CustomSpawnInterval = value;
                RefreshMapsSpawnInterval();
            }
        }

        internal void AddMap(Map map)
        {
            m_maps.Add(map);

            if (!m_mapsByPoint.ContainsKey(map.Position))
                m_mapsByPoint.Add(map.Position, new List<Map>());

            m_mapsByPoint[map.Position].Add(map);

            map.SubArea = this;
        }

        internal void RemoveMap(Map map)
        {
            m_maps.Remove(map);

            if (m_mapsByPoint.ContainsKey(map.Position))
            {
                var list = m_mapsByPoint[map.Position];
                list.Remove(map);

                if (list.Count <= 0)
                    m_mapsByPoint.Remove(map.Position);
            }

            map.SubArea = null;
        }

        public Map[] GetMaps(int x, int y)
        {
            return GetMaps(new Point(x, y));
        }

        public Map[] GetMaps(int x, int y, bool outdoor)
        {
            return GetMaps(new Point(x, y), outdoor);
        }

        public Map[] GetMaps(Point position)
        {
            return !m_mapsByPoint.ContainsKey(position) ? new Map[0] : m_mapsByPoint[position].ToArray();
        }

        public Map[] GetMaps(Point position, bool outdoor)
        {
            return !m_mapsByPoint.ContainsKey(position) ? new Map[0] : m_mapsByPoint[position].Where(entry => entry.Outdoor == outdoor).ToArray();
        }

        public void AddMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Add(spawn);

            foreach (var map in Maps.Where(map => map.Outdoor))
            {
                map.AddMonsterSpawn(spawn);
            }
        }

        public void RemoveMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Remove(spawn);

            foreach (var map in Maps)
            {
                map.RemoveMonsterSpawn(spawn);
            }
        }

        public ReadOnlyCollection<MonsterSpawn> MonsterSpawns
        {
            get { return m_monsterSpawns.AsReadOnly(); }
        }

        public int RollMonsterLengthLimit(int imposedLimit = 8)
        {
            var difficulty = Difficulty;

            if (!MonsterGroupLengthProb.ContainsKey(difficulty))
                difficulty = Difficulty.Normal;

            var thresholds = MonsterGroupLengthProb[difficulty].Take(imposedLimit).ToArray();
            var sum = thresholds.Sum();

            var rand = new AsyncRandom();
            var roll = rand.NextDouble(0, sum);

            double l = 0;
            for (var i = 0; i < thresholds.Length; i++)
            {
                l += thresholds[i];

                if (roll <= l)
                    return i + 1;
            }

            return 1;
        }

        public int RollMonsterGrade(int minGrade, int maxGrade)
        {
            var difficulty = Difficulty;

            if (!MonsterGroupLengthProb.ContainsKey(difficulty))
                difficulty = Difficulty.Normal;

            var threshold = MonsterGroupLengthProb[difficulty].Skip(minGrade - 1).Take(maxGrade - minGrade + 1).ToArray();
            var sum = threshold.Sum();

            var rand = new AsyncRandom();
            var roll = rand.NextDouble(0, sum);

            double l = 0;
            for (var i = 0; i < threshold.Length; i++)
            {
                l += threshold[i];

                if (!(roll <= l))
                    continue;

                // in case of additional grades
                if (i < threshold.Length - 1 || maxGrade <= threshold.Length)
                    return i + 1;

                var secondRoll = rand.Next(0, maxGrade - threshold.Length + 1);

                return i + secondRoll + 1;
            }

            return 1;
        }

        public int GetMonsterSpawnInterval()
        {
            var difficulty = Difficulty;

            if (!MonsterSpawnInterval.ContainsKey(difficulty))
                difficulty = Difficulty.Normal;

            if (Record.CustomSpawnInterval.HasValue)
                return (int) Record.CustomSpawnInterval.Value;

            return (int) MonsterSpawnInterval[difficulty];
        }

        private void RefreshMapsSpawnInterval()
        {
            foreach (var pool in Maps.SelectMany(map => map.SpawningPools))
            {
                pool.SetTimer(GetMonsterSpawnInterval());
            }
        }
    }
}
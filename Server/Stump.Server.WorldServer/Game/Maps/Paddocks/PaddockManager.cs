using System.Collections.Generic;
using System.Linq;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;

namespace Stump.Server.WorldServer.Game.Maps.Paddocks
{
    public class PaddockManager : DataManager<PaddockManager>, ISaveable
    {
        private Dictionary<int, WorldMapPaddockRecord> m_paddockSpawns;
        private readonly List<Paddock> m_paddocks = new List<Paddock>();

        [Initialization(InitializationPass.Eighth)]
        public override void Initialize()
        {
            m_paddockSpawns = Database.Query<WorldMapPaddockRecord>(WorldMapPaddockRelator.FetchQuery).ToDictionary(entry => entry.Id);

            foreach (var spawn in m_paddockSpawns)
            {
                m_paddocks.Add(new Paddock(spawn.Value));
            }

            World.Instance.RegisterSaveableInstance(this);
        }

        public WorldMapPaddockRecord[] GetPaddockSpawns()
        {
            return m_paddockSpawns.Values.ToArray();
        }

        public WorldMapPaddockRecord GetPaddockSpawn(int mapId)
        {
            WorldMapPaddockRecord spawn;
            return m_paddockSpawns.TryGetValue(mapId, out spawn) ? spawn : null;
        }

        public Paddock GetPaddock(int mapId)
        {
            return m_paddocks.FirstOrDefault(x => x.Map.Id == mapId);
        }

        public Mount LoadMount(MountRecord record)
        {
            var mount = new Mount(record);
            var mountPaddock = Database.Query<MountPaddock>(string.Format(MountPaddockRelator.FetchByMountId, mount.Id)).FirstOrDefault();
            if (mountPaddock != null)
                mount.OwnerId = mountPaddock.CharacterId;

            return mount;
        }

        public void Save()
        {
            foreach (var paddock in m_paddocks.Where(paddock => paddock.IsRecordDirty))
            {
                paddock.Save();
            }
        }
    }
}

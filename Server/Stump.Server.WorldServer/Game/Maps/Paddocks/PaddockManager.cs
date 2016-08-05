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
        private Dictionary<int, Paddock> m_paddocks = new Dictionary<int, Paddock>();

        [Initialization(InitializationPass.Eighth)]
        public override void Initialize()
        {
            m_paddocks = Database.Query<WorldMapPaddockRecord, MountRecord, WorldMapPaddockRecord>(new WorldMapPaddockRelator().Map, WorldMapPaddockRelator.FetchQuery).ToDictionary(entry => entry.Id, x => new Paddock(x));

            World.Instance.RegisterSaveableInstance(this);
        }
        
        
        public Paddock GetPaddock(int id)
        {
            Paddock paddock;
            return m_paddocks.TryGetValue(id, out paddock) ? paddock : null;
        }

        public Paddock GetPaddockByMap(int mapId)
        {
            return m_paddocks.Values.FirstOrDefault(x => x.Map.Id == mapId);
        }

        public void Save()
        {
            // save only public paddocks, the others are saved with the guilds
            foreach (var paddock in m_paddocks.Where(paddock => paddock.Value.IsPublicPaddock() && paddock.Value.IsRecordDirty))
            {
                paddock.Value.Save(Database);
            }
        }
    }
}

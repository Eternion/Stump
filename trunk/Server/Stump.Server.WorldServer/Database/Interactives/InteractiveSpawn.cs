using Castle.ActiveRecord;
using Stump.Server.WorldServer.Worlds.Maps;

namespace Stump.Server.WorldServer.Database.Interactives
{
    [ActiveRecord("interactives_spawns")]
    public class InteractiveSpawn : WorldBaseRecord<InteractiveSpawn>
    {
        private Map m_map;

        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [BelongsTo]
        public InteractiveTemplate Template
        {
            get;
            set;
        }

        [Property]
        public int ElementId
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public int MapId
        {
            get;
            set;
        }

        public Map GetMap()
        {
            return m_map ?? (m_map = Worlds.World.Instance.GetMap(MapId));
        }
    }
}
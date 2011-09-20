using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs_spawns")]
    public class NpcSpawn : WorldBaseRecord<NpcSpawn>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("TemplateId", Cascade = CascadeEnum.Delete)]
        public NpcTemplate Template
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

        [Property(NotNull = true)]
        public int CellId
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public ObjectPosition GetPosition()
        {
            var map = Worlds.World.Instance.GetMap(MapId);

            if (map == null)
                throw new Exception(string.Format("Cannot load NpcSpawn id={0}, map {1} isn't found", Id, MapId));

            var cell = map.Cells[CellId];

            return new ObjectPosition(map, cell, Direction);
        }
    }
}
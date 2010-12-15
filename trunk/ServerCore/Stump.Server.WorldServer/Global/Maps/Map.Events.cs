using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global.Maps
{
    public partial class Map
    {
        public delegate void SpawnedEntityHandler(Map sender, Entity entity);

        public event SpawnedEntityHandler SpawnedEntity;

        private void NotifySpawnedEntity(Entity entity)
        {
            SpawnedEntityHandler handler = SpawnedEntity;

            if (handler != null)
                handler(this, entity);
        }

        public event SpawnedEntityHandler UnSpawnedEntity;

        public void NotifyUnSpawnedEntity(Entity entity)
        {
            SpawnedEntityHandler handler = UnSpawnedEntity;

            if (handler != null)
                handler(this, entity);
        }
    }
}

using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Game.Maps.Spawns
{
    public class StaticSpawningPool : SpawningPoolBase
    {
        public StaticSpawningPool(Map map, int interval)
            : base(map, interval)
        {
        }

        protected override bool IsLimitReached()
        {
            throw new NotImplementedException();
        }

        protected override int GetNextSpawnInterval()
        {
            throw new NotImplementedException();
        }

        protected override MonsterGroup DequeueNextGroupToSpawn()
        {
            throw new NotImplementedException();
        }
    }
}

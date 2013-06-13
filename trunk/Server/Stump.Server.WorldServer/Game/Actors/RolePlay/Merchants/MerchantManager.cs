using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using MerchantSpawn = Stump.Server.WorldServer.Database.World.WorldMapMerchantRecord;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants
{
    public class MerchantManager : DataManager<MerchantManager>
    {
        private Dictionary<int, MerchantSpawn> m_merchantSpawns;

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_merchantSpawns = Database.Query<MerchantSpawn>(WorldMapMerchantRelator.FetchQuery).ToDictionary(entry => entry.Id);
        }

        public MerchantSpawn[] GetMerchantSpawns()
        {
            return m_merchantSpawns.Values.ToArray();
        }

        public void AddMerchantSpawn(MerchantSpawn spawn)
        {
            Database.Insert(spawn);
            m_merchantSpawns.Add(spawn.Id, spawn);
        }

        public void RemoveMerchantSpawn(MerchantSpawn spawn)
        {
            Database.Delete(spawn);
            m_merchantSpawns.Remove(spawn.Id);
        }
    }
}

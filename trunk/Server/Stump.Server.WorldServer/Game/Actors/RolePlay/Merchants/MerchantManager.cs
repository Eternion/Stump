using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class MerchantManager : DataManager<MerchantManager>, ISaveable
    {
        private Dictionary<int, MerchantSpawn> m_merchantSpawns;
        private List<Merchant> m_activeMerchants = new List<Merchant>();
        private Stack<MerchantSpawn> m_addedSpawns = new Stack<MerchantSpawn>();
        private Stack<MerchantSpawn> m_removedSpawns = new Stack<MerchantSpawn>();


        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_merchantSpawns = Database.Query<MerchantSpawn>(WorldMapMerchantRelator.FetchQuery).ToDictionary(entry => entry.Id);
        }

        public MerchantSpawn[] GetMerchantSpawns()
        {
            return m_merchantSpawns.Values.ToArray();
        }

        public ReadOnlyCollection<Merchant> Merchants
        {
            get { return m_activeMerchants.AsReadOnly(); }
        }

        public void AddMerchantSpawn(MerchantSpawn spawn, bool lazySave = true)
        {
            if (lazySave)
                m_addedSpawns.Push(spawn);
            else
                Database.Insert(spawn);
            m_merchantSpawns.Add(spawn.Id, spawn);
        }

        public void RemoveMerchantSpawn(MerchantSpawn spawn, bool lazySave = true)
        {
            if (lazySave)
                m_removedSpawns.Push(spawn);
            else
                Database.Delete(spawn);
            m_merchantSpawns.Remove(spawn.Id);
        }

        public void RegisterMerchant(Merchant merchant)
        {
            m_activeMerchants.Add(merchant);
        }

        public void UnRegisterMerchant(Merchant merchant)
        {
            m_activeMerchants.Remove(merchant);
        }

        public void Save()
        {
            while (m_addedSpawns.Count > 0)
            {
                var spawn = m_addedSpawns.Pop();
                Database.Insert(spawn);
            }

            while (m_removedSpawns.Count > 0)
            {
                var spawn = m_removedSpawns.Pop();
                Database.Delete(spawn);
            }

            foreach (var merchant in m_activeMerchants)
            {
                if (merchant.Bag.IsDirty)
                    merchant.Save();
            }
        }
    }
}

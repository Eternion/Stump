﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Spells;
using MerchantSpawn = Stump.Server.WorldServer.Database.World.WorldMapMerchantRecord;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants
{
    public class MerchantManager : DataManager<MerchantManager>, ISaveable
    {
        private Dictionary<int, MerchantSpawn> m_merchantSpawns;
        private List<Merchant> m_activeMerchants = new List<Merchant>();


        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_merchantSpawns = Database.Query<MerchantSpawn>(WorldMapMerchantRelator.FetchQuery).ToDictionary(entry => entry.CharacterId);
            World.Instance.RegisterSaveableInstance(this);
        }

        public MerchantSpawn[] GetMerchantSpawns()
        {
            return m_merchantSpawns.Values.ToArray();
        }

        public MerchantSpawn GetMerchantSpawn(int characterId)
        {
            MerchantSpawn spawn;
            if (m_merchantSpawns.TryGetValue(characterId, out spawn))
                return spawn;

            return null;
        }

        public ReadOnlyCollection<Merchant> Merchants
        {
            get { return m_activeMerchants.AsReadOnly(); }
        }

        public void AddMerchantSpawn(MerchantSpawn spawn, bool lazySave = true)
        {
            if (lazySave)
                WorldServer.Instance.IOTaskPool.AddMessage(() => Database.Insert(spawn));
            else
                Database.Insert(spawn);
            m_merchantSpawns.Add(spawn.CharacterId, spawn);
        }

        public void RemoveMerchantSpawn(MerchantSpawn spawn, bool lazySave = true)
        {
            if (lazySave)
                WorldServer.Instance.IOTaskPool.AddMessage(() => Database.Delete(spawn));
            else
                Database.Delete(spawn);
            m_merchantSpawns.Remove(spawn.CharacterId);
        }

        public void ActiveMerchant(Merchant merchant)
        {
            merchant.Map.Enter(merchant);
            m_activeMerchants.Add(merchant);
        }

        public void UnActiveMerchant(Merchant merchant)
        {
            merchant.Delete();
            m_activeMerchants.Remove(merchant);
        }

        public IEnumerable<Merchant> UnActiveMerchantFromAccount(WorldAccount account)
        {
            var merchants = m_activeMerchants.Where(x => x.IsMerchantOwner(account)).ToArray();

            foreach (var merchant in merchants)
            {
                UnActiveMerchant(merchant);
                yield return merchant;
            }
        }

        public void Save()
        {
            foreach (var merchant in m_activeMerchants)
            {
                if (merchant.IsRecordDirty)
                    merchant.Save();
            }
        }
    }
}

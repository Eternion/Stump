using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Basic;
using TaxCollectorSpawn = Stump.Server.WorldServer.Database.World.WorldMapTaxCollectorRecord;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors
{
    public class TaxCollectorManager : DataManager<TaxCollectorManager>, ISaveable
    {
        private UniqueIdProvider m_idProvider;
        private Dictionary<int, TaxCollectorSpawn> m_taxCollectorSpawns;
        private readonly List<TaxCollectorNpc> m_activeTaxCollectors = new List<TaxCollectorNpc>();


        [Initialization(InitializationPass.Eighth)]
        public override void Initialize()
        {
            m_taxCollectorSpawns = Database.Query<TaxCollectorSpawn>(WorldMapTaxCollectorRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_idProvider = m_taxCollectorSpawns.Any() ? new UniqueIdProvider(m_taxCollectorSpawns.Select(x => x.Value.Id).Max()) : new UniqueIdProvider(1);
            World.Instance.RegisterSaveableInstance(this);

            World.Instance.SpawnTaxCollectors();
        }

        public TaxCollectorSpawn[] GetTaxCollectorSpawns()
        {
            return m_taxCollectorSpawns.Values.ToArray();
        }

        public TaxCollectorSpawn[] GetTaxCollectorSpawns(int guildId)
        {
            return m_taxCollectorSpawns.Values.Where(x => x.GuildId == guildId).ToArray();
        }

        public void AddTaxCollectorSpawn(Character character, bool lazySave = true)
        {
            if (character.Guild.TaxCollectors.Count() >= character.Guild.MaxTaxCollectors)
                return;

            if (character.Inventory.Kamas < character.Guild.HireCost)
                return;

            character.Inventory.SubKamas(character.Guild.HireCost);

            var taxCollectorNpc = new TaxCollectorNpc(m_idProvider.Pop(), character);

            if (lazySave)
                WorldServer.Instance.IOTaskPool.AddMessage(() => Database.Insert(taxCollectorNpc.Record));
            else
                Database.Insert(taxCollectorNpc.Record);

            m_taxCollectorSpawns.Add(taxCollectorNpc.Id, taxCollectorNpc.Record);
            taxCollectorNpc.Map.Enter(taxCollectorNpc);
        }

        public void RemoveTaxCollectorSpawn(TaxCollectorSpawn spawn, bool lazySave = true)
        {
            if (lazySave)
                WorldServer.Instance.IOTaskPool.AddMessage(() => Database.Delete(spawn));
            else
                Database.Delete(spawn);

            m_taxCollectorSpawns.Remove(spawn.Id);
        }

        public void Save()
        {
            foreach (var taxCollector in m_activeTaxCollectors.Where(taxCollector => taxCollector.IsRecordDirty))
            {
                taxCollector.Save();
            }
        }
    }
}

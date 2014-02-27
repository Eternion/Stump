using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.TaxCollector;
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
            if (!character.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_HIRE_TAX_COLLECTOR))
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NO_RIGHTS));
                return;
            }

            if (character.Guild.TaxCollectors.Count() >= character.Guild.MaxTaxCollectors)
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_MAX_REACHED));
                return;
            }

            if (character.Position.Map.TaxCollector != null)
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_ALREADY_ONE));
                return;
            }

            if (character.Inventory.Kamas < character.Guild.HireCost)
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NOT_ENOUGH_KAMAS));
                return;
            }

            if (!character.Position.Map.AllowCollector)
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_CANT_HIRE_HERE));
                return;
            }

            character.Inventory.SubKamas(character.Guild.HireCost);
            var position = character.Position.Clone();

            var taxCollectorNpc = new TaxCollectorNpc(m_idProvider.Pop(), position.Map.GetNextContextualId(), position, character.Guild, character.Name);

            if (lazySave)
                WorldServer.Instance.IOTaskPool.AddMessage(() => Database.Insert(taxCollectorNpc.Record));
            else
                Database.Insert(taxCollectorNpc.Record);

            character.Guild.AddTaxCollector(taxCollectorNpc);

            m_taxCollectorSpawns.Add(taxCollectorNpc.GlobalId, taxCollectorNpc.Record);
            m_activeTaxCollectors.Add(taxCollectorNpc);

            taxCollectorNpc.Map.Enter(taxCollectorNpc);

            //Le percepteur %1 a été posé en <b>%2</b> par <b>%3</b>.
            TaxCollectorHandler.SendTaxCollectorMovementMessage(taxCollectorNpc.Guild.Clients, true, taxCollectorNpc, character.Name);
            TaxCollectorHandler.SendTaxCollectorMovementAddMessage(taxCollectorNpc.Guild.Clients, taxCollectorNpc);
        }

        public void RemoveTaxCollectorSpawn(TaxCollectorNpc taxCollector, bool lazySave = true)
        {
            if (lazySave)
                WorldServer.Instance.IOTaskPool.AddMessage(() => Database.Delete(taxCollector.Record));
            else
                Database.Delete(taxCollector.Record);

            m_taxCollectorSpawns.Remove(taxCollector.GlobalId);
            m_activeTaxCollectors.Remove(taxCollector);

            taxCollector.Map.Leave(taxCollector);
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

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.TaxCollector;
using TaxCollectorSpawn = Stump.Server.WorldServer.Database.World.WorldMapTaxCollectorRecord;
using Stump.Core.Collections;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors
{
    public class TaxCollectorManager : DataManager<TaxCollectorManager>, ISaveable
    {
        private UniqueIdProvider m_idProvider;
        private Dictionary<int, TaxCollectorSpawn> m_taxCollectorSpawns;
        private readonly List<TaxCollectorNpc> m_activeTaxCollectors = new List<TaxCollectorNpc>();
        private Dictionary<int, TaxCollectorNamesRecord> m_taxCollectorNames;
        private Dictionary<int, TaxCollectorFirstnamesRecord> m_taxCollectorFirstnames;
        private readonly TimedStack<TaxCollectorSpawn> m_lastRemovedTaxCollectors = new TimedStack<TaxCollectorSpawn>(3600);

        [Initialization(InitializationPass.Eighth)]
        public override void Initialize()
        {
            m_taxCollectorSpawns = Database.Query<TaxCollectorSpawn>(WorldMapTaxCollectorRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_idProvider = m_taxCollectorSpawns.Any() ? new UniqueIdProvider(m_taxCollectorSpawns.Select(x => x.Value.Id).Max()) : new UniqueIdProvider(1);
            m_taxCollectorNames = Database.Query<TaxCollectorNamesRecord>(TaxCollectorNamesRelator.FetchQuery).ToDictionary(x => x.Id);
            m_taxCollectorFirstnames = Database.Query<TaxCollectorFirstnamesRecord>(TaxCollectorFirstnamesRelator.FetchQuery).ToDictionary(x => x.Id);

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

        public int GetRandomTaxCollectorFirstname()
        {
            return m_taxCollectorFirstnames.RandomElementOrDefault().Key;
        }

        public int GetRandomTaxCollectorName()
        {
            return m_taxCollectorNames.RandomElementOrDefault().Key;
        }

        public string GetTaxCollectorFirstName(int Id)
        {
            TaxCollectorFirstnamesRecord record;
            m_taxCollectorFirstnames.TryGetValue(Id, out record);

            return record == null ? "(no name)" : TextManager.Instance.GetText(record.FirstnameId);
        }

        public string GetTaxCollectorName(int Id)
        {
            TaxCollectorNamesRecord record;
            m_taxCollectorNames.TryGetValue(Id, out record);

            return record == null ? "(no name)" : TextManager.Instance.GetText(record.NameId);
        }

        public bool AddTaxCollectorSpawn(Character character)
        {
            if (!character.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_HIRE_TAX_COLLECTOR))
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NO_RIGHTS));
                return false;
            }

            if (character.Guild.TaxCollectors.Count >= character.Guild.MaxTaxCollectors)
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_MAX_REACHED));
                return false;
            }

            if (character.Position.Map.TaxCollector != null)
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_ALREADY_ONE));
                return false;
            }

            m_lastRemovedTaxCollectors.Clean();
            if (m_lastRemovedTaxCollectors.FirstOrDefault(x => x.First.GuildId == character.Guild.Id && x.First.MapId == character.Map.Id) != null)
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_CANT_HIRE_YET));
                return false;
            }

            if (!character.Position.Map.AllowCollector)
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_CANT_HIRE_HERE));
                return false;
            }

            if (character.IsInFight())
            {
                character.Client.Send(new TaxCollectorErrorMessage((sbyte)TaxCollectorErrorReasonEnum.TAX_COLLECTOR_ERROR_UNKNOWN));
                return false;
            }

            var position = character.Position.Clone();

            var taxCollectorNpc = new TaxCollectorNpc(m_idProvider.Pop(), position.Map.GetNextContextualId(), position, character.Guild, character);

            WorldServer.Instance.IOTaskPool.AddMessage(() => Database.Insert(taxCollectorNpc.Record));

            m_taxCollectorSpawns.Add(taxCollectorNpc.GlobalId, taxCollectorNpc.Record);
            m_activeTaxCollectors.Add(taxCollectorNpc);

            taxCollectorNpc.Map.Enter(taxCollectorNpc);
            character.Guild.AddTaxCollector(taxCollectorNpc);

            TaxCollectorHandler.SendTaxCollectorMovementMessage(taxCollectorNpc.Guild.Clients, TaxCollectorMovementTypeEnum.TAX_COLLECTOR_HIRED, taxCollectorNpc, character.Id, character.Name);

            return true;
        }

        public void RemoveTaxCollectorSpawn(TaxCollectorNpc taxCollector)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() => Database.Delete(taxCollector.Record));

            taxCollector.Bag.DeleteBag();

            m_taxCollectorSpawns.Remove(taxCollector.GlobalId);
            m_activeTaxCollectors.Remove(taxCollector);

            m_lastRemovedTaxCollectors.Push(taxCollector.Record);
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

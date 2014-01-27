using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Dialogs.TaxCollector;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items.TaxCollector;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors
{
    public class TaxCollectorNpc : NamedActor
    {
        public const string TAXCOLLECTOR_LOOK = "{714|||140}"; //todo: Find correct Look
        
        private readonly WorldMapTaxCollectorRecord m_record;
        private readonly List<TaxCollectorExchangeDialog> m_openedDialogs = new List<TaxCollectorExchangeDialog>();
        private string m_name;
        private ActorLook m_look;

        public TaxCollectorNpc(int id, ObjectPosition position, Guild guild)
        {
            var random = new AsyncRandom();

            Position = position;
            Guild = guild;
            Bag = new TaxCollectorBag(this);
            FightResult = new TaxCollectorFightResult(this);
            Guild.AddTaxCollector(this);

            m_record = new WorldMapTaxCollectorRecord
            {
                Id = id,
                Map = Position.Map,
                Cell = Position.Cell.Id,
                Direction = (int)Position.Direction,
                FirstNameId = (short)random.Next(1, 154),
                LastNameId = (short)random.Next(1, 253),
                GuildId = guild.Id,
            };
        }

        public TaxCollectorNpc(WorldMapTaxCollectorRecord record)
        {
            m_record = record;
            Bag = new TaxCollectorBag(this);

            if (record.MapId == null)
                throw new Exception("TaxCollector's map not found");

            Position = new ObjectPosition(
                record.Map,
                record.Map.Cells[m_record.Cell],
                (DirectionsEnum)m_record.Direction);


            Guild = GuildManager.Instance.TryGetGuild(Record.GuildId);
            Guild.AddTaxCollector(this);
        }

        public WorldMapTaxCollectorRecord Record
        {
            get
            {
                return m_record;
            }
        }

        public ReadOnlyCollection<TaxCollectorExchangeDialog> OpenDialogs
        {
            get { return m_openedDialogs.AsReadOnly(); }
        }

        public override int Id
        {
            get { return m_record.Id; }
            protected set { m_record.Id = value; }
        }

        public override string Name
        {
            get
            {
                return m_name ?? (m_name= string.Format("{0} {1}", TextManager.Instance.GetText(FirstNameId), TextManager.Instance.GetText(LastNameId)));
            }
        }

        public int GuildId
        {
            get { return m_record.GuildId; }
            protected set { m_record.GuildId = value; }
        }

        public Guild Guild
        {
            get;
            protected set;
        }

        public TaxCollectorBag Bag
        {
            get;
            protected set;
        }

        public override sealed ObjectPosition Position
        {
            get;
            protected set;
        }

        public override ActorLook Look
        {
            get
            {
                return m_look ?? (m_look = ActorLook.Parse(TAXCOLLECTOR_LOOK));
            }
        }

        public TaxCollectorFightResult FightResult
        {
            get;
            protected set;
        }

        public short FirstNameId
        {
            get { return m_record.FirstNameId; }
            protected set { m_record.FirstNameId = value; }
        }

        public short LastNameId
        {
            get { return m_record.LastNameId; }
            protected set { m_record.LastNameId = value; }
        }

        protected override void OnDisposed()
        {
            foreach (var dialog in OpenDialogs.ToArray())
            {
                dialog.Close();
            }

            base.OnDisposed();
        }

        public bool IsRecordDirty
        {
            get;
            set;
        }

        public bool IsBagEmpty()
        {
            return Bag.Count == 0;
        }

        public void LoadRecord()
        {
            Bag.LoadRecord();
        }

        public void Save()
        {
            WorldServer.Instance.IOTaskPool.EnsureContext();

            if (Bag.IsDirty)
                Bag.Save();

            WorldServer.Instance.DBAccessor.Database.Update(m_record);
        }

        public bool IsTaxCollectorOwner(Guilds.GuildMember member)
        {
            return member.Guild.Id == m_record.GuildId;
        }

        public void OnDialogOpened(TaxCollectorExchangeDialog dialog)
        {
            m_openedDialogs.Add(dialog);
        }

        public void OnDialogClosed(TaxCollectorExchangeDialog dialog)
        {
            m_openedDialogs.Remove(dialog);
            TaxCollectorManager.Instance.RemoveTaxCollectorSpawn(this);

            //<b>%3</b> a relevé la collecte sur le percepteur %1 en <b>%2</b> et recolté : %4
        }

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayTaxCollectorInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(), FirstNameId, LastNameId, Guild.GetGuildInformations(), Guild.Level, 0);
        }

        public TaxCollectorInformations GetNetworkTaxCollector()
        {
            return new TaxCollectorInformations(Id, FirstNameId, LastNameId, new AdditionalTaxCollectorInformations("", 0),
                (short)Position.Map.Position.X, (short)Position.Map.Position.Y, (short)Position.Map.SubArea.Id, 0, Look.GetEntityLook(), 0, 0, 0, 0);
        }

        public ExchangeGuildTaxCollectorGetMessage GetExchangeGuildTaxCollector()
        {
            return new ExchangeGuildTaxCollectorGetMessage(Name, (short)Position.Map.Position.X, (short)Position.Map.Position.Y, Position.Map.Id,
                (short)Position.Map.SubArea.Id, "", 0, new ObjectItemQuantity[0]);
        }

        public StorageInventoryContentMessage GetStorageInventoryContent()
        {
            return new StorageInventoryContentMessage(Bag.Select(x => x.GetObjectItem()), 0);
        }

        #endregion
    }
}
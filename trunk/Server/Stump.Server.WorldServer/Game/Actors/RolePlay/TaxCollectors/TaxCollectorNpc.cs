using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.TaxCollector;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items.TaxCollector;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.ORM.SubSonic.Extensions;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors
{
    public class TaxCollectorNpc : NamedActor
    {
        public const string TAXCOLLECTOR_LOOK = "{714|||140}"; //todo: Find correct Look
        
        private readonly WorldMapTaxCollectorRecord m_record;
        private readonly List<TaxCollectorExchangeDialog> m_openedDialogs = new List<TaxCollectorExchangeDialog>();

        public TaxCollectorNpc(int IdProvide, Character character)
        {
            Position = character.Position.Clone();

            m_record = new WorldMapTaxCollectorRecord
            {
                Id = IdProvide,
                Map = Position.Map,
                Cell = Position.Cell.Id,
                Direction = (int)Position.Direction,
                FirstNameId = (short)Numeric.Random(1, 154),
                LastNameId = (short)Numeric.Random(1, 253),
                GuildId = character.Guild.Id,
            };

            Guild = character.Guild;
            Bag = new TaxCollectorBag(this);

            Guild.AddTaxCollector(this);
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
                return string.Format("{0} {1}", TextManager.Instance.GetText(FirstNameId) + TextManager.Instance.GetText(LastNameId));
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

        public ActorLook Look
        {
            get
            {
                return ActorLook.Parse(TAXCOLLECTOR_LOOK);
            }
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

        public void BindGuild(Guild guild)
        {
            if (Guild != null)
                throw new Exception(string.Format("Guild already bound to TaxCollector {0}", Id));

            Guild = guild;
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
            var guild = GuildManager.Instance.TryGetGuild(GuildId);

            return new GameRolePlayTaxCollectorInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(), FirstNameId, LastNameId, guild.GetGuildInformations(), guild.Level, 0);
        }

        public TaxCollectorInformations GetNetworkTaxCollector()
        {
            return new TaxCollectorInformations(Id, FirstNameId, LastNameId, new AdditionalTaxCollectorInformations("", 0), (short)Position.Point.X, (short)Position.Point.Y, (short)Position.Map.SubArea.Id, 0, Look.GetEntityLook(), 0, 0, 0, 0);
        }

        public ExchangeGuildTaxCollectorGetMessage GetExchangeGuildTaxCollector()
        {
            return new ExchangeGuildTaxCollectorGetMessage(Name, (short)Position.Point.X, (short)Position.Point.Y, Position.Map.Id, (short)Position.Map.SubArea.Id, "", 0, new ObjectItemQuantity[0]);
        }

        public StorageInventoryContentMessage GetStorageInventoryContent()
        {
            return new StorageInventoryContentMessage(Bag.Select(x => x.GetObjectItem()), 0);
        }

        #endregion
    }
}
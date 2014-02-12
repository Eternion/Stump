using System;
using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors
{
    public class TaxCollectorNpc : NamedActor
    {
        public const string TAXCOLLECTOR_LOOK = "{714|||140}"; //todo: Find correct Look
        
        private readonly WorldMapTaxCollectorRecord m_record;

        public TaxCollectorNpc(int IdProvide, Character character)
        {
            m_record = new WorldMapTaxCollectorRecord
            {
                Id = IdProvide,
                Map = character.Map,
                Cell = character.Cell.Id,
                Direction = (int)character.Direction,
                FirstNameId = (short)FastRandom.Current.Next(1, 154),
                LastNameId = (short)FastRandom.Current.Next(1, 253),
                GuildId = character.Guild.Id,
            };

            Guild = character.Guild;
            Position = character.Position.Clone();

            Guild.AddTaxCollector(this);
        }

        public TaxCollectorNpc(WorldMapTaxCollectorRecord record)
        {
            m_record = record;

            if (record.MapId == null)
                throw new Exception("TaxCollector's map not found");

            Position = new ObjectPosition(
                record.Map,
                record.Map.Cells[m_record.Cell],
                (DirectionsEnum)m_record.Direction);

            var guild = GuildManager.Instance.TryGetGuild(Record.GuildId);
            guild.AddTaxCollector(this);
        }

        public WorldMapTaxCollectorRecord Record
        {
            get
            {
                return m_record;
            }
        }

        public override int Id
        {
            get { return m_record.Id; }
            protected set { m_record.Id = value; }
        }

        //public override string Name
        //{
        //    get
        //    {
        //        return TextManager.Instance.GetText(FirstNameId) + TextManager.Instance.GetText(LastNameId);
        //    }
        //}

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

        public uint KamasEarned
        {
            get { return m_record.KamasEarned; }
            set { m_record.KamasEarned = value; }
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

        public void Save()
        {
            WorldServer.Instance.DBAccessor.Database.Update(m_record);
        }

        public bool IsTaxCollectorOwner(Guilds.GuildMember member)
        {
            return member.Guild.Id == m_record.GuildId;
        }

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            var guild = GuildManager.Instance.TryGetGuild(GuildId);

            return new GameRolePlayTaxCollectorInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(), FirstNameId, LastNameId, guild.GetGuildInformations(), guild.Level, 0);
        }

        public TaxCollectorInformations GetNetworkTaxCollector()
        {
            return new TaxCollectorInformations(Id, FirstNameId, LastNameId, new AdditionalTaxCollectorInformations("", 0), (short)Position.Point.X, (short)Position.Point.Y, (short)Position.Map.SubArea.Id, 0, Look.GetEntityLook(), (int)KamasEarned, 0, 0, 0);
        }

        public ExchangeGuildTaxCollectorGetMessage GetExchangeGuildTaxCollector()
        {
            return new ExchangeGuildTaxCollectorGetMessage("", (short)Position.Point.X, (short)Position.Point.Y, Position.Map.Id, (short)Position.Map.SubArea.Id, "", 0, new ObjectItemQuantity[0]);
        }

        #endregion
    }
}
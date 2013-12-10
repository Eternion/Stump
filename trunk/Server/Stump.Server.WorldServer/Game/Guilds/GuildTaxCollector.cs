using System;
using ServiceStack.Text;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class GuildTaxCollector : NamedActor
    {
        public const short TAXCOLLECTOR_LOOK = 0;

        private readonly WorldMapTaxCollectorRecord m_record;

        public GuildTaxCollector(WorldMapTaxCollectorRecord record)
        {
            m_record = record;

            if (record.Map == null)
                throw new Exception("TaxCollector's map not found");

            Position = new ObjectPosition(
                record.Map,
                record.Map.Cells[m_record.Cell],
                (DirectionsEnum)m_record.Direction);

            EntityLook = new EntityLook { bonesId = TAXCOLLECTOR_LOOK };
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

        public EntityLook EntityLook
        {
            get;
            set;
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

        public bool IsTaxCollectorOwner(GuildMember member)
        {
            return member.Guild.Id == m_record.GuildId;
        }

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            var guild = GuildManager.Instance.TryGetGuild(GuildId);

            return new GameRolePlayTaxCollectorInformations(Id, EntityLook, GetEntityDispositionInformations(), FirstNameId, LastNameId, guild.GetGuildInformations(), guild.Level, 0);
        }

        public TaxCollectorInformations GetNetworkTaxCollector()
        {
            return new TaxCollectorInformations(Id, FirstNameId, LastNameId, new AdditionalTaxCollectorInformations("SpheX", (int)DateTime.Now.ToUnixTime()), (short)Position.Point.X, (short)Position.Point.Y, (short)Position.Map.SubArea.Id, 0, EntityLook, (int)KamasEarned, 0, 0, 0);
        }
        #endregion
    }
}

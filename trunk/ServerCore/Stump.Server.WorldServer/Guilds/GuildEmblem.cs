
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Guild;

namespace Stump.Server.WorldServer.World.Guilds
{
    public class GuildEmblem
    {

        public GuildEmblem(GuildEmblemRecord record)
        {
            m_record = record;
            SymbolShape = record.SymbolShape;
            SymbolColor = record.SymbolColor;
            BackgroundShape = record.BackgroundShape;
            BackgroundColor = record.BackgroundColor;
        }

        private readonly GuildEmblemRecord m_record;

        public int SymbolShape
        {
            get;
            set;
        }

        public int SymbolColor
        {
            get;
            set;
        }

        public int BackgroundShape
        {
            get;
            set;
        }

        public int BackgroundColor
        {
            get;
            set;
        }

        public void Save()
        {
            m_record.SymbolShape = SymbolShape;
            m_record.SymbolColor = SymbolColor;
            m_record.BackgroundShape = BackgroundShape;
            m_record.BackgroundColor = BackgroundColor;
            m_record.SaveAndFlush();
        }

        public DofusProtocol.Classes.GuildEmblem ToGuildEmblem()
        {
            return new DofusProtocol.Classes.GuildEmblem(SymbolShape, SymbolColor, BackgroundShape,BackgroundColor);
        }

    }
}

using Stump.Database.WorldServer;
using Stump.Database.WorldServer.House;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Entities
{
    public class House
    {

        public House(HouseRecord record)
        {
          record.
        }

        private readonly HouseRecord m_record;

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

        public DofusProtocol.Classes.HouseInformations ToGuildEmblem()
        {
            return new Stump.DofusProtocol.Classes.HouseInformations();
        }

    }
}
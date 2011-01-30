// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using Stump.Database.WorldServer;
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
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
namespace Stump.Server.DataProvider.Data.Map
{
    public class MapCapabilities
    {

        public MapCapabilities(int capabilities)
        {
            m_capabilities = capabilities;
        }

        private readonly int m_capabilities;

        public bool AllowChallenge
        {
            get { return (m_capabilities & 1) != 0; }
        }

        public bool AllowAggression
        {
            get { return (m_capabilities & 2) != 0; }
        }

        public bool AllowTeleportTo
        {
            get { return (m_capabilities & 4) != 0; }
        }

        public bool AllowTeleportFrom
        {
            get { return (m_capabilities & 8) != 0; }
        }

        public bool AllowExchangeBetweenPlayers
        {
            get { return (m_capabilities & 16) != 0; }
        }

        public bool AllowHumanVendor
        {
            get { return (m_capabilities & 32) != 0; }
        }

        public bool AllowCollector
        {
            get { return (m_capabilities & 64) != 0; }
        }

        public bool AllowSoulCapture
        {
            get { return (m_capabilities & 128) != 0; }
        }

        public bool AllowSoulSummon
        {
            get { return (m_capabilities & 256) != 0; }
        }

        public bool AllowTavernRegen
        {
            get { return (m_capabilities & 512) != 0; }
        }

        public bool AllowTombMode
        {
            get { return (m_capabilities & 1024) != 0; }
        }

        public bool TeleportEverywhere
        {
            get { return (m_capabilities & 2048) != 0; }
        }

        public bool AllowFightChallenge
        {
            get { return (m_capabilities & 4096) != 0; }
        }

    }
}
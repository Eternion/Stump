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
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Data.TaxCollectorData
{
    public static class TaxCollectorDataProvider
    {

        private static Dictionary<uint, string> m_taxCollectorNames;

        private static Dictionary<uint, string> m_taxCollectorFirstNames;

        private static readonly Random m_rnd = new Random();


        [StageStep(Stages.One, "Loaded TaxCollectors Name")]
        public static void LoadTaxCollectorName()
        {
            var names = DataLoader.LoadData<TaxCollectorName>();

            foreach (var name in names)
                m_taxCollectorNames.Add(name.nameId, DataLoader.GetI18NText(name.nameId));
        }

        [StageStep(Stages.One, "Loaded TaxCollectors FirstName")]
        public static void LoadTaxCollectorFirstName()
        {
            var firstNames = DataLoader.LoadData<TaxCollectorFirstname>();

            foreach (var firstName in firstNames)
                m_taxCollectorFirstNames.Add(firstName.firstnameId, DataLoader.GetI18NText(firstName.firstnameId));
        }

        public static KeyValuePair<uint,string> GetRandomName()
        {
            int nbr = m_rnd.Next(m_taxCollectorNames.Count);

            return m_taxCollectorNames.ElementAt(nbr);
        }

        public static KeyValuePair<uint, string> GetRandomFirstName()
        {
            int nbr = m_rnd.Next(m_taxCollectorFirstNames.Count);

            return m_taxCollectorFirstNames.ElementAt(nbr);
        }

        public static string GetFullName(uint firstNameId, uint lastNameId)
        {
            if (m_taxCollectorFirstNames.ContainsKey(firstNameId) && m_taxCollectorNames.ContainsKey(lastNameId))
                return m_taxCollectorNames[lastNameId] + " " + m_taxCollectorFirstNames[firstNameId];
            return "null";
        }

    }
}





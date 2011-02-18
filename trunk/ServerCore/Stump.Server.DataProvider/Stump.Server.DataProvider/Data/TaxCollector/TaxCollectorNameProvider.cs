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
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.TaxCollector
{
    public class TaxCollectorNameProvider : DataProvider<int,string>
    {
        private static readonly AsyncRandom m_rnd = new AsyncRandom();

        protected override string GetData(int id)
        {
            return D2OLoader.GetI18NText(D2OLoader.LoadData<TaxCollectorName>(id).nameId);
        }

        protected override Dictionary<int, string> GetAllData()
        {
            return D2OLoader.LoadData<TaxCollectorName>().ToDictionary(t=>t.id, t=>D2OLoader.GetI18NText(t.nameId));
        }

        /// <summary>
        /// Can be only used with pre-loading
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<int, string> GetRandom()
        {
            return m_dico.ElementAt(m_rnd.NextInt(m_dico.Count));
        }
    }
}





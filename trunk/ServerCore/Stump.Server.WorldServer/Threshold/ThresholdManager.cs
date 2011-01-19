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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Stump.Database;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.WorldServer.Data;

namespace Stump.Server.WorldServer.Threshold
{
    public static class ThresholdManager
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static Dictionary<string, ThresholdDictionnary> m_thresholds = new Dictionary<string, ThresholdDictionnary>();

        public static Dictionary<string, ThresholdDictionnary> Thresholds
        {
            get { return m_thresholds; }
        }

        public static ThresholdDictionnary CharacterExp
        {
            get
            {
                return m_thresholds["CharacterExp"];
            }
        }

        [StageStep(Stages.One, "Loaded Thresholds")]
        public static void LoadThresholds()
        {
            m_thresholds = ThresholdLoader.LoadThresholds();
        }

    }
}
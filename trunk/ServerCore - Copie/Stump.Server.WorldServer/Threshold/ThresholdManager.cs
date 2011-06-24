
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
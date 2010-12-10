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
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Win32.Targets;

namespace Stump.BaseCore.Framework.IO
{
    public static class NLogHelper
    {
        /// <summary>
        ///   Directory where logs are stored
        /// </summary>
        /// <remarks>
        ///   Don't put a dot (.) at the begin
        /// </remarks>
        public static readonly string LogFilePath = "/logs/"; //  /logs/ = ./logs/

        /// <summary>
        ///   Defines the log profile.
        /// </summary>
        /// <param name = "activefileLog">if set to <c>true</c> logs by file are actived.</param>
        /// <param name = "activeconsoleLog">if set to <c>true</c> logs on the console are actived.</param>
        public static void DefineLogProfile(bool activefileLog, bool activeconsoleLog)
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            consoleTarget.Layout = "<${date:format=HH\\:mm\\:ss}> [${level}] ${message}";

            var fileTarget = new FileTarget();
            fileTarget.FileName = "${basedir}" + LogFilePath + "log" + ".txt";
            fileTarget.Layout = "<${date:format=HH\\:mm\\:ss}> [${level}] ${message}";

            if (activefileLog)
                config.AddTarget("file", fileTarget);

            if (activeconsoleLog)
                config.AddTarget("console", consoleTarget);

            if (activeconsoleLog)
            {
                var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule);
            }

            if (activefileLog)
            {
                var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
                config.LoggingRules.Add(rule);
            }

            LogManager.Configuration = config;
        }

        /// <summary>
        ///   Defines the log profile.
        /// </summary>
        /// <param name = "activefileLog">if set to <c>true</c> logs by file are actived.</param>
        /// <param name = "activeconsoleLog">if set to <c>true</c> logs on the console are actived.</param>
        /// <param name = "logName">Name of the log file.</param>
        public static void DefineLogProfile(bool activefileLog, bool activeconsoleLog, string logName)
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            consoleTarget.Layout = "<${date:format=HH\\:mm\\:ss}> [${level}] ${message}";

            var fileTarget = new FileTarget();
            fileTarget.FileName = "${basedir}" + LogFilePath + logName + ".txt";
            fileTarget.Layout = "<${date:format=HH\\:mm\\:ss}> [${level}] ${message}";

            if (activefileLog)
                config.AddTarget("file", fileTarget);

            if (activeconsoleLog)
                config.AddTarget("console", consoleTarget);

            if (activeconsoleLog)
            {
                var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule);
            }

            if (activefileLog)
            {
                var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
                config.LoggingRules.Add(rule);
            }

            LogManager.Configuration = config;
        }

        /// <summary>
        ///   Enable the logging.
        /// </summary>
        public static void EnableLogging()
        {
            LogManager.EnableLogging();
        }

        /// <summary>
        ///   Disable the logging.
        /// </summary>
        public static void DisableLogging()
        {
            LogManager.DisableLogging();
        }
    }
}
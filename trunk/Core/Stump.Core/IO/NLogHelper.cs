using NLog;
using NLog.Config;
using NLog.Targets;

namespace Stump.Core.IO
{
    public static class NLogHelper
    {
        public static string LogFormatConsole = "[${blockcenter:length=18:inner=${callsite:className=true:methodName=false:includeSourcePath=false:nonamespace=true}}](${threadid}) ${message}";
        public static string LogFormatFile = "[${level}] <${date:format=G}> ${message}";

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

            var consoleTarget = new ColoredConsoleTarget {Layout = LogFormatConsole};

            var fileTarget = new FileTarget
            {
                FileName = "${basedir}" + LogFilePath + "log_${date:format=dd-MM-yyyy}" + ".txt",
                Layout = LogFormatFile
            };

            var fileErrorTarget = new FileTarget
            {
                FileName = "${basedir}" + LogFilePath +
                           "error_${date:format=dd-MM-yyyy}" + ".txt",
                Layout =
                    "-------------${level} at ${date:format=G}------------- ${newline} ${callsite} -> ${newline}\t${message} ${newline}-------------${level} at ${date:format=G}------------- ${newline}"
            };

            if (activefileLog)
            {
                config.AddTarget("file", fileTarget);
                config.AddTarget("fileErrors", fileErrorTarget);
            }

            if (activeconsoleLog)
                config.AddTarget("console", consoleTarget);

#if DEBUG
            var level = LogLevel.Debug;
#else
            var level = LogLevel.Info;
#endif

            if (activeconsoleLog)
            {
                var rule = new LoggingRule("*", level, consoleTarget);
                config.LoggingRules.Add(rule);
            }

            if (activefileLog)
            {
                var rule = new LoggingRule("*", level, fileTarget);
                rule.DisableLoggingForLevel(LogLevel.Fatal);
                rule.DisableLoggingForLevel(LogLevel.Error);
                config.LoggingRules.Add(rule);

                var errorRule = new LoggingRule("*", LogLevel.Warn, fileErrorTarget);
                config.LoggingRules.Add(errorRule);
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
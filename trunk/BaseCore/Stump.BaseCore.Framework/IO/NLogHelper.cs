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

        public static readonly string LogErrorFilePath = "/errors/";

        /// <summary>
        ///   Defines the log profile.
        /// </summary>
        /// <param name = "activefileLog">if set to <c>true</c> logs by file are actived.</param>
        /// <param name = "activeconsoleLog">if set to <c>true</c> logs on the console are actived.</param>
        public static void DefineLogProfile(bool activefileLog, bool activeconsoleLog)
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            consoleTarget.Layout = "<${date:format=HH\\:mm\\:ss}> [${level}] ${stacktrace} : ${message}";

            var fileTarget = new FileTarget();
            fileTarget.FileName = "${basedir}" + LogFilePath + "log_<${date:format=dd-MM-yyyy}>" + ".txt";
            fileTarget.Layout = "<${date:format=G}> [${level}] ${callsite} : ${message}";

            var fileErrorTarget = new FileTarget();
            fileErrorTarget.FileName = "${basedir}" + LogFilePath + LogErrorFilePath +
                                       "error_<${date:format=dd-MM-yyyy}>" + ".txt";
            fileErrorTarget.Layout = "-------------${level} at ${date:format=G}------------- ${newline} ${callsite} : ${message} ${newline} -------------${level} at ${date:format=G}------------- ${newline}";

            if (activefileLog)
            {
                config.AddTarget("file", fileTarget);
                config.AddTarget("file_errors", fileErrorTarget);
            }

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
                rule.DisableLoggingForLevel(LogLevel.Fatal);
                rule.DisableLoggingForLevel(LogLevel.Error);
                config.LoggingRules.Add(rule);

                rule = new LoggingRule("*", LogLevel.Warn, fileErrorTarget);
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
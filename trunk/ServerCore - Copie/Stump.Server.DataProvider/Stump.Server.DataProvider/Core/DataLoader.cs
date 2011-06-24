using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using Stump.Core.Attributes;

namespace Stump.Server.DataProvider.Core
{
    public static class DataLoader
    {
        private static readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///  Contains all of params of the providers
        ///  LoadingType :  - PreLoading    : Load the entire file and store it into memory (Memory--/Speed++)
        ///                 - CacheLoading  : Store into memory during a defined time objects that are requested once (Memory+-/Speed+-)
        ///  
        ///  LifeTime (CacheLoading only)   : Life time of a cached object (in seconds)
        ///  CheckTime (CacheLoading only)  : Time between each check cycle to make the cache clean (in seconds)
        /// </summary>
        [Variable]
        public static List<DataManagerParams> DataManagerParams = new List<DataManagerParams>();

        public static void Initialize()
        {
            Dictionary<string, DataManagerParams> @params = DataManagerParams.ToDictionary(p => p.ProviderType);

            Assembly asm = Assembly.GetExecutingAssembly();

            Dictionary<Type, DataManagerAttribute> managers =
                asm.GetTypes().Where(
                    entry => entry.GetCustomAttributes(typeof (DataManagerAttribute), false).Count() > 0).
                    ToDictionary(entry => entry,
                                 entry =>
                                 (DataManagerAttribute)
                                 entry.GetCustomAttributes(typeof (DataManagerAttribute), false).FirstOrDefault());

            foreach (var manager in managers.OrderByDescending(entry => entry.Value.LoadPriority))
            {
                FieldInfo field = manager.Key.GetField("Instance");

                if (field != null)
                {
                    object instance = field.GetValue(null);
                    DataManagerParams managerParams = @params.ContainsKey(manager.Key.Name)
                                                          ? @params[manager.Key.Name]
                                                          : new DataManagerParams
                                                                {LoadingType = LoadingType.PreLoading};

                    m_logger.Info(manager.Value.LoadingMessage + "(" + managerParams.LoadingType + ")");

                    manager.Key.GetMethod("Initialize").Invoke(instance, new object[] {managerParams});
                }
            }
        }
    }
}
using Stump.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SharpRaven.Data;

namespace Stump.Server.BaseServer.Exceptions
{
    public class ExceptionManager : Singleton<ExceptionManager>
    {
        private readonly List<Exception> m_exceptions = new List<Exception>();

        public ReadOnlyCollection<Exception> Exceptions => m_exceptions.AsReadOnly();

        public static void RegisterException(Exception ex)
        {
            if (ServerBase.IsExceptionLoggerEnabled)
            {
                ServerBase.InstanceAsBase.ExceptionLogger.Capture(new SentryEvent(ex));
            }
            //m_exceptions.Add(ex);
        }
    }
}
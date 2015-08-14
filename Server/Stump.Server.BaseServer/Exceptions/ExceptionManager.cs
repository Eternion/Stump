using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SharpRaven.Data;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.Exceptions
{
    public class ExceptionManager : Singleton<ExceptionManager>
    {
        private readonly List<Exception> m_exceptions = new List<Exception>();

        public ReadOnlyCollection<Exception> Exceptions
        {
            get { return m_exceptions.AsReadOnly(); }
        }

        public void RegisterException(Exception ex)
        {
            if (ServerBase.IsExceptionLoggerEnabled)
            {
                var message = ex.Message;
                var stackTrace = ex.StackTrace;

                if (ex.InnerException != null)
                {
                    message += "\r\nInnerException : " + ex.InnerException.Message;
                    stackTrace += "\r\nInnerException : " + ex.InnerException.StackTrace;
                }
                ServerBase.InstanceAsBase.ExceptionLogger.CaptureException(ex, new SentryMessage(message + "\r\n" + stackTrace));
            }
            //m_exceptions.Add(ex);
        }
    }
}
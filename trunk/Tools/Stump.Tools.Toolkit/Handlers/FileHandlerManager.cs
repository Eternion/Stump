using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Tools.Toolkit.Handlers
{
    public class FileHandlerManager : Singleton<FileHandlerManager>
    {
        private readonly Dictionary<string, Func<IFileHandler>> m_handlers = new Dictionary<string, Func<IFileHandler>>();

        public void Initialize()
        {
            var handlers = ( from entry in Assembly.GetExecutingAssembly().GetTypes()
                             let attr = entry.GetCustomAttribute<FileHandlerAttribute>()
                             where attr != null
                             select new KeyValuePair<Type, string>(entry, attr.FileExt) );

            foreach (var handler in handlers)
            {
                RegisterHandler(handler.Key, handler.Value);
            }
        }

        public void RegisterHandler(Type handler, string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt))
                throw new ArgumentException("fileExt");


            if (!handler.HasInterface(typeof(IFileHandler)))
                throw new InvalidOperationException(string.Format("Invalid file handler {0}, must implement IFileHandler", handler.Name));

            var ctor = handler.GetConstructor(new Type[0]);

            if (ctor == null)
                throw new InvalidOperationException(string.Format("Invalid file handler {0}, no ctor() not found", handler.Name));

            var deleg = ctor.CreateDelegate<Func<IFileHandler>>();

            m_handlers.Add(fileExt.ToLower(), deleg);
        }

        public bool CanHandle(string fileExt)
        {
            return m_handlers.ContainsKey(fileExt.ToLower());
        }

        public IFileHandler TryGetHandler(string fileExt)
        {
            if (!CanHandle(fileExt))
                return null;

            return m_handlers[fileExt.ToLower()]();
        }
    }
}
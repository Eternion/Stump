
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using NLog;
using Stump.BaseCore.Framework.Reflection;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.BaseServer.Handler
{
    public sealed class HandlerManager
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Key : Typeof handled message
        ///   Value : Target method
        /// </summary>
        private readonly Dictionary<Type, Tuple<IHandlerContainer, Handler, Action<BaseClient,Message>>> m_handlers =
            new Dictionary<Type, Tuple<IHandlerContainer, Handler, Action<BaseClient,Message>>>();

        /// <summary>
        ///   Automatically detects and registers all PacketHandlers within the given Assembly
        /// </summary>
        public void RegisterAll(Assembly asm)
        {
            // Register all the packet handlers in the given assembly
            foreach (Type asmType in asm.GetTypes())
            {
                Register(asmType);
            }
        }

        /// <summary>
        ///   Registers all packet handlers defined in the given type.
        /// </summary>
        /// <param name = "type">the type to search through for packet handlers</param>
        private void Register(Type type)
        {
            if(type.IsAbstract || !type.GetInterfaces().Contains(typeof(IHandlerContainer)))
                return;
            
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            IHandlerContainer handlerContainer;

            try
            {
                handlerContainer = (IHandlerContainer)Activator.CreateInstance(type, true);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create HandlerContainer " + type.Name +
                                    ".\n " + e.Message);
            }

            foreach (MethodInfo method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof (Handler), false) as Handler[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                try
                {
                    if (
                        method.GetParameters().Count(
                            entry =>
                            (entry.ParameterType.IsSubclassOf(typeof(Message)) ||
                             entry.ParameterType.IsSubclassOf(typeof(BaseClient)))) != 2)
                        throw new ArgumentException("Incorrect delegate parameters");

                    var handlerDelegate = method.CreateDelegate(typeof(BaseClient),typeof(Message))  as Action<BaseClient,Message>;
                    
                    foreach (var attribute in attributes)
                    {
                        RegisterHandler(attribute.Message, handlerContainer, attribute, handlerDelegate);
                    }
                }
                catch (Exception e)
                {
                    string handlerStr = type.FullName + "." + method.Name;
                    throw new Exception("Unable to register PacketHandler " + handlerStr +
                                        ".\n Make sure its arguments are: " + typeof (BaseClient).FullName + ", " +
                                        typeof (Message).FullName +
                                        ".\n" + e.Message);
                }
            }
        }

        private void RegisterHandler(Type msgType, IHandlerContainer handlerContainer, Handler handler, Action<BaseClient,Message> target)
        {
            if (m_handlers.ContainsKey(msgType))
            {
                m_logger.Debug(string.Format("Packet handler {0} already registered ! Func : {1} ", msgType, target));
            }

            m_handlers.Add(msgType,
                           target != null
                               ? new Tuple<IHandlerContainer, Handler, Action<BaseClient, Message>>(handlerContainer, handler, target)
                               : new Tuple<IHandlerContainer, Handler, Action<BaseClient, Message>>(handlerContainer, handler, null));
        }

        public bool IsRegister(Type msgType)
        {
            return m_handlers.ContainsKey(msgType);
        }

        public void Dispatch(BaseClient client, Message message)
        {
            var messageType = message.GetType();
            Tuple<IHandlerContainer, Handler, Action<BaseClient,Message>> handler;
            if (m_handlers.TryGetValue(messageType, out handler))
            {
                try
                {
                    if (!handler.Item1.PredicateSuccess(client, messageType))
                    {
                        m_logger.Warn(client + " tried to send " + message + " but predicate didn't success");
                        return;
                    }

#if DEBUG
                    Console.WriteLine(string.Format("{0} << {1}", client, message.GetType().Name));
#endif

                    handler.Item3(client, message);
                }
                catch (Exception ex)
                {
                    m_logger.Error(string.Format("[Handler : {0}] Force disconnection of client {1} : {2}", messageType.Name, client, ex));
                    client.Disconnect();
                }
            }
            else
            {
                m_logger.Debug("Received Unknown packet : " + messageType.Name);
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("****HandlerManager*****");

            result.AppendLine("Available Handlers Count : " + m_handlers.Count);

            foreach (var handler in m_handlers)
                result.AppendLine(handler.Key.ToString());

            return result.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.BaseServer.Handler
{
    public class HandlerManager<THandler, TAttribute, TContainer, TClient> : Singleton<THandler>
        where THandler : HandlerManager<THandler, TAttribute, TContainer, TClient> 
        where TContainer : IHandlerContainer
        where TAttribute : HandlerAttribute
        where TClient : BaseClient
    {
        protected class MessageHandler
        {
            public MessageHandler(TContainer container, TAttribute handlerAttribute, Action<TClient, Message> action)
            {
                Container = container;
                Attribute = handlerAttribute;
                Action = action;
            }

            public TContainer Container
            {
                get;
                private set;
            }

            public TAttribute Attribute
            {
                get;
                private set;
            }

            public Action<TClient, Message> Action
            {
                get;
                private set;
            }
        }

        protected readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Key : Typeof handled message
        ///   Value : Target method
        /// </summary>
        protected readonly Dictionary<uint, MessageHandler> m_handlers = new Dictionary<uint, MessageHandler>();

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
            if (type.IsAbstract || !(type.GetInterfaces().Contains(typeof(TContainer)) || type.IsSubclassOf(typeof(TContainer))))
                return;
            
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            TContainer handlerContainer;

            try
            {
                handlerContainer = (TContainer)Activator.CreateInstance(type, true);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create HandlerContainer " + type.Name +
                                    ".\n " + e.Message);
            }

            foreach (MethodInfo method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(TAttribute), false) as TAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                try
                {
                    if (
                        method.GetParameters().Count(
                            entry =>
                            (entry.ParameterType.IsSubclassOf(typeof(Message)) ||
                             (entry.ParameterType == typeof(TClient) || entry.ParameterType.IsSubclassOf(typeof(TClient))) )) != 2)
                        throw new ArgumentException("Incorrect delegate parameters");

                    var handlerDelegate = method.CreateDelegate(typeof(TClient), typeof(Message)) as Action<TClient, Message>;
                    
                    foreach (var attribute in attributes)
                    {
                        RegisterHandler(attribute.MessageId, handlerContainer, attribute, handlerDelegate);
                    }
                }
                catch (Exception e)
                {
                    string handlerStr = type.FullName + "." + method.Name;
                    throw new Exception("Unable to register PacketHandler " + handlerStr +
                                        ".\n Make sure its arguments are: " + typeof(TClient).FullName + ", " +
                                        typeof (Message).FullName +
                                        ".\n" + e.Message);
                }
            }
        }

        private void RegisterHandler(uint messageId, TContainer handlerContainer, TAttribute handlerAttribute, Action<TClient, Message> target)
        {
            if (m_handlers.ContainsKey(messageId))
            {
                m_logger.Debug(string.Format("Packet handler {0} already registered ! Func : {1} ", MessageReceiver.GetMessageType(messageId), target));
            }

            m_handlers.Add(messageId,
                           target != null
                               ? new MessageHandler(handlerContainer, handlerAttribute, target)
                               : new MessageHandler(handlerContainer, handlerAttribute, null));
        }

        public bool IsRegister(uint messageId)
        {
            return m_handlers.ContainsKey(messageId);
        }

        public virtual void Dispatch(TClient client, Message message)
        {
            MessageHandler handler;
            if (m_handlers.TryGetValue(message.MessageId, out handler))
            {
                try
                {
                    if (!handler.Container.CanHandleMessage(client, message.MessageId))
                    {
                        m_logger.Warn(client + " tried to send " + message + " but predicate didn't success");
                        return;
                    }

                    ServerBase.InstanceAsBase.IOTaskPool.AddMessage(() => handler.Action(client, message));
                }
                catch (Exception ex)
                {
                    m_logger.Error(string.Format("[Handler : {0}] Force disconnection of client {1} : {2}", message, client, ex));
                    client.Disconnect();
                }
            }
            else
            {
                m_logger.Debug("Received Unknown packet : " + message);
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
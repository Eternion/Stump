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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using Stump.BaseCore.Framework.Extensions;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.BaseServer.Handler
{
    public sealed class HandlerManager
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Key : Typeof handled message
        ///   Value : Target method
        /// </summary>
        private readonly Dictionary<Type, Tuple<IHandlerContainer, Handler, Delegate>> m_handlers =
            new Dictionary<Type, Tuple<IHandlerContainer, Handler, Delegate>>();

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
        public void Register(Type type)
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
                object[] attributes = method.GetCustomAttributes(
                    typeof (Handler), false);


                if (attributes.Length == 0)
                    continue;

                try
                {
                    if (
                        method.GetParameters().Count(
                            entry =>
                            (entry.ParameterType.IsSubclassOf(typeof (Message)) ||
                             entry.ParameterType.IsSubclassOf(typeof (BaseClient)))) != 2)
                        throw new ArgumentException("Incorrect delegate parameters");

                    Delegate handlerDelegate = Delegate.CreateDelegate(method.GetActionType(), method);
                    

                    foreach (object attribute in attributes)
                    {
                        RegisterHandler(((Handler) attribute).Message, handlerContainer, (Handler) attribute, handlerDelegate);
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

        internal void RegisterHandler(Type msgType, IHandlerContainer handlerContainer, Handler handler, Delegate target)
        {
            if (m_handlers.ContainsKey(msgType))
            {
                logger.Debug(string.Format("Packet handler {0} already registered ! Func : {1} ", msgType, target));
            }

            m_handlers.Add(msgType,
                           target != null
                               ? new Tuple<IHandlerContainer, Handler, Delegate>(handlerContainer, handler, target)
                               : new Tuple<IHandlerContainer, Handler, Delegate>(handlerContainer, handler, null));
        }

        public bool IsRegister(Type msgType)
        {
            return m_handlers.ContainsKey(msgType);
        }


        public void Dispatch(BaseClient client, Message message)
        {
            Tuple<IHandlerContainer, Handler, Delegate> handler;
            if (m_handlers.TryGetValue(message.GetType(), out handler))
            {
                try
                {
                    //////////////////////////////////////////////////////////////////////////
                    // The following should be benchmarked.
                    // What is the best when you have to handle a large amount of packets
                    // Parallel stuff sounds good but need a serious benchmarking
                    // since this is some kind of critical stuff we have there...
                    //////////////////////////////////////////////////////////////////////////

                    if (!handler.Item1.PredicateSuccess(client, message.GetType()))
                    {
                        logger.Warn(client + " tried to send " + message + " but predicate didn't success");
                        return;
                    }

#if DEBUG
                    Console.WriteLine(string.Format("{0} << {1}", client, message.GetType().Name));
#endif

                    handler.Item3.DynamicInvoke(client, message);

                    /*handler.Item3.GetInvocationList()
                    .AsParallel().AsOrdered()
                    .Select(d => d.DynamicInvoke(client, message));*/
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("[Handler : {0}] Force disconnection of client {1} : {2}", message.GetType().Name, client, ex));
                    client.Disconnect();
                }
            }
            else
            {
                logger.Debug("Received Unknown packet : " + message.GetType().Name);
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
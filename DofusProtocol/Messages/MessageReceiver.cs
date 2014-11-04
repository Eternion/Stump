using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Stump.Core.IO;
using Stump.Core.Reflection;
using System.Runtime.Serialization;
using Stump.DofusProtocol.Messages.Custom;

namespace Stump.DofusProtocol.Messages
{
    public static class MessageReceiver
    {
        private static readonly Dictionary<uint, MessageDefinition> MessagesDefinitions = new Dictionary<uint, MessageDefinition>();


        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            Assembly asm = Assembly.GetAssembly(typeof(MessageReceiver));

            foreach (Type type in asm.GetTypes().Where(x => x.IsSubclassOf(typeof(Message))))
            {
                var fieldId = type.GetField("Id");

                if (fieldId != null)
                {
                    var id = (uint)fieldId.GetValue(type);
                    var @override = type.GetCustomAttribute<OverrideMessageAttribute>() != null;
                    
                    ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);

                    if (ctor == null)
                        throw new Exception(
                            string.Format("'{0}' doesn't implemented a parameterless constructor",
                                            type));

                    var deleg = ctor.CreateDelegate<Func<Message>>();

                    MessageDefinition message;
                    if (MessagesDefinitions.TryGetValue(id, out message))
                    {
                        if (!message.Override && !@override)
                            throw new AmbiguousMatchException(
                                string.Format(
                                    "MessageReceiver() => {0} item is already in the dictionary, old type is : {1}, new type is  {2} (use OverrideMessage attribute)",
                                    id, MessagesDefinitions[id], type));
                        
                        else if (@override && message.Override)
                            throw new Exception(
                                string.Format("MessageReceiver() => {0} and {1} override both the same message (id:{2})", message.MessageType, type, id));

                        else if (@override)
                            MessagesDefinitions[id] = new MessageDefinition(id, type, deleg, true);
                    }
                    else
                        MessagesDefinitions.Add(id, new MessageDefinition(id, type, deleg, @override));

                }
            }
        }

        /// <summary>
        ///   Gets instance of the message defined by id thanks to BigEndianReader.
        /// </summary>
        /// <param name = "id">id.</param>
        /// <returns></returns>
        public static Message BuildMessage(uint id, IDataReader reader)
        {
            MessageDefinition definition;
            if (!MessagesDefinitions.TryGetValue(id, out definition))
                throw new MessageNotFoundException(string.Format("Message <id:{0}> doesn't exist", id));

            Message message = definition.MessageConstructor();

            if (message == null)
                throw new MessageNotFoundException(string.Format("Constructors[{0}] (delegate {1}) does not exist", id, MessagesDefinitions[id]));

            message.Unpack(reader);

            return message;
        }

        public static Type GetMessageType(uint id)
        {
            MessageDefinition definition;
            if (!MessagesDefinitions.TryGetValue(id, out definition))
                throw new MessageNotFoundException(string.Format("Message <id:{0}> doesn't exist", id));

            return definition.MessageType;
        }

        [Serializable]
        public class MessageNotFoundException : Exception
        {
            public MessageNotFoundException()
            {
            }

            public MessageNotFoundException(string message)
                : base(message)
            {
            }

            public MessageNotFoundException(string message, Exception inner)
                : base(message, inner)
            {
            }

            protected MessageNotFoundException(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}
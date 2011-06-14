using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Stump.BaseCore.Framework.IO;
using Stump.BaseCore.Framework.Reflection;

namespace Stump.DofusProtocol.Messages
{
    public static class MessageReceiver
    {
        private static readonly Dictionary<uint, Type> Messages = new Dictionary<uint, Type>(200);
        private static readonly Dictionary<uint, Func<Message>> Constructors = new Dictionary<uint, Func<Message>>();


        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            Assembly asm = Assembly.GetAssembly(typeof(MessageReceiver));

                foreach (Type type in asm.GetTypes())
                {
                    var fieldId = type.GetField("Id");

                    if (fieldId != null)
                    {
                        var id = (uint)fieldId.GetValue(type);
                        if (Messages.ContainsKey(id))
                            throw new AmbiguousMatchException(
                                string.Format(
                                    "MessageReceiver() => {0} item is already in the dictionary, old type is : {1}, new type is  {2}",
                                    id, Messages[id], type));

                        Messages.Add(id, type);

                        ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);

                        if (ctor == null)
                            throw new Exception(
                                string.Format("'{0}' doesn't implemented a parameterless constructor",
                                              type));

                        Constructors.Add(id, ctor.CreateDelegate<Message>());
                    }
                }
        }

        /// <summary>
        ///   Gets instance of the message defined by id thanks to BigEndianReader.
        /// </summary>
        /// <param name = "id">id.</param>
        /// <returns></returns>
        public static Message GetMessage(uint id, BigEndianReader reader)
        {
            if (!Messages.ContainsKey(id))
               throw new KeyNotFoundException(string.Format("Message <id:{0}> doesn't exist", id));

            Message message = Constructors[id]();

            if (message == null)
                throw new KeyNotFoundException(string.Format("Constructors[{0}] (delegate {1}) does not exist", id, Messages[id]));

            message.Unpack(reader);

            return message;
        }
    }
}
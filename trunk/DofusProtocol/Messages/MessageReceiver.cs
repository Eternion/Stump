using System;
using System.Collections.Generic;
using System.Reflection;
using Stump.BaseCore.Framework.Reflection;
using Stump.BaseCore.Framework.IO;

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
                    try
                    {
                        FieldInfo fi = type.GetField("protocolId");

                        if (fi != null)
                        {
                            uint id = (uint)fi.GetValue(type);

                            Messages.Add(id, type);

                            var ctor = type.GetConstructor(Type.EmptyTypes);

                            if (ctor == null)
                                throw new Exception(
                                    string.Format("'{0}' doesn't implemented a parameterless constructor",
                                                  type.ToString()));

                            Constructors.Add(id, ctor.CreateDelegate<Message>());
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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

            var message = Constructors[id]();

            message.unpack(reader, 0);

            return message;
        }
    }
}
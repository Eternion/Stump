using System;

namespace Stump.DofusProtocol.Messages
{
    public class MessageDefinition
    {
        public MessageDefinition(uint id, Type messageType, Func<Message> messageConstructor, bool @override)
        {
            Id = id;
            MessageType = messageType;
            MessageConstructor = messageConstructor;
            Override = @override;
        }

        public uint Id
        {
            get;
            set;
        }

        public Type MessageType
        {
            get;
            set;
        }

        public Func<Message> MessageConstructor
        {
            get;
            set;
        }

        public bool Override
        {
            get;
            set;
        }
    }
}
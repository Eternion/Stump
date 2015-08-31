using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages.Custom
{
    public class CustomTicketMessage : Message
    {

        public string ticket;


        public const uint Id = 10001;

        public override uint MessageId
        {
            get { return Id; }
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(ticket);
        }

        public override void Deserialize(IDataReader reader)
        {
            ticket = reader.ReadUTF();
        }
    }
}
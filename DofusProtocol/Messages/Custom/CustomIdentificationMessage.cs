using System.Collections.Generic;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages.Custom
{
    public class CustomIdentificationMessage : Message
    {
        
        public string login;
        public string password;


        public const Id = 10000;

        public override uint MessageId
        {
            get { return Id; }
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(login);
            writer.WriteUTF(password);
        }

        public override void Deserialize(IDataReader reader)
        {
            login = reader.ReadUTF();
            password = reader.ReadUTF();
        }
    }
}
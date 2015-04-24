

// Generated on 04/24/2015 03:37:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AccountCapabilitiesMessage : Message
    {
        public const uint Id = 6216;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int accountId;
        public bool tutorialAvailable;
        public ushort breedsVisible;
        public ushort breedsAvailable;
        public sbyte status;
        
        public AccountCapabilitiesMessage()
        {
        }
        
        public AccountCapabilitiesMessage(int accountId, bool tutorialAvailable, ushort breedsVisible, ushort breedsAvailable, sbyte status)
        {
            this.accountId = accountId;
            this.tutorialAvailable = tutorialAvailable;
            this.breedsVisible = breedsVisible;
            this.breedsAvailable = breedsAvailable;
            this.status = status;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(accountId);
            writer.WriteBoolean(tutorialAvailable);
            writer.WriteUShort(breedsVisible);
            writer.WriteUShort(breedsAvailable);
            writer.WriteSByte(status);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            tutorialAvailable = reader.ReadBoolean();
            breedsVisible = reader.ReadUShort();
            if (breedsVisible < 0 || breedsVisible > 65535)
                throw new Exception("Forbidden value on breedsVisible = " + breedsVisible + ", it doesn't respect the following condition : breedsVisible < 0 || breedsVisible > 65535");
            breedsAvailable = reader.ReadUShort();
            if (breedsAvailable < 0 || breedsAvailable > 65535)
                throw new Exception("Forbidden value on breedsAvailable = " + breedsAvailable + ", it doesn't respect the following condition : breedsAvailable < 0 || breedsAvailable > 65535");
            status = reader.ReadSByte();
        }
        
    }
    
}
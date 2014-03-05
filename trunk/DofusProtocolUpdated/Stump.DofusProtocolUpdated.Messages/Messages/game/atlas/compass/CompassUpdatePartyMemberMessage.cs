

// Generated on 03/05/2014 20:34:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CompassUpdatePartyMemberMessage : CompassUpdateMessage
    {
        public const uint Id = 5589;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int memberId;
        
        public CompassUpdatePartyMemberMessage()
        {
        }
        
        public CompassUpdatePartyMemberMessage(sbyte type, Types.MapCoordinates coords, int memberId)
         : base(type, coords)
        {
            this.memberId = memberId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(memberId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            memberId = reader.ReadInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}
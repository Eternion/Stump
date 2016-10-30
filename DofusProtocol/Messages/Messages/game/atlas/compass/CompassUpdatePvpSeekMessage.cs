

// Generated on 10/30/2016 16:20:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CompassUpdatePvpSeekMessage : CompassUpdateMessage
    {
        public const uint Id = 6013;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long memberId;
        public string memberName;
        
        public CompassUpdatePvpSeekMessage()
        {
        }
        
        public CompassUpdatePvpSeekMessage(sbyte type, Types.MapCoordinates coords, long memberId, string memberName)
         : base(type, coords)
        {
            this.memberId = memberId;
            this.memberName = memberName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(memberId);
            writer.WriteUTF(memberName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            memberId = reader.ReadVarLong();
            if (memberId < 0 || memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
            memberName = reader.ReadUTF();
        }
        
    }
    
}
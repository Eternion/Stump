

// Generated on 04/19/2016 10:17:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyFollowStatusUpdateMessage : AbstractPartyMessage
    {
        public const uint Id = 5581;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool success;
        public bool isFollowed;
        public long followedId;
        
        public PartyFollowStatusUpdateMessage()
        {
        }
        
        public PartyFollowStatusUpdateMessage(int partyId, bool success, bool isFollowed, long followedId)
         : base(partyId)
        {
            this.success = success;
            this.isFollowed = isFollowed;
            this.followedId = followedId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, success);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, isFollowed);
            writer.WriteByte(flag1);
            writer.WriteVarLong(followedId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            success = BooleanByteWrapper.GetFlag(flag1, 0);
            isFollowed = BooleanByteWrapper.GetFlag(flag1, 1);
            followedId = reader.ReadVarLong();
            if (followedId < 0 || followedId > 9007199254740990)
                throw new Exception("Forbidden value on followedId = " + followedId + ", it doesn't respect the following condition : followedId < 0 || followedId > 9007199254740990");
        }
        
    }
    
}


// Generated on 10/27/2014 19:57:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class UpdateMountBoostMessage : Message
    {
        public const uint Id = 6179;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double rideId;
        public IEnumerable<Types.UpdateMountBoost> boostToUpdateList;
        
        public UpdateMountBoostMessage()
        {
        }
        
        public UpdateMountBoostMessage(double rideId, IEnumerable<Types.UpdateMountBoost> boostToUpdateList)
        {
            this.rideId = rideId;
            this.boostToUpdateList = boostToUpdateList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(rideId);
            var boostToUpdateList_before = writer.Position;
            var boostToUpdateList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in boostToUpdateList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 boostToUpdateList_count++;
            }
            var boostToUpdateList_after = writer.Position;
            writer.Seek((int)boostToUpdateList_before);
            writer.WriteUShort((ushort)boostToUpdateList_count);
            writer.Seek((int)boostToUpdateList_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            rideId = reader.ReadDouble();
            if (rideId < -9.007199254740992E15 || rideId > 9.007199254740992E15)
                throw new Exception("Forbidden value on rideId = " + rideId + ", it doesn't respect the following condition : rideId < -9.007199254740992E15 || rideId > 9.007199254740992E15");
            var limit = reader.ReadUShort();
            var boostToUpdateList_ = new Types.UpdateMountBoost[limit];
            for (int i = 0; i < limit; i++)
            {
                 boostToUpdateList_[i] = Types.ProtocolTypeManager.GetInstance<Types.UpdateMountBoost>(reader.ReadShort());
                 boostToUpdateList_[i].Deserialize(reader);
            }
            boostToUpdateList = boostToUpdateList_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double) + sizeof(short) + boostToUpdateList.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}
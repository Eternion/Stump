
// Generated on 03/25/2013 19:24:21
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)boostToUpdateList.Count());
            foreach (var entry in boostToUpdateList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            rideId = reader.ReadDouble();
            var limit = reader.ReadUShort();
            boostToUpdateList = new Types.UpdateMountBoost[limit];
            for (int i = 0; i < limit; i++)
            {
                 (boostToUpdateList as Types.UpdateMountBoost[])[i] = Types.ProtocolTypeManager.GetInstance<Types.UpdateMountBoost>(reader.ReadShort());
                 (boostToUpdateList as Types.UpdateMountBoost[])[i].Deserialize(reader);
            }
        }
        
    }
    
}
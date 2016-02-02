

// Generated on 02/02/2016 14:14:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeGuildTaxCollectorGetMessage : Message
    {
        public const uint Id = 5762;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string collectorName;
        public short worldX;
        public short worldY;
        public int mapId;
        public short subAreaId;
        public string userName;
        public long callerId;
        public string callerName;
        public double experience;
        public short pods;
        public IEnumerable<Types.ObjectItemGenericQuantity> objectsInfos;
        
        public ExchangeGuildTaxCollectorGetMessage()
        {
        }
        
        public ExchangeGuildTaxCollectorGetMessage(string collectorName, short worldX, short worldY, int mapId, short subAreaId, string userName, long callerId, string callerName, double experience, short pods, IEnumerable<Types.ObjectItemGenericQuantity> objectsInfos)
        {
            this.collectorName = collectorName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.userName = userName;
            this.callerId = callerId;
            this.callerName = callerName;
            this.experience = experience;
            this.pods = pods;
            this.objectsInfos = objectsInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(collectorName);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteVarShort(subAreaId);
            writer.WriteUTF(userName);
            writer.WriteVarLong(callerId);
            writer.WriteUTF(callerName);
            writer.WriteDouble(experience);
            writer.WriteVarShort(pods);
            var objectsInfos_before = writer.Position;
            var objectsInfos_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
                 objectsInfos_count++;
            }
            var objectsInfos_after = writer.Position;
            writer.Seek((int)objectsInfos_before);
            writer.WriteUShort((ushort)objectsInfos_count);
            writer.Seek((int)objectsInfos_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            collectorName = reader.ReadUTF();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            mapId = reader.ReadInt();
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            userName = reader.ReadUTF();
            callerId = reader.ReadVarLong();
            if (callerId < 0 || callerId > 9.007199254740992E15)
                throw new Exception("Forbidden value on callerId = " + callerId + ", it doesn't respect the following condition : callerId < 0 || callerId > 9.007199254740992E15");
            callerName = reader.ReadUTF();
            experience = reader.ReadDouble();
            if (experience < -9.007199254740992E15 || experience > 9.007199254740992E15)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < -9.007199254740992E15 || experience > 9.007199254740992E15");
            pods = reader.ReadVarShort();
            if (pods < 0)
                throw new Exception("Forbidden value on pods = " + pods + ", it doesn't respect the following condition : pods < 0");
            var limit = reader.ReadUShort();
            var objectsInfos_ = new Types.ObjectItemGenericQuantity[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos_[i] = new Types.ObjectItemGenericQuantity();
                 objectsInfos_[i].Deserialize(reader);
            }
            objectsInfos = objectsInfos_;
        }
        
    }
    
}
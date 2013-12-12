

// Generated on 12/12/2013 16:57:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ZaapListMessage : TeleportDestinationsListMessage
    {
        public const uint Id = 1604;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int spawnMapId;
        
        public ZaapListMessage()
        {
        }
        
        public ZaapListMessage(sbyte teleporterType, IEnumerable<int> mapIds, IEnumerable<short> subAreaIds, IEnumerable<short> costs, IEnumerable<sbyte> destTeleporterType, int spawnMapId)
         : base(teleporterType, mapIds, subAreaIds, costs, destTeleporterType)
        {
            this.spawnMapId = spawnMapId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(spawnMapId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spawnMapId = reader.ReadInt();
            if (spawnMapId < 0)
                throw new Exception("Forbidden value on spawnMapId = " + spawnMapId + ", it doesn't respect the following condition : spawnMapId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}
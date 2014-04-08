

// Generated on 03/06/2014 18:50:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LockableStateUpdateStorageMessage : LockableStateUpdateAbstractMessage
    {
        public const uint Id = 5669;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mapId;
        public int elementId;
        
        public LockableStateUpdateStorageMessage()
        {
        }
        
        public LockableStateUpdateStorageMessage(bool locked, int mapId, int elementId)
         : base(locked)
        {
            this.mapId = mapId;
            this.elementId = elementId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(mapId);
            writer.WriteInt(elementId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            mapId = reader.ReadInt();
            elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(int);
        }
        
    }
    
}


// Generated on 07/29/2013 23:08:36
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StartupActionsListMessage : Message
    {
        public const uint Id = 1301;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.StartupActionAddObject> actions;
        
        public StartupActionsListMessage()
        {
        }
        
        public StartupActionsListMessage(IEnumerable<Types.StartupActionAddObject> actions)
        {
            this.actions = actions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)actions.Count());
            foreach (var entry in actions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            actions = new Types.StartupActionAddObject[limit];
            for (int i = 0; i < limit; i++)
            {
                 (actions as Types.StartupActionAddObject[])[i] = new Types.StartupActionAddObject();
                 (actions as Types.StartupActionAddObject[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + actions.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}
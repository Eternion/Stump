

// Generated on 12/12/2013 16:57:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class QuestObjectiveInformations
    {
        public const short Id = 385;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short objectiveId;
        public bool objectiveStatus;
        public IEnumerable<string> dialogParams;
        
        public QuestObjectiveInformations()
        {
        }
        
        public QuestObjectiveInformations(short objectiveId, bool objectiveStatus, IEnumerable<string> dialogParams)
        {
            this.objectiveId = objectiveId;
            this.objectiveStatus = objectiveStatus;
            this.dialogParams = dialogParams;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(objectiveId);
            writer.WriteBoolean(objectiveStatus);
            writer.WriteUShort((ushort)dialogParams.Count());
            foreach (var entry in dialogParams)
            {
                 writer.WriteUTF(entry);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            objectiveId = reader.ReadShort();
            if (objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
            objectiveStatus = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            dialogParams = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (dialogParams as string[])[i] = reader.ReadUTF();
            }
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(bool) + sizeof(short) + dialogParams.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x));
        }
        
    }
    
}
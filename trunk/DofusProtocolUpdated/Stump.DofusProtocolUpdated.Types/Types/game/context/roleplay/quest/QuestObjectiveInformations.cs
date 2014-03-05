

// Generated on 03/05/2014 20:34:49
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
            var dialogParams_before = writer.Position;
            var dialogParams_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in dialogParams)
            {
                 writer.WriteUTF(entry);
                 dialogParams_count++;
            }
            var dialogParams_after = writer.Position;
            writer.Seek((int)dialogParams_before);
            writer.WriteUShort((ushort)dialogParams_count);
            writer.Seek((int)dialogParams_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            objectiveId = reader.ReadShort();
            if (objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
            objectiveStatus = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            var dialogParams_ = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 dialogParams_[i] = reader.ReadUTF();
            }
            dialogParams = dialogParams_;
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(bool) + sizeof(short) + dialogParams.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x));
        }
        
    }
    
}
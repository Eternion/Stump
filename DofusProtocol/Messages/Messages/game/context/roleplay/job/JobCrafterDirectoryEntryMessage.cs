

// Generated on 10/30/2016 16:20:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobCrafterDirectoryEntryMessage : Message
    {
        public const uint Id = 6044;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.JobCrafterDirectoryEntryPlayerInfo playerInfo;
        public IEnumerable<Types.JobCrafterDirectoryEntryJobInfo> jobInfoList;
        public Types.EntityLook playerLook;
        
        public JobCrafterDirectoryEntryMessage()
        {
        }
        
        public JobCrafterDirectoryEntryMessage(Types.JobCrafterDirectoryEntryPlayerInfo playerInfo, IEnumerable<Types.JobCrafterDirectoryEntryJobInfo> jobInfoList, Types.EntityLook playerLook)
        {
            this.playerInfo = playerInfo;
            this.jobInfoList = jobInfoList;
            this.playerLook = playerLook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            playerInfo.Serialize(writer);
            var jobInfoList_before = writer.Position;
            var jobInfoList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in jobInfoList)
            {
                 entry.Serialize(writer);
                 jobInfoList_count++;
            }
            var jobInfoList_after = writer.Position;
            writer.Seek((int)jobInfoList_before);
            writer.WriteUShort((ushort)jobInfoList_count);
            writer.Seek((int)jobInfoList_after);

            playerLook.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            playerInfo = new Types.JobCrafterDirectoryEntryPlayerInfo();
            playerInfo.Deserialize(reader);
            var limit = reader.ReadUShort();
            var jobInfoList_ = new Types.JobCrafterDirectoryEntryJobInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 jobInfoList_[i] = new Types.JobCrafterDirectoryEntryJobInfo();
                 jobInfoList_[i].Deserialize(reader);
            }
            jobInfoList = jobInfoList_;
            playerLook = new Types.EntityLook();
            playerLook.Deserialize(reader);
        }
        
    }
    
}


// Generated on 12/12/2013 16:57:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobCrafterDirectorySettingsMessage : Message
    {
        public const uint Id = 5652;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.JobCrafterDirectorySettings> craftersSettings;
        
        public JobCrafterDirectorySettingsMessage()
        {
        }
        
        public JobCrafterDirectorySettingsMessage(IEnumerable<Types.JobCrafterDirectorySettings> craftersSettings)
        {
            this.craftersSettings = craftersSettings;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)craftersSettings.Count());
            foreach (var entry in craftersSettings)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            craftersSettings = new Types.JobCrafterDirectorySettings[limit];
            for (int i = 0; i < limit; i++)
            {
                 (craftersSettings as Types.JobCrafterDirectorySettings[])[i] = new Types.JobCrafterDirectorySettings();
                 (craftersSettings as Types.JobCrafterDirectorySettings[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + craftersSettings.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}
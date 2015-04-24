

// Generated on 04/24/2015 03:38:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HumanOptionFollowers : HumanOption
    {
        public const short Id = 410;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.IndexedEntityLook> followingCharactersLook;
        
        public HumanOptionFollowers()
        {
        }
        
        public HumanOptionFollowers(IEnumerable<Types.IndexedEntityLook> followingCharactersLook)
        {
            this.followingCharactersLook = followingCharactersLook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var followingCharactersLook_before = writer.Position;
            var followingCharactersLook_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in followingCharactersLook)
            {
                 entry.Serialize(writer);
                 followingCharactersLook_count++;
            }
            var followingCharactersLook_after = writer.Position;
            writer.Seek((int)followingCharactersLook_before);
            writer.WriteUShort((ushort)followingCharactersLook_count);
            writer.Seek((int)followingCharactersLook_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var followingCharactersLook_ = new Types.IndexedEntityLook[limit];
            for (int i = 0; i < limit; i++)
            {
                 followingCharactersLook_[i] = new Types.IndexedEntityLook();
                 followingCharactersLook_[i].Deserialize(reader);
            }
            followingCharactersLook = followingCharactersLook_;
        }
        
        
    }
    
}
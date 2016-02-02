

// Generated on 02/02/2016 14:14:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PrismFightersInformation
    {
        public const short Id = 443;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
        public IEnumerable<Types.CharacterMinimalPlusLookInformations> allyCharactersInformations;
        public IEnumerable<Types.CharacterMinimalPlusLookInformations> enemyCharactersInformations;
        
        public PrismFightersInformation()
        {
        }
        
        public PrismFightersInformation(short subAreaId, Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo, IEnumerable<Types.CharacterMinimalPlusLookInformations> allyCharactersInformations, IEnumerable<Types.CharacterMinimalPlusLookInformations> enemyCharactersInformations)
        {
            this.subAreaId = subAreaId;
            this.waitingForHelpInfo = waitingForHelpInfo;
            this.allyCharactersInformations = allyCharactersInformations;
            this.enemyCharactersInformations = enemyCharactersInformations;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(subAreaId);
            waitingForHelpInfo.Serialize(writer);
            var allyCharactersInformations_before = writer.Position;
            var allyCharactersInformations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in allyCharactersInformations)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 allyCharactersInformations_count++;
            }
            var allyCharactersInformations_after = writer.Position;
            writer.Seek((int)allyCharactersInformations_before);
            writer.WriteUShort((ushort)allyCharactersInformations_count);
            writer.Seek((int)allyCharactersInformations_after);

            var enemyCharactersInformations_before = writer.Position;
            var enemyCharactersInformations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in enemyCharactersInformations)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 enemyCharactersInformations_count++;
            }
            var enemyCharactersInformations_after = writer.Position;
            writer.Seek((int)enemyCharactersInformations_before);
            writer.WriteUShort((ushort)enemyCharactersInformations_count);
            writer.Seek((int)enemyCharactersInformations_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            waitingForHelpInfo = new Types.ProtectedEntityWaitingForHelpInfo();
            waitingForHelpInfo.Deserialize(reader);
            var limit = reader.ReadUShort();
            var allyCharactersInformations_ = new Types.CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 allyCharactersInformations_[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterMinimalPlusLookInformations>(reader.ReadShort());
                 allyCharactersInformations_[i].Deserialize(reader);
            }
            allyCharactersInformations = allyCharactersInformations_;
            limit = reader.ReadUShort();
            var enemyCharactersInformations_ = new Types.CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 enemyCharactersInformations_[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterMinimalPlusLookInformations>(reader.ReadShort());
                 enemyCharactersInformations_[i].Deserialize(reader);
            }
            enemyCharactersInformations = enemyCharactersInformations_;
        }
        
        
    }
    
}
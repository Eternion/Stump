

// Generated on 01/04/2015 11:54:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapRunningFightDetailsExtendedMessage : MapRunningFightDetailsMessage
    {
        public const uint Id = 6500;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.NamedPartyTeam> namedPartyTeams;
        
        public MapRunningFightDetailsExtendedMessage()
        {
        }
        
        public MapRunningFightDetailsExtendedMessage(int fightId, IEnumerable<Types.GameFightFighterLightInformations> attackers, IEnumerable<Types.GameFightFighterLightInformations> defenders, IEnumerable<Types.NamedPartyTeam> namedPartyTeams)
         : base(fightId, attackers, defenders)
        {
            this.namedPartyTeams = namedPartyTeams;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var namedPartyTeams_before = writer.Position;
            var namedPartyTeams_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in namedPartyTeams)
            {
                 entry.Serialize(writer);
                 namedPartyTeams_count++;
            }
            var namedPartyTeams_after = writer.Position;
            writer.Seek((int)namedPartyTeams_before);
            writer.WriteUShort((ushort)namedPartyTeams_count);
            writer.Seek((int)namedPartyTeams_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var namedPartyTeams_ = new Types.NamedPartyTeam[limit];
            for (int i = 0; i < limit; i++)
            {
                 namedPartyTeams_[i] = new Types.NamedPartyTeam();
                 namedPartyTeams_[i].Deserialize(reader);
            }
            namedPartyTeams = namedPartyTeams_;
        }
        
    }
    
}
using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.XmlSerialize
{
    public class NpcSerialized : Localizable
    {
        private NpcSerialized()
        {
            
        }

        public NpcSerialized(uint mapId, GameRolePlayNpcInformations npcInformations)
            : base((ushort) npcInformations.disposition.cellId, mapId)
        {
            NpcInformations = npcInformations;
        }

        public GameRolePlayNpcInformations NpcInformations
        {
            get;
            set;
        }

        public EntityDispositionInformations Disposition
        {
            get { return NpcInformations.disposition; }
        }
    }
}

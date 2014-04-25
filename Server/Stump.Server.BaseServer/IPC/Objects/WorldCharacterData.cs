using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Objects
{
    // easier so, it's a simle tuple
    [ProtoContract]
    public class WorldCharacterData
    {
        public WorldCharacterData()
        {
            
        }

        public WorldCharacterData(int characterId, int worldId)
        {
            CharacterId = characterId;
            WorldId = worldId;
        }

        [ProtoMember(1)]
        public int CharacterId
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public int WorldId
        {
            get;
            set;
        }
    }
}
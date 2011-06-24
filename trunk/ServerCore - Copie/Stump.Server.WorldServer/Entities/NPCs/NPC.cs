
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.World.Entities.Actors;

namespace Stump.Server.WorldServer.Entities
{
    public class NPC : Actor, ITrader, IExchanger
    {

        protected NPC(long id,string name, ExtendedLook look, VectorIsometric position, uint npcId, bool sex,uint specialArtworkId, bool canGiveQuest)
            :base(id,name,look,position)
        {
            NpcId = npcId;
            Sex = sex;
            SpecialArtworkId = specialArtworkId;
            CanGiveQuest = canGiveQuest;
        }

        public uint NpcId
        {
            get;
            set;
        }

        public bool Sex
        {
            get;
            set;
        }

        public uint SpecialArtworkId
        {
            get;
            set;
        }

        public bool CanGiveQuest
        {
            get;
            set;
        }

        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
            return new GameRolePlayNpcInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), NpcId, Sex, SpecialArtworkId, CanGiveQuest);
        }

    }
}
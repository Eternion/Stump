
using System;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.World.Actors;
using Stump.Server.WorldServer.World.Entities.Actors;

namespace Stump.Server.WorldServer.Entities
{
    public class Prism : Actor, IAttackable,IAligned
    {

        protected Prism(long id,string name, ExtendedLook look, VectorIsometric position, ActorAlignment alignment)
            : base(id,name, look, position)
        {
            Alignment = alignment;
        }

        public ActorAlignment Alignment
        {
            get;
            set;
        }

        public DateTime PlaceDate
        {
            get;
            set;
        }

        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
            return new GameRolePlayPrismInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Alignment.ToActorAlignmentInformations());
        }

    }
}
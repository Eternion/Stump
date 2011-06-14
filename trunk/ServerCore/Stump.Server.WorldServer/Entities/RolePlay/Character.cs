using System;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Entities.Actors;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities.RolePlay
{
    public partial class Character : Humanoid
    {
        public Character(CharacterRecord record, WorldClient client)
            : base(record.Id, record.BaseLook, new ObjectPosition(record.MapId, record.CellId, record.Direction), record.Name)
        {
        }

        public override GameContextActorInformations GetActorInformations()
        {
            return new GameRolePlayCharacterInformations((int)Id, Look, GetDispositionInformations(), Name, GetHumanInformations(), Alignment.ToActorAlignmentInformations());
        }

        public override HumanInformations GetHumanInformations()
        {
            // todo : guilds
             
            return new HumanInformations(
                GetFollowingCharacters(), // followers
                (int)Emote.Id,
                Emote.Duration,
                GetActorRestrictions(),
                ActorTitle.Id,
                ActorTitle.Params);
        }
    }
}
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs
{
    public sealed class Npc : RolePlayActor
    {
        public Npc(NpcTemplate template, ObjectPosition position)
        {
            Template = template;
            Position = position;
        }

        public NpcTemplate Template
        {
            get;
            private set;
        }

        public override int Id
        {
            get;
            protected set;
        }

        public int TemplateId
        {
            get { return Template.Id; }
        }

        public override EntityLook Look
        {
            get { return Template.Look; }
        }

        public override ObjectPosition Position
        {
            get;
            protected set;
        }

        public void InteractWith(NpcActionTypeEnum action, Character dialoguer)
        {
            if (!CanInteractWith(action, dialoguer))
                return;


        }

        public bool CanInteractWith(NpcActionTypeEnum action, Character dialoguer)
        {
            return Template.Actions.Contains((uint)action) && dialoguer.Map == Position.Map;
        }

        public void SpeakWith(Character dialoguer)
        {

        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayNpcInformations(Id,
                                                   Look,
                                                   GetEntityDispositionInformations(),
                                                   (short)Template.Id,
                                                   Template.Gender != 0,
                                                   Template.SpecialArtworkId,
                                                   false);
        }
    }
}
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs
{
    public sealed class Npc : RolePlayActor
    {
        public Npc(int id, NpcTemplate template, ObjectPosition position)
        {
            Id = id;
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

        public void InteractWith(NpcActionTypeEnum actionType, Character dialoguer)
        {
            if (!CanInteractWith(actionType, dialoguer))
                return;

            var action = Template.GetNpcAction(actionType);

            action.Execute(this, dialoguer);
        }

        public bool CanInteractWith(NpcActionTypeEnum action, Character dialoguer)
        {
            return Template.ActionsIds.Contains((uint)action) && dialoguer.Map == Position.Map;
        }

        public void SpeakWith(Character dialoguer)
        {
            if (!CanInteractWith(NpcActionTypeEnum.ACTION_TALK, dialoguer))
                return;


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
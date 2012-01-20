using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs
{
    public sealed class Npc : RolePlayActor
    {
        public Npc(int id, NpcTemplate template, ObjectPosition position, EntityLook look)
        {
            Id = id;
            Template = template;
            Position = position;
            Look = look;

            m_gameContextActorInformations = new ObjectValidator<GameContextActorInformations>(BuildGameContextActorInformations);
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
            get;
            protected set;
        }

        public void Refresh()
        {
            m_gameContextActorInformations.Invalidate();

            if (Map != null)
                Map.Refresh(this);
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
            return dialoguer.Map == Position.Map && Template.GetNpcAction(action) != null;
        }

        public void SpeakWith(Character dialoguer)
        {
            if (!CanInteractWith(NpcActionTypeEnum.ACTION_TALK, dialoguer))
                return;

            InteractWith(NpcActionTypeEnum.ACTION_TALK, dialoguer);
        }

        #region GameContextActorInformations

        private readonly ObjectValidator<GameContextActorInformations> m_gameContextActorInformations;

        private GameContextActorInformations BuildGameContextActorInformations()
        {
            return new GameRolePlayNpcInformations(Id,
                                                   Look,
                                                   GetEntityDispositionInformations(),
                                                   (short)Template.Id,
                                                   Template.Gender != 0,
                                                   Template.SpecialArtworkId);
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return m_gameContextActorInformations;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1})", Template.Name, Template.Id);
        }
    }
}
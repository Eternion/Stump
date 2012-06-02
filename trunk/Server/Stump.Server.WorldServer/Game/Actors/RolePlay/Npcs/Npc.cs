using System;
using System.Linq;
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

            m_gameContextActorInformations =
                new ObjectValidator<GameContextActorInformations>(BuildGameContextActorInformations);
        }

        public Npc(int id, NpcSpawn spawn)
            : this(id, spawn.Template, spawn.GetPosition(), spawn.Look)
        {
            Spawn = spawn;
        }

        public NpcTemplate Template
        {
            get;
            private set;
        }

        public NpcSpawn Spawn
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
            set;
        }

        public event Action<Npc, NpcActionTypeEnum, NpcAction, Character> Interacted;

        private void OnInteracted(NpcActionTypeEnum actionType, NpcAction action, Character character)
        {
            Action<Npc, NpcActionTypeEnum, NpcAction, Character> handler = Interacted;
            if (handler != null) handler(this, actionType, action, character);
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

            NpcAction[] actions = Template.GetNpcActions(actionType);

            NpcAction action =
                actions.First(entry => entry.ConditionaExpression == null || entry.ConditionaExpression.Eval(dialoguer));

            action.Execute(this, dialoguer);
            OnInteracted(actionType, action, dialoguer);
        }

        public bool CanInteractWith(NpcActionTypeEnum action, Character dialoguer)
        {
            if (dialoguer.Map != Position.Map)
                return false;

            NpcAction[] actions = Template.GetNpcActions(action);

            return actions.Length > 0 && actions.Any(entry => entry.ConditionaExpression == null ||
                                                              entry.ConditionaExpression.Eval(dialoguer));
        }

        public void SpeakWith(Character dialoguer)
        {
            if (!CanInteractWith(NpcActionTypeEnum.ACTION_TALK, dialoguer))
                return;

            InteractWith(NpcActionTypeEnum.ACTION_TALK, dialoguer);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}) [{2}]", Template.Name, Id, TemplateId);
        }

        #region GameContextActorInformations

        private readonly ObjectValidator<GameContextActorInformations> m_gameContextActorInformations;

        private GameContextActorInformations BuildGameContextActorInformations()
        {
            return new GameRolePlayNpcInformations(Id,
                                                   Look,
                                                   GetEntityDispositionInformations(),
                                                   (short) Template.Id,
                                                   Template.Gender != 0,
                                                   Template.SpecialArtworkId);
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return m_gameContextActorInformations;
        }

        #endregion
    }
}
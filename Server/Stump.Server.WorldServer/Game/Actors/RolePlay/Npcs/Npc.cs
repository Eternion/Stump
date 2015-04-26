using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs
{
    public sealed class Npc : RolePlayActor, IInteractNpc, IContextDependant
    {
        private readonly List<NpcAction> m_actions = new List<NpcAction>();

        public Npc(int id, NpcTemplate template, ObjectPosition position, ActorLook look)
        {
            Id = id;
            Template = template;
            Position = position;
            Look = look;

            m_gameContextActorInformations =
                new ObjectValidator<GameContextActorInformations>(BuildGameContextActorInformations);
            m_actions.AddRange(Template.Actions);
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

        public override ActorLook Look
        {
            get;
            set;
        }

        public List<NpcAction> Actions
        {
            get { return m_actions; }
        }

        public event Action<Npc, NpcActionTypeEnum, NpcAction, Character> Interacted;

        private void OnInteracted(NpcActionTypeEnum actionType, NpcAction action, Character character)
        {
            var handler = Interacted;
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

            var action = Actions.First(entry => entry.ActionType.Contains(actionType) && entry.CanExecute(this, dialoguer));

            action.Execute(this, dialoguer);
            OnInteracted(actionType, action, dialoguer);
        }

        public bool CanInteractWith(NpcActionTypeEnum action, Character dialoguer)
        {
            if (dialoguer.Map != Position.Map)
                return false;

            return Actions.Count > 0 && Actions.Any(entry => entry.ActionType.Contains(action) && entry.CanExecute(this, dialoguer));
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
                                                   Look.GetEntityLook(),
                                                   GetEntityDispositionInformations(),
                                                   (short) Template.Id,
                                                   Template.Gender != 0,
                                                   Template.SpecialArtworkId);
        }

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
        {
            return m_gameContextActorInformations;
        }

        #endregion
    }
}
// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Data.Emote;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Spells;
using Stump.Server.WorldServer.World.Actors.Extensions;

namespace Stump.Server.WorldServer.World.Actors.Actor
{
    public abstract partial class Actor
    {

        #region Move

        public delegate void MoveHandler(List<uint> movementsKey);
        public event MoveHandler ActorMoved;

        public virtual void Move(MovementPath path)
        {
            List<uint> movementsKey = path.GetServerMovementKeys();

            Map.Characters.Send(c =>
            {
                ContextHandler.SendGameMapMovementMessage(c.Client, movementsKey, this);
                BasicHandler.SendBasicNoOperationMessage(c.Client);
            }, c => c.Context == GameContextEnum.ROLE_PLAY);

            if (ActorMoved != null) ActorMoved(movementsKey);
        }

        #endregion

        #region Follow

        private void OnMove(List<uint> movementsKey)
        {
            movementsKey.RemoveAt(movementsKey.Count - 1);

            Map.Characters.Send(c =>
            {
                ContextHandler.SendGameContextRemoveElementMessage(c.Client, this);
                BasicHandler.SendBasicNoOperationMessage(c.Client);
            }, c => c.Context == GameContextEnum.ROLE_PLAY);

            if (ActorMoved != null) ActorMoved(movementsKey);
        }

        public virtual void Follow(Actor actor)
        {
            actor.ActorMoved += OnMove;
        }

        public virtual void StopFollowing(Actor actor)
        {
            actor.ActorMoved -= OnMove;
        }

        #endregion

        #region Teleport

        public virtual void Teleport(VectorIsometric position)
        {
            Map.Characters.Send(c =>
            {
                ContextHandler.SendGameContextRemoveElementMessage(c.Client, this);
                BasicHandler.SendBasicNoOperationMessage(c.Client);
            }, c => c.Context == GameContextEnum.ROLE_PLAY);

            Map.Leave(this);

            Position = position;

            Map.Enter(this);

            Map.Characters.Send(c =>
            {
                ContextHandler.SendGameRolePlayShowActorMessage(c.Client, this);
                BasicHandler.SendBasicNoOperationMessage(c.Client);
            }, c => c.Context == GameContextEnum.ROLE_PLAY);
        }

        #endregion

        #region Speak

        public virtual void Say()
        {
           //TODO
        }

        #endregion

        #region ChangeLook

        public virtual void ChangeLook(ExtendedLook look)
        {
            Look = look;

            Map.Characters.Send(c =>
            {
                ContextHandler.SendGameContextRefreshEntityLookMessage(c.Client, this);
                BasicHandler.SendBasicNoOperationMessage(c.Client);
            }, c => c.Context == GameContextEnum.ROLE_PLAY);
        }

        #endregion

        #region Play Emote

        public virtual void PlayEmote(EmotesEnum emote)
        {
            Map.Characters.Send(c =>
            {
                ContextHandler.SendEmotePlayMessage(c.Client, this, emote, EmoteDurationProvider.Instance.Get(emote));
                BasicHandler.SendBasicNoOperationMessage(c.Client);
            }, c => c.Context == GameContextEnum.ROLE_PLAY);
        }

        #endregion

        #region Show Smiley

        public virtual void ShowSmiley(uint smileyId)
        {
            Map.Characters.Send(c =>
            {
                ChatHandler.SendChatSmileyMessage(c.Client, this, smileyId);
                BasicHandler.SendBasicNoOperationMessage(c.Client);
            }, c => c.Context == GameContextEnum.ROLE_PLAY);
        }

        #endregion

        #region Change Direction

        public virtual void ChangeDirection(DirectionsEnum direction)
        {
            Position.Direction = direction;

            Map.Characters.Send(c =>
            {
                ContextHandler.SendGameMapChangeOrientationMessage(c.Client, this);
                BasicHandler.SendBasicNoOperationMessage(c.Client);
            }, c => c.Context == GameContextEnum.ROLE_PLAY);
        }

        #endregion

        #region Update Name

        public virtual void UpdateName(string newName)
        {
            Name = newName;

            Map.Characters.Send(c =>
               {
                   ContextHandler.SendGameContextRemoveElementMessage(c.Client, this);
                   ContextHandler.SendGameRolePlayShowActorMessage(c.Client, this);
                   BasicHandler.SendBasicNoOperationMessage(c.Client);
               }, c => c.Context == GameContextEnum.ROLE_PLAY);
        }

        #endregion

    }
}





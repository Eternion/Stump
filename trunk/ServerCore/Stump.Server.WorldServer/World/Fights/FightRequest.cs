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
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.World.Actors.Character;

namespace Stump.Server.WorldServer.Fights
{
    public class FightRequest : IDialogRequest
    {
        public FightRequest(Character source, Character target)
        {
            Source = source;
            Target = target;
        }

        #region IDialogRequest Members

        public Character Source
        {
            get;
            set;
        }

        public Character Target
        {
            get;
            set;
        }

        public void StartDialog()
        {
            ContextHandler.SendGameRolePlayPlayerFightFriendlyRequestedMessage(Source.Client, Target, Source, Target);
            ContextHandler.SendGameRolePlayPlayerFightFriendlyRequestedMessage(Target.Client, Source, Source, Target);
        }

        public void AcceptDialog()
        {
            try
            {
                ContextHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Source, Source, Target, true);

                Source.StartFightWith(Target, true);
            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }

        public void DeniedDialog() // Logically never used
        {
            try
            {
                ContextHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, Source, Source, Target, false);
            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }

        #endregion

        public void DeniedDialog(Character replier)
        {
            try
            {
                if (replier.Id == Source.Id)
                    ContextHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Target.Client, replier, Source, Target, false);
                else if (replier.Id == Target.Id)
                    ContextHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, replier, Source, Target, false);

            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }
    }
}
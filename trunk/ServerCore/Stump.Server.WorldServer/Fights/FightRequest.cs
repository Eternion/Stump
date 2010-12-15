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
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Fights
{
    public class FightRequest : IDialogRequest
    {
        public FightRequest(Character source, Character target, FightGroup groupSource, FightGroup groupTarget)
        {
            Source = source;
            Target = target;
            GroupSource = groupSource;
            GroupTarget = groupTarget;
        }

        public FightGroup GroupSource
        {
            get;
            set;
        }

        public FightGroup GroupTarget
        {
            get;
            set;
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

        public void AcceptDialog()
        {
            try
            {
                ContextHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, GroupTarget.Fight, true);
                GroupTarget.Fight.StartingFight();
            }
            catch
            {
                /* ... */
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
                ContextHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Source.Client, GroupTarget.Fight, false);
                GroupTarget.Fight.CancelFight(GroupSource.Id);
            }
            catch
            {
                /* ... */
            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }

        #endregion

        public void DeniedDialog(int groupId)
        {
            try
            {
                Fight fight = FightManager.GetFightByGroupId(groupId);

                Character chr = fight.SourceGroup.Id == groupId
                                    ? fight.TargetGroup.Members.First().Entity as Character
                                    : fight.SourceGroup.Members.First().Entity as Character;

                ContextHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(chr.Client, fight, false);
                fight.CancelFight(groupId);
            }
            catch
            {
                /* ... */
            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }
    }
}
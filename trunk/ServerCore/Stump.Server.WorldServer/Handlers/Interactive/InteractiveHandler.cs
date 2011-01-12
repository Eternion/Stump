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
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Skills;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class InteractiveHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (InteractiveUseRequestMessage))]
        public static void HandleInteractiveUseRequestMessage(WorldClient client, InteractiveUseRequestMessage message)
        {
            InteractiveObject interactiveObject = client.ActiveCharacter.Map.GetInteractiveObject(message.elemId);

            if (interactiveObject == null)
                return;

            SkillBase skill = interactiveObject.GetSkill(message.skillInstanceUid);

            if (skill == null)
                return;

            client.ActiveCharacter.Map.CallOnAllCharacters(character =>
                SendInteractiveUsedMessage(character.Client, client.ActiveCharacter, interactiveObject, skill));

            interactiveObject.ExecuteSkill(client.ActiveCharacter,
                                           message.skillInstanceUid);
        }

        public static void SendInteractiveUsedMessage(WorldClient client, Entity entity, InteractiveObject interactiveObject, SkillBase skill)
        {
            client.Send(new InteractiveUsedMessage((uint) entity.Id, interactiveObject.ElementId, skill.SkillId, skill.Duration));
        }
    }
}
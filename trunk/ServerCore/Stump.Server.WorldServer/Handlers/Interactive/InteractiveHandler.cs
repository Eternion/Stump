
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

            client.ActiveCharacter.Map.Do(character =>
                                                           SendInteractiveUsedMessage(character.Client,
                                                                                      client.ActiveCharacter,
                                                                                      interactiveObject, skill));

            interactiveObject.ExecuteSkill(client.ActiveCharacter,
                                           message.skillInstanceUid);
        }

        public static void SendInteractiveUsedMessage(WorldClient client, Entity entity,
                                                      InteractiveObject interactiveObject, SkillBase skill)
        {
            client.Send(new InteractiveUsedMessage((uint) entity.Id, interactiveObject.ElementId, skill.SkillId,
                                                   skill.Duration));
        }
    }
}
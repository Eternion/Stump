using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("Paddock", typeof(Skill), typeof(int), typeof(InteractiveSkillRecord), typeof(InteractiveObject))]
    public class SkillPaddock : Skill
    {
        public SkillPaddock(int id, InteractiveSkillRecord record, InteractiveObject interactiveObject)
            : base (id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return Record.IsConditionFilled(character);
        }

        public override void Execute(Character character)
        {
            character.Client.Send(new ExchangeStartOkMountMessage(new MountClientData[0], new MountClientData[0]));
        }
    }
}

using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("Revive", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillRevive : Skill
    {
        public SkillRevive(int id, InteractiveSkillTemplate skillTemplate, InteractiveObject interactiveObject) : base(id, skillTemplate, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return character.PlayerLifeStatus == PlayerLifeStatusEnum.STATUS_PHANTOM;
        }

        public override int StartExecute(Character character)
        {
            character.Energy = 1000;

            return 0;
        }
    }
}

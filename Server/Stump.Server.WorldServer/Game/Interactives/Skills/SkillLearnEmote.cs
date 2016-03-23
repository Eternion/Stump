using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("LearnEmote", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillLearnEmote : CustomSkill
    {
        public SkillLearnEmote(int id, InteractiveCustomSkillRecord skillTemplate, InteractiveObject interactiveObject)
            : base(id, skillTemplate, interactiveObject)
        {
        }

        public int EmoteId => Record.GetParameter<int>(0);

        public override int StartExecute(Character character)
        {
            character.AddEmote((EmotesEnum)EmoteId);

            return base.StartExecute(character);
        }
    }
}

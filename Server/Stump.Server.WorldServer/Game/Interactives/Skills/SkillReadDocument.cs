using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("ReadDocument", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillReadDocument : CustomSkill
    {
        public SkillReadDocument(int id, InteractiveCustomSkillRecord skillTemplate, InteractiveObject interactiveObject)
            : base(id, skillTemplate, interactiveObject)
        {
        }

        public short DocumentId => Record.GetParameter<short>(0);

        public override int StartExecute(Character character)
        {
            character.Client.Send(new DocumentReadingBeginMessage(DocumentId));

            return base.StartExecute(character);
        }
    }
}

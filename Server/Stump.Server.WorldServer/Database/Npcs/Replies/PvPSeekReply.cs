using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using System.Linq;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("PvPSeek", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class PvPSeekReply : NpcReply
    {
        public PvPSeekReply(NpcReplyRecord record)
            : base(record)
        {
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            var target = Game.World.Instance.GetCharacters(x => character.CanAgress(x, true) == FighterRefusedReasonEnum.FIGHTER_ACCEPTED).RandomElementOrDefault();

            if (target == null)
                return false;

            foreach (var contract in character.Inventory.GetItems(x => x.Template.Id == (int)ItemIdEnum.ORDRE_DEXECUTION_10085))
                character.Inventory.RemoveItem(contract);

            var item = ItemManager.Instance.CreatePlayerItem(character, (int)ItemIdEnum.ORDRE_DEXECUTION_10085, 25);

            var seekEffect = item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_Seek);

            if (seekEffect != null)
                item.Effects.Remove(seekEffect);

            item.Effects.Add(new EffectString(EffectsEnum.Effect_Seek, target.Name));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_Alignment, (short)target.AlignmentSide));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_Grade, target.AlignmentGrade));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_Level, target.Level));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_NonExchangeable_981, 0));

            character.Inventory.AddItem(item);

            return true;
        }
    }
}

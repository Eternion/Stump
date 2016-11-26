using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(ItemIdEnum.BOUCLIER_DE_GUILDE_13240)]
    public class GuildShieldItem : BasePlayerItem
    {
        public GuildShieldItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override ActorLook UpdateItemSkin(ActorLook characterLook)
        {
            if (IsEquiped())
            {
                if (Owner.Guild != null)
                {
                    characterLook.AddSkin(1730); //New ApparenceId

                    characterLook.AddSkin((short)Owner.Guild.Emblem.Template.SkinId); //Emblem Skin

                    characterLook.AddColor(7, Owner.Guild.Emblem.SymbolColor);
                    characterLook.AddColor(8, Owner.Guild.Emblem.BackgroundColor);
                }
            }
            else
            {
                 characterLook.RemoveSkin(1730); //New ApparenceId
                
                if (Owner.Guild != null)
                {
                    characterLook.RemoveSkin((short)Owner.Guild.Emblem.Template.SkinId); //Emblem Skin

                    characterLook.RemoveColor(7);
                    characterLook.RemoveColor(8);
                }
            }
            return base.UpdateItemSkin(characterLook);
        }
    }
}
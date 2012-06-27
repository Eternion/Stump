using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Plugins.DefaultPlugin.Items
{
    public class WeaponEffectsFix
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Initialization(typeof(ItemManager), Silent = true)]
        public static void ApplyFix()
        {
            logger.Debug("Apply weapons effects fix");

            foreach (var template in ItemManager.Instance.GetTemplates().OfType<WeaponTemplate>())
            {
                foreach (var effect in template.Effects)
                {
                    if (effect.Template.Category == 2)
                    {
                        if (template.TypeId == 7) // hammer
                        {
                            effect.ZoneShape = SpellShapeEnum.Hammer;
                            effect.ZoneSize = 1;
                        }
                        else if (template.TypeId == 4) // staff
                        {
                            effect.ZoneShape = SpellShapeEnum.T;
                            effect.ZoneSize = 1;
                        }
                    }
                }
            }


        }  
    }
}
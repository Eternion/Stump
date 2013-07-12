using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class ReloadCommands : CommandBase
    {
        public Dictionary<string, object> m_entries = new Dictionary<string, object>()
            {
                {"npcs", NpcManager.Instance},
                {"monsters", MonsterManager.Instance},
                {"items", ItemManager.Instance},
                {"world", World.Instance},
                {"spells", SpellManager.Instance},
                {"effects", EffectManager.Instance},
                {"interactives", InteractiveManager.Instance},
                {"breeds", BreedManager.Instance},
                {"experiences", ExperienceManager.Instance},
                {"langs", TextManager.Instance},

            };

        public ReloadCommands()
        {
            Aliases = new[] {"reload"};
            RequiredRole=RoleEnum.Administrator;
            Description = "Reload manager";
            AddParameter<string>("name", "n", "Name of the manager to reload", isOptional:true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (!trigger.IsArgumentDefined("name"))
            {
                trigger.Reply("Entries : " + string.Join(", ", m_entries.Keys));
                return;
            }

            var name = trigger.Get<string>("name").ToLower();
            object entry;

            if (!m_entries.TryGetValue(name, out entry))
            {
                trigger.ReplyError("{0} not a valid name.", name);
                trigger.ReplyError("Entries : " + string.Join(", ", m_entries.Keys));
                return;
            }
            var method = entry.GetType().GetMethod("Initialize", new Type[0]);

            if (method == null)
            {
                trigger.ReplyError("Cannot reload {0} : method Initialize() not found", name);
                return;
            }

            World.Instance.SendAnnounce("[RELOAD] Reloading " + name + " ... WORLD PAUSED", Color.DodgerBlue);
            Task.Factory.StartNew(() =>
                {
                    World.Instance.Pause();
                    try
                    {
                        method.Invoke(entry, new object[0]);
                    }
                    finally
                    {
                        World.Instance.Resume();
                    }

                    World.Instance.SendAnnounce("[RELOAD] " + name + " reloaded ... WORLD RESUMED", Color.DodgerBlue);
                });
        }
    }
}

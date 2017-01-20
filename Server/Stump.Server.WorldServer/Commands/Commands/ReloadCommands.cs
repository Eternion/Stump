using System;
using System.Collections.Generic;
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
using Stump.Server.WorldServer.Game.Social;
using Stump.Server.WorldServer.Game.Idols;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class ReloadCommands : CommandBase
    {
        public Dictionary<string, Action> m_entries = new Dictionary<string, Action>
            {
                {"npcs", NpcManager.Instance.Initialize},
                {"monsters", MonsterManager.Instance.Initialize},
                {"items", ItemManager.Instance.Initialize},
                {"world", World.Instance.Reload},
                {"effects", EffectManager.Instance.Initialize},
                {"interactives", InteractiveManager.Instance.Initialize},
                {"breeds", BreedManager.Instance.Initialize},
                {"experiences", ExperienceManager.Instance.Initialize},
                {"langs", TextManager.Instance.Initialize},
                {"badwords", ChatManager.Instance.Initialize},
                {"idols", IdolManager.Instance.Initialize},
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

            if (!m_entries.TryGetValue(name, out var entry))
            {
                trigger.ReplyError("{0} not a valid name.", name);
                trigger.ReplyError("Entries : " + string.Join(", ", m_entries.Keys));
                return;
            }
            var method = entry;

            if (method == null)
            {
                trigger.ReplyError("Cannot reload {0} : method not found", name);
                return;
            }

            trigger.ReplyBold($"[RELOAD] Reloading {name} ... WORLD PAUSED");

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
                {
                    try
                    {
                        method.Invoke();
                    }
                    finally
                    {
                    }

                    trigger.ReplyBold($"[RELOAD] {name} reloaded ... WORLD RESUMED");
                });
        }
    }
}

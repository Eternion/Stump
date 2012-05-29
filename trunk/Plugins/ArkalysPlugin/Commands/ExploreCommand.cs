using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace ArkalysPlugin.Commands
{
    public class ExploreCommand : InGameCommand
    {
        [Variable(true)]
        public static ExplorationEntry[] ExplorationEntries = new ExplorationEntry[]
        {
            new ExplorationEntry("ankama", 0, 230),
            new ExplorationEntry("incarnam", 80216068, 283),
            new ExplorationEntry("astrub", 84674563, 315),
            new ExplorationEntry("pandala", 13605, 227),
            new ExplorationEntry("grobe", 17716, 245),
            new ExplorationEntry("bonta", 147768, 272),
            new ExplorationEntry("brakmar", 144419, 216),
            new ExplorationEntry("vulkania", 76283906, 498),
            new ExplorationEntry("otomai", 154642, 271),
            new ExplorationEntry("frigost", 54172969, 286),
            new ExplorationEntry("nowel", 66062340, 245),
            new ExplorationEntry("wabbits", 13060, 173),
            new ExplorationEntry("moon", 17932, 172),
            new ExplorationEntry("prison", 69604354, 412),
        };

        public ExploreCommand()
        {
            Aliases = new [] { "explore" };
            RequiredRole = RoleEnum.Player;
            Description = "Téléporte vers une zone. Tapez '.explore' pour la liste des zones";
            AddParameter<string>("name", "n", "Nom de la zone", isOptional:true);
        }

        public override void Execute(GameTrigger trigger)
        {
            if (!trigger.IsArgumentDefined("name"))
            {
                foreach (var entry in ExplorationEntries)
                {
                    trigger.Reply(" - {0}", entry.Name);
                }
            }
            else
            {
                var name = trigger.Get<string>("name");
                var entry = ExplorationEntries.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                if (entry == null)
                {
                    trigger.ReplyError("La zone '{0}' n'existe pas, tapez '.explore' pour la liste des zones", name);
                }
                else
                {
                    trigger.Character.Teleport(entry.Position);
                }
            }
        }
    }

    public class ExplorationEntry
    {
        private ObjectPosition m_position;

        public ExplorationEntry(string name, int mapId, short cellId)
        {
            Name = name;
            MapId = mapId;
            CellId = cellId;
        }

        public ExplorationEntry()
        {
        }

        public string Name
        {
            get;
            set;
        }

        public int MapId
        {
            get;
            set;
        }

        public short CellId
        {
            get;
            set;
        }

        [XmlIgnore]
        public ObjectPosition Position
        {
            get { return m_position ?? (m_position = new ObjectPosition(World.Instance.GetMap(MapId), CellId)); }
            set
            {
                m_position = value;
                MapId = value.Map.Id;
                CellId = value.Cell.Id;
            }
        }
    }
}
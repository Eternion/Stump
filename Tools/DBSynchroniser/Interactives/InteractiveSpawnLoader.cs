using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;
using Stump.DofusProtocol.D2oClasses.Tools.Ele;
using Stump.DofusProtocol.D2oClasses.Tools.Ele.Datas;
using Stump.ORM.SubSonic.Linq.Structure;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace DBSynchroniser.Interactives
{
    public class InteractiveSpawnLoader
    {
        public static void LoadSpawns()
        {
            Console.WriteLine("Generating interactive spawns");
            var worldDatabase = Program.ConnectToWorld();
            if (worldDatabase == null)
                return;

            worldDatabase.Database.Execute("DELETE FROM interactives_spawns");
            worldDatabase.Database.Execute("ALTER TABLE interactives_spawns AUTO_INCREMENT=1");
            string eleFilePath = Path.Combine(Program.FindDofusPath(), "content", "maps", "elements.ele");
            string mapsfilePath = Path.Combine(Program.FindDofusPath(), "content", "maps", "maps0.d2p");

            var eleFile = new EleReader(eleFilePath);
            var eleInstance = eleFile.ReadElements();
            var d2pFile = new D2pFile(mapsfilePath);
            var entries = d2pFile.ReadAllFiles();
            var i = 0;
            var ids = new List<int>();
            var failures = new List<int>();
            var spawns = new Dictionary<int, InteractiveSpawn>();
            var elementsGlobal = new Dictionary<int, DlmGraphicalElement>();
            var identifiableElementsByMap = new Dictionary<int, List<IdentifiableElement>>();

            int fails = 0;
            Console.WriteLine("Loading maps ... ");
            Program.InitializeCounter();
            foreach (var mapBytes in entries.Values)
            {
                DlmReader mapFile;
                DlmMap map = null;
                try
                {
                    mapFile = new DlmReader(mapBytes) { DecryptionKey = Program.MapDecryptionKey };
                    map = mapFile.ReadMap();
                }
                catch (Exception)
                {
                    fails++;
                    continue;
                }
                var elements = (from layer in map.Layers
                    from cell in layer.Cells
                    from element in cell.Elements.OfType<DlmGraphicalElement>()
                    where element.Identifier != 0
                    let point = new MapPoint(cell.Id)
                    select new IdentifiableElement(element, map)).ToList();

                identifiableElementsByMap.Add(map.Id, elements);
                Program.UpdateCounter(i++, entries.Count);
            }
            Program.EndCounter();

            var identifiableElements = identifiableElementsByMap.Values.SelectMany(x => x).
                GroupBy(x => x.Element.Identifier).ToDictionary(x => x.Key, x => x.ToList());

            
            Program.InitializeCounter();
            i = 0;
            // every elements grouped by the same id
            foreach (var keyPair in identifiableElements)
            {
                var likelyElement = keyPair.Value.Where(x => !x.Ignore)
                    .OrderBy(x => Math.Abs(x.Element.PixelOffset.X) + Math.Abs(x.Element.PixelOffset.Y)).First();


                var eleElement = eleInstance.GraphicalDatas[(int) likelyElement.Element.ElementId];
                InteractiveSpawn spawn;

                spawn = new InteractiveSpawn
                {
                    Id = (int) likelyElement.Element.Identifier,
                    MapId = likelyElement.Map.Id,
                    CellId = likelyElement.Element.Cell.Id,
                    Animated = eleElement is AnimatedGraphicalElementData || eleElement is EntityGraphicalElementData,
                    ElementId = (int) likelyElement.Element.ElementId
                };

                if (ids.Contains(spawn.Id))
                {
                    Console.WriteLine($"Id {spawn.Id} already added");
                    failures.Add(spawn.Id);
                    continue;
                }

                ids.Add(spawn.Id);
                worldDatabase.Database.Insert("interactives_spawns", "Id", false, spawn);
                spawns.Add(spawn.Id, spawn);
                elementsGlobal.Add(spawn.Id, likelyElement.Element);

                Program.UpdateCounter(i++, identifiableElements.Count);
            }
            
            Program.EndCounter();

            if (fails > 0)
                Console.WriteLine($"{fails} failes !");

            Program.ExecutePatch("./Patchs/interactives_spawns_patch.sql", worldDatabase.Database);
        }
    }
}